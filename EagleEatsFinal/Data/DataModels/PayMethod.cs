using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EagleEatsFinal.Data
{
    public class PayMethod
    {
        [Key]
        public int Method_Id { get; set; }

        public string Information { get; set; }

        public string ProviderName { get; set; }


    }
}
