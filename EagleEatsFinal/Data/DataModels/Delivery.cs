using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EagleEatsFinal.Data
{
    public enum DeliveryStatus
    {
        Delivered, Late, Missed, Pending
    }
    public class Delivery
    {
        [Key]
        public int Delivery_Id { get; set; }
        public User Driver { get; set; }
        public DeliveryRoute Route { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public DateTime ETA { get; set; }
        public decimal TotalCost { get; set; }
        public decimal? Tip { get; set; }
        public decimal Tax { get; set; }

        public Delivery()
        {

        }
        public Delivery(decimal itemprice)
        {
            Tax = 0.07m;
            TotalCost = itemprice + (itemprice * Tax) + 10;
        }
    }
}
