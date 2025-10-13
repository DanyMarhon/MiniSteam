namespace MiniSteam.Application.Dtos.Game
{
    public class GameResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int IdPublisher { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public string Platform { get; set; }
        public string CoverImageUrl { get; set; }
    }
}
