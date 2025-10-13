using MiniSteam.Abstractions;

namespace MiniSteam.Entities
{
    public class Game : IEntity
    {
        public Game()
        {
            GenrePerGames = new HashSet<GenrePerGame>();
            PlatformPerGames = new HashSet<PlatformPerGame>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int IdPublisher { get; set; }
        public virtual Publisher Publisher { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public string CoverImageUrl { get; set; }

        public virtual ICollection<GenrePerGame> GenrePerGames { get; set; }
        public virtual ICollection<PlatformPerGame> PlatformPerGames { get; set; }
    }
}
