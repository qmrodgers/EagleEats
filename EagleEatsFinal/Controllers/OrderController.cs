using EagleEatsFinal.Models;
using EagleEatsFinal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace EagleEatsFinal.Controllers
{
    [Route("Order/")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger, ApplicationDbContext db, UserManager<User> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Request Item
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// 

        [Route("Request/Item")]
        public async Task<IActionResult> RequestItem()
        {

            return View();
        }

        [HttpPost]
        [Route("Request/Item")]
        public async Task<IActionResult> RequestItem(Item item)
        {
            if (item is null) { ViewData["Error"] = "No item was requested for order"; return RedirectToAction("RequestItem"); }
            System.Diagnostics.Debug.WriteLine("hello");
            return RedirectToAction("RequestRoute", item);


        }



        [Route("Request/Route")]
        public IActionResult RequestRoute(Item item)
        {
            if (item is null) { ViewData["Error"] = "No item was requested for order"; return RedirectToAction("RequestItem"); }
            ViewData["Item"] = item;
            return View();
        }

        [HttpPost]
        [Route("Request/Route")]
        public async Task<IActionResult> RequestRoute(DeliveryRoute route)
        {
            route.RequestTime = DateTime.Now;
            ViewData["Item"] = route.Item;
            try
            {
                route.Sender = _userManager.GetUserAsync(User).Result;
                ModelState["Sender"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
                ModelState["Sender"].Errors.Clear();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error retrieving user", "We were unable to associate your user account with this request. Please try again later");
            }
            if (route.RequestedDeliveryTime.CompareTo(route.RequestedPickupTime) < 0)
            {
                ModelState.AddModelError("RequestedDeliveryTime", "Requested Delivery Time must be after Requested Pickup Time");
            }
            else if (route.RequestedPickupTime.CompareTo(route.RequestTime) <= 0)
            {
                ModelState.AddModelError("RequestedPickupTime", "Pickup Request Time must be after the current time!");
            }

            route.RequestTime = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _db.DeliveryRoutes.Add(route);
                _db.SaveChanges();
                string success = $"Successfully added a request for the item: {route.Item.Name}";
                return RedirectToAction("Requests", new { successMessage = success });
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }

        [Route("Requests")]
        public async Task<IActionResult> Requests(string successMessage = "")
        {


            if (successMessage != "")
            {
                ViewData["Success"] = successMessage;
            }
            Debug.WriteLine(_db.Offers.Count());
            User user;
            try
            {
                user = _userManager.GetUserAsync(User).Result;
                if (user == null) throw new Exception();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Could not gather user information. Are you logged in?";
                return View();
            }

            return View(_db.DeliveryRoutes.Include(d => d.Item).Include(d => d.Offers).Where(d => d.Sender == user));
        }

        [Route("Offers")]
        public async Task<IActionResult> Offers(string rId)
        {


#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (_db.DeliveryRoutes
                .Include(d => d.Sender).Where(o => o.Route_Id == Convert.ToInt32(rId))
                .FirstOrDefault().Sender ==
                _userManager.GetUserAsync(User).Result)
            {

                return View(_db.DeliveryRoutes.Include(o => o.Offers).ThenInclude(x => x.User).Include(d => d.Item).Where(o => o.Route_Id == Convert.ToInt32(rId)).FirstOrDefault());

            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            ViewData["Error"] = "Couldn't pull list of offers for item";
            return View("Requests", _db.DeliveryRoutes.Include(d => d.Item).Include(d => d.Offers).Where(d => d.Sender == _userManager.GetUserAsync(User).Result));
        }


        [Route("Offers/Accept")]
        public async Task<IActionResult> AcceptOffer(string oId)
        {
            ViewData["Offer"] = _db.Offers.Where(o => o.Id == int.Parse(oId)).FirstOrDefault();
            return View();
        }

        [HttpPost]
        [Route("Offers/Accept")]
        public async Task<IActionResult> PostAcceptOffer(string oId)
        {
            Offer offer = _db.Offers.Include(o => o.User).Where(o => o.Id == int.Parse(oId)).FirstOrDefault();
            offer.offerStatus = OfferStatus.Accepted;
            


            _db.Offers.Update(offer);

            DeliveryRoute route = _db.DeliveryRoutes.Include(r => r.Item).Where(r => r.Offers.Contains(offer)).FirstOrDefault();

            foreach (var o in route.Offers)
            {
                if(o != offer)
                {
                    _db.Offers.Remove(o);
                }
            }

            route.Status = RequestStatus.Accepted;

            Delivery delivery = new Delivery(offer.price)
            {
                Driver = offer.User,
                Route = route,
                DeliveryStatus = DeliveryStatus.Pending,
                OrderTime = route.RequestTime,
                ETA = route.RequestedDeliveryTime

            };


            _db.Add(delivery);

            _db.SaveChanges();

            ViewData["Success"] = $"Successfully accepted offer for {route.Item.Name}";
            return View("Requests", _db.DeliveryRoutes.Include(d => d.Item).Where(d => d.Sender == _userManager.GetUserAsync(User).Result));
        }


        [Route("Offers/Reject")]
        public async Task<IActionResult> RejectOffer(string oId)
        {
            ViewData["Offer"] = _db.Offers.Where(o => o.Id == int.Parse(oId)).FirstOrDefault();
            return View();
        }

        [HttpPost]
        [Route("Offers/Reject")]
        public async Task<IActionResult> PostRejectOffer(string oId)
        {
            Offer offer = _db.Offers.Where(o => o.Id == int.Parse(oId)).FirstOrDefault();
            offer.offerStatus = OfferStatus.Rejected;
            
            return View("Offers", _db.DeliveryRoutes.Include(o => o.Offers).Where(o => o.Offers.Contains(offer)).FirstOrDefault());
        }



        [Route("Requests/SearchRequests")]
        public IActionResult SearchRequests()
        {

            IEnumerable<DeliveryRoute> routes = _db.DeliveryRoutes
                .Include(x => x.Item)
                .Include(x => x.Sender)
                .Include(x => x.Offers)
                .Where(x => (from v in x.Offers where v.User == _userManager.GetUserAsync(User).Result select v).FirstOrDefault() == null)
                .Where(x => x.Sender != _userManager.GetUserAsync(User).Result);

            return View(routes);
        }

        [Route("Deliveries")]
        public IActionResult Deliveries()
        {

            IEnumerable<Delivery> deliveries = _db.Deliveries
                .Include(x => x.Route).ThenInclude(x => x.Item)
                .Include(x => x.Driver)
                .Where(x => x.DeliveryStatus == DeliveryStatus.Pending)
                .Where(x => x.Driver == _userManager.GetUserAsync(User).Result);

            return View(deliveries);
        }


        [Route("Requests/SearchRequests/Make-An-Offer")]
        public async Task<IActionResult> MakeAnOffer(string rId)
        {

            DeliveryRoute? route = await _db.DeliveryRoutes
                .Include(x => x.Item)
                .Include(x => x.Offers)
                .Where(x => x.Route_Id == int.Parse(rId))
                .FirstOrDefaultAsync();

            if (route is null)
            {
                ViewData["Error"] = "Could not find the specified route. Please try again later!";
                return View("SearchRequests", _db.DeliveryRoutes
                .Include(x => x.Item)
                .Include(x => x.Sender)
                .Include(x => x.Offers)
                .Where(x => (from v in x.Offers where v.User == _userManager.GetUserAsync(User).Result select v).FirstOrDefault() != null)
                .Where(x => x.Sender != _userManager.GetUserAsync(User).Result));
            }

            ViewData["Route"] = route;
            return View();
        }

        [HttpPost]
        [Route("Requests/SearchRequests/Make-An-Offer")]
        public async Task<IActionResult> MakeAnOffer(string rId, Offer offer)
        {

            User user = _userManager.GetUserAsync(User).Result;

            DeliveryRoute? route = await _db.DeliveryRoutes
                    .Include(x => x.Item)
                    .Include(x => x.Offers)
                    .Where(x => x.Route_Id == int.Parse(rId))
                    .FirstOrDefaultAsync();
            ViewData["Route"] = route;

            offer.User = user;
            ModelState["User"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            ModelState["User"].Errors.Clear();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if ((from x in route.Offers where x.User == user select x).FirstOrDefault() != null)
            {
                ViewData["Error"] = "Cannot make more than one offer on a route";
                return View("SearchRequests", _db.DeliveryRoutes
                .Include(x => x.Item)
                .Include(x => x.Sender)
                .Include(x => x.Offers)
                .Where(x => (from v in x.Offers where v.User == _userManager.GetUserAsync(User).Result select v).FirstOrDefault() != null)
                .Where(x => x.Sender != _userManager.GetUserAsync(User).Result));
            }

            route.Offers.Add(offer);
            _db.SaveChanges();
            Debug.WriteLine(_db.Offers.Count());
            Debug.WriteLine(route.Offers.Count());
            ViewData["Success"] = "successfully created offer!";
            return View("SearchRequests", _db.DeliveryRoutes
                .Include(x => x.Item)
                .Include(x => x.Sender)
                .Include(x => x.Offers)
                .Where(x => (from v in x.Offers where v.User == _userManager.GetUserAsync(User).Result select v).FirstOrDefault() == null)
                .Where(x => x.Sender != _userManager.GetUserAsync(User).Result));



        }


        [Route("Route/Delete")]
        public async Task<IActionResult> DeleteRoute(string rId, string delete = "false")
        {
            if (delete == "true")
            {
                try
                {
                    var route = _db.DeliveryRoutes.Include(x => x.Item).Where(r => r.Route_Id == int.Parse(rId)).FirstOrDefault();
                    _db.DeliveryRoutes.Remove(route);
                    _db.SaveChanges();
                    string success = $"Successfully removed route: {rId}";
                    return RedirectToAction("Requests", new { successMessage = success });
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = ex.Message + "Could not delete route";
                    return View();
                }
            }


            ViewData["Route"] = rId;
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }


        [HttpGet]
        [Route("Request/Api")]
        public async Task<JsonResult> MapApiRequest()
        {

            HttpClient client = new HttpClient();
            string key = "f79c7ab2a03b1ffc7189b74256c8435e";

            return Json("hello");
        }



    }
}