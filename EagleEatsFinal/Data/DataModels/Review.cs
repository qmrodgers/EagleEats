using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EagleEatsFinal.Data
{
    public class Review
    {
        [Key]
        public int Review_Id { get; set; }
        public int Reviewed_User_Id { get; set; }

        public int Reviewer_Id { get; set; }
        public string Comment { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal Score { get; set; }
    }
}
