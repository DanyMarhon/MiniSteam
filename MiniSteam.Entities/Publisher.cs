using MiniSteam.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MiniSteam.Entities
{
    public class Publisher : IEntity
    {
        public Publisher()
        {
        }
        public int Id { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
    }
}
