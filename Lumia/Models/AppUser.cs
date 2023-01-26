using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Lumia.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(maximumLength: 50)]
        public string FullName  { get; set; }
    }
}
