using System.ComponentModel.DataAnnotations;

namespace MiniSteam.Application.Dtos.Identity.User
{
    public class UserRegistroRequestDto
    {
        [Required]
        public string Names { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
    }
}
