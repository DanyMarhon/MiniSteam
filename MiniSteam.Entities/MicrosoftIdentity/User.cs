using MiniSteam.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MiniSteam.Entities.MicrosoftIdentity
{
    public class User : IdentityUser<Guid>
    {
        [Required(ErrorMessage = "{0} Required")]
        [StringLength(100)]
        [PersonalData]
        public string Names { get; set; }
        [Required(ErrorMessage = "{0} Required")]
        [StringLength(100)]
        [PersonalData]
        public string Surname { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
    }
}
