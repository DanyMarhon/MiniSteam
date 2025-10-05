using MiniSteam.Abstractions;

namespace MiniSteam.Entities
{
    public class Genre : IEntity
    {
        public Genre()
        {
            GenresPerGames = new HashSet<GenrePerGame>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<GenrePerGame> GenresPerGames { get; set; }
    }
}
