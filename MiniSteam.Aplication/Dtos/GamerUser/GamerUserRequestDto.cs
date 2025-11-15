namespace MiniSteam.Application.Dtos.GamerUser
{
    public class GamerUserRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? Password { get; set; }
    }
}
