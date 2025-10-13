using System.ComponentModel.DataAnnotations;

namespace MiniSteam.Application.Dtos.Publisher
{
    public class PublisherRequestDto
    {
        public int Id { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
    }
}
