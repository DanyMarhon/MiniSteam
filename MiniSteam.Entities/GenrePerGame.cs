using MiniSteam.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSteam.Entities
{
    public class GenrePerGame : IEntity
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Genre))]
        public int IdGenre { get; set; }
        [ForeignKey(nameof(Game))]
        public int IdGame { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual Game Game { get; set; }
    }
}
