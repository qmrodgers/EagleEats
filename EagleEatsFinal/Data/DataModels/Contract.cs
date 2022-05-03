using System.ComponentModel.DataAnnotations;

namespace EagleEatsFinal.Data
{
    public class Contract
    {
        [Key]
        public int Id { get; set; }
        public DeliveryRoute ContractedRoute { get; set; }
        public bool DelivererConfirmedDelivery { get; set; }
        public bool ReceiverConfirmedDelivery { get; set; }

    }
}
