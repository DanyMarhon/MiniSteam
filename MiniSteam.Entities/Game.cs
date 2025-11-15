using Microsoft.EntityFrameworkCore;
using MiniSteam.Abstractions;
using MiniSteam.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public Game(
            string title,
            int idPublisher,
            DateTime releaseDate,
            decimal price,
            string coverImageUrl,
            GameClassification classification)
            : this()
        {
            SetTitle(title);
            SetIdPublisher(idPublisher);
            SetReleaseDate(releaseDate);
            SetPrice(price);
            SetCoverImageUrl(coverImageUrl);
            SetClassification(classification);
        }
        #endregion

        #region Properties
        public int Id { get; private set; }

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

        /// <summary>
        /// PEGI Classification (3, 7, 12, 16, 18)
        /// </summary>
        public GameClassification Classification { get; private set; }

        public virtual ICollection<GenrePerGame> GenrePerGames { get; private set; }
        public virtual ICollection<PlatformPerGame> PlatformPerGames { get; private set; }
        #endregion

        #region Controlled Setters & Getters

        public void SetId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id must be greater than 0.");

            Id = id;
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Game title cannot be empty.");

            if (title.Length > 150)
                throw new ArgumentException("Game title cannot exceed 150 characters.");

            Title = title.Trim();
        }

        public void SetDescription(string? description)
        {
            if (description != null && description.Length > 1000)
                throw new ArgumentException("Description cannot exceed 1000 characters.");

            Description = description?.Trim();
        }

        public void SetIdPublisher(int idPublisher)
        {
            if (idPublisher <= 0)
                throw new ArgumentException("Publisher Id must be greater than 0.");

            IdPublisher = idPublisher;
        }

        public void SetPublisher(Publisher publisher)
        {
            if (publisher is null)
                throw new ArgumentNullException(nameof(publisher), "Publisher cannot be null.");

            Publisher = publisher;
            IdPublisher = publisher.Id;
        }

        public void SetReleaseDate(DateTime releaseDate)
        {
            if (releaseDate > DateTime.Now.AddYears(5))
                throw new ArgumentException("Release date cannot be more than 5 years in the future.");

            ReleaseDate = releaseDate;
        }

        public void SetPrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException("Price cannot be negative.");

            Price = decimal.Round(price, 2, MidpointRounding.AwayFromZero);
        }

        public void SetCoverImageUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Cover image URL cannot be empty.");

            if (url.Length > 500)
                throw new ArgumentException("Cover image URL cannot exceed 500 characters.");

            CoverImageUrl = url.Trim();
        }

        public void SetClassification(GameClassification classification)
        {
            if (!Enum.IsDefined(typeof(GameClassification), classification))
                throw new ArgumentException("Invalid game classification.");

            Classification = classification;
        }

        public GameClassification GetClassification() => Classification;
        public string GetTitle() => Title;
        public string? GetDescription() => Description;
        public decimal GetPrice() => Price;
        public string GetCoverImageUrl() => CoverImageUrl;
        #endregion
    }
}
