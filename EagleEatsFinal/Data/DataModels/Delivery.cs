using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EagleEatsFinal.Data
{
    public enum DeliveryStatus
    {
        Delivered, Late, Missed
    }
    public enum RequestStatus
    {
        Pending, Accepted, Rejected
    }
    public class Delivery
    {
        [Key]
        public int Delivery_Id { get; set; }
        public User Driver { get; set; }
        public DeliveryRoute Route { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public DateTime ETA { get; set; }
        public decimal TotalCost { get; set; }
        public decimal Tip { get; set; }
        public decimal Tax { get; set; }


    }
}
