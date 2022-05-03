using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EagleEatsFinal.Data
{
    public enum RequestStatus
    {
        Pending,
        Accepted,
        InProgress,
        Delivered
    }
    public class DeliveryRoute
    {
        [Key]
        public int Route_Id { get; set; }
        public User Sender { get; set; }
        public RequestStatus Status { get; set; }
        public User? Receiver { get; set; }
        public Item Item { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime RequestedPickupTime { get; set; }
        public DateTime RequestedDeliveryTime { get; set; }
        public ICollection<Offer>? Offers { get; set; }

        public DeliveryRoute()
        {
            Status = RequestStatus.Pending;
        }
    }
}