using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EagleEats.Data
{
    public class User: IdentityUser
    {
        [Key]
        public int User_Id { get; set; }
        public string Address { get; set; }
       
    }
}
