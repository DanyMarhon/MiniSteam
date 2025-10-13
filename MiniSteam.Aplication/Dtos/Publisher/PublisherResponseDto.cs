using System.ComponentModel.DataAnnotations;

namespace MiniSteam.Application.Dtos.Publisher
{
    public class PublisherResponseDto
    {
        public int Id { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
    }
}
