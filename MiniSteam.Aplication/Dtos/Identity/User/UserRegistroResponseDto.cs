using System.ComponentModel.DataAnnotations;

namespace MiniSteam.Application.Dtos.Identity.User
{
    public class UserRegistroResponseDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
