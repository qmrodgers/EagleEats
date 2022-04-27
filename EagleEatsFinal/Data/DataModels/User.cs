using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EagleEatsFinal.Data
{
    public class User: IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Address { get; set; }
       
    }
}
