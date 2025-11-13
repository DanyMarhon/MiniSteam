using MiniSteam.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniSteam.Entities
{
    public class Game : IEntity
    {
        #region Constructors
        public Game()
        {
            GenrePerGames = new HashSet<GenrePerGame>();
            PlatformPerGames = new HashSet<PlatformPerGame>();

            Title = string.Empty;
            CoverImageUrl = string.Empty;
            Publisher = null;
        }

        public Game(string title, int idPublisher, DateTime releaseDate, decimal price, string coverImageUrl)
            : this()
        {
            SetTitle(title);
            SetIdPublisher(idPublisher);
            SetReleaseDate(releaseDate);
            SetPrice(price);
            SetCoverImageUrl(coverImageUrl);
        }
        #endregion

        #region Properties
        public int Id { get; set; }

        [StringLength(150)]
        public string Title { get; private set; }

        [StringLength(1000)]
        public string? Description { get; private set; }

        [ForeignKey(nameof(Publisher))]
        public int IdPublisher { get; private set; }

        public virtual Publisher? Publisher { get; private set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; private set; }

        [Range(0, 999999.99)]
        [Precision(18, 2)]
        public decimal Price { get; private set; }

        [StringLength(500)]
        public string CoverImageUrl { get; private set; }

        public virtual ICollection<GenrePerGame> GenrePerGames { get; private set; }
        public virtual ICollection<PlatformPerGame> PlatformPerGames { get; private set; }
        #endregion

        #region Setters y Getters controlados
        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("El título del juego no puede estar vacío.");

            if (title.Length > 150)
                throw new ArgumentException("El título no puede tener más de 150 caracteres.");

            Title = title.Trim();
        }

        public void SetDescription(string? description)
        {
            if (description != null && description.Length > 1000)
                throw new ArgumentException("La descripción no puede tener más de 1000 caracteres.");

            Description = description?.Trim();
        }

        public void SetIdPublisher(int idPublisher)
        {
            if (idPublisher <= 0)
                throw new ArgumentException("El Id del publicador debe ser mayor que 0.");
            IdPublisher = idPublisher;
        }

        public void SetPublisher(Publisher publisher)
        {
            if (publisher is null)
                throw new ArgumentNullException(nameof(publisher), "El publicador no puede ser nulo.");

            Publisher = publisher;
            IdPublisher = publisher.Id;
        }

        public void SetReleaseDate(DateTime releaseDate)
        {
            if (releaseDate > DateTime.Now.AddYears(5))
                throw new ArgumentException("La fecha de lanzamiento no puede ser más de 5 años en el futuro.");

            ReleaseDate = releaseDate;
        }

        public void SetPrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException("El precio no puede ser negativo.");

            // Normalizar a 2 decimales para evitar problemas de precisión al persistir en la BD
            Price = decimal.Round(price, 2, MidpointRounding.AwayFromZero);
        }

        public void SetCoverImageUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("La URL de la imagen de portada no puede estar vacía.");

            if (url.Length > 500)
                throw new ArgumentException("La URL de la imagen no puede tener más de 500 caracteres.");

            CoverImageUrl = url.Trim();
        }

        public string GetTitle() => Title;
        public string? GetDescription() => Description;
        public decimal GetPrice() => Price;
        public string GetCoverImageUrl() => CoverImageUrl;
        #endregion
    }
}

