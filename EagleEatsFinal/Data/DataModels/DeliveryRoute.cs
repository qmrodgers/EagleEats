using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EagleEatsFinal.Data
{
    public class DeliveryRoute
    {
        [Key]
        public int Route_Id { get; set; }
        public User Sender { get; set; }

        public User Receiver { get; set; }
        public Item Item { get; set; }
        public string? StartLocation { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public string? EndLocation { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal RouteCost { get; set; }
        public decimal RouteDistance { get; set; }
    }
}