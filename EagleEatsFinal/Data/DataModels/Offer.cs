using System.ComponentModel.DataAnnotations;

namespace EagleEatsFinal.Data
{
    public enum OfferStatus {
        Pending, Accepted, Rejected
    }
    public class Offer
    {

        [Key]
        public int Id { get; set; }
        public decimal price { get; set; }
        public string? pitch { get; set; }
        public OfferStatus offerStatus { get; set; }

        public User User { get; set; }

        public Offer() {
            offerStatus = OfferStatus.Pending;
        }

    }
}
