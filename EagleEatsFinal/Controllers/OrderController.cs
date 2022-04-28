using EagleEatsFinal.Models;
using EagleEatsFinal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace EagleEatsFinal.Controllers
{
    [Route("Order/")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }



        [Route("Request/Item")]
        public async Task<IActionResult> RequestItem()
        {
            
            return View();
        }

        [HttpPost]
        [Route("Request/Item")]
        public async Task<IActionResult> RequestItem(Item? item) 
        {
            if (item is null) {ViewData["Error"] = "No item was requested for order"; return RedirectToAction("RequestItem");}
            System.Diagnostics.Debug.WriteLine("hello");
            return RequestRoute(item);


        }
        
        


        [Route("Request/Route")]
        public IActionResult RequestRoute(Item? item)
        {
            if (item is null) {ViewData["Error"] = "No item was requested for order"; return RedirectToAction("RequestItem");}
            ViewData["Item"] = item;
            
            return View();
        }

        [HttpPost]
        [Route("Request/Route")]
        public async Task<IActionResult> PostRequestRoute(Item item)
        {
            ViewData["Item"] = item;
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


    }
}