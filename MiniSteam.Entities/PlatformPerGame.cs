using MiniSteam.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSteam.Entities
{
    public class PlatformPerGame : IEntity
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Platform))]
        public int IdPlatform { get; set; }
        [ForeignKey(nameof(Game))]
        public int IdGame { get; set; }
        public virtual Platform Platform { get; set; }
        public virtual Game Game { get; set; }
    }
}
