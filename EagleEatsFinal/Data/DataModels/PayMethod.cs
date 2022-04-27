using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EagleEats.Data
{
    public class PayMethod
    {
        [Key]
        public int Method_Id { get; set; }
        public User User { get; set; }

        public string Information { get; set; }

        public string ProviderName { get; set; }


    }
}
