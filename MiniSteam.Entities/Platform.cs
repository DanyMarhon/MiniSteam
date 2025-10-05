using MiniSteam.Abstractions;

namespace MiniSteam.Entities
{
    public class Platform : IEntity
    {
        public Platform()
        {
            PlatformPerGames = new HashSet<PlatformPerGame>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<PlatformPerGame> PlatformPerGames { get; set; }
    }
}
