using MiniSteam.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MiniSteam.Entities
{
    public class Genre : IEntity
    {
        public Genre()
        {
            GenresPerGames = new HashSet<GenrePerGame>();
        }

        public Genre(string name)
        {
            SetName(name);
            GenresPerGames = new HashSet<GenrePerGame>();
        }

        #region Properties
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; private set; }

        public virtual ICollection<GenrePerGame> GenresPerGames { get; set; }
        #endregion

        #region Setters y Getters
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del género no puede estar vacío.");

            if (name.Length > 50)
                throw new ArgumentException("El nombre del género no puede tener más de 50 caracteres.");

            Name = name;
        }

        public string GetName()
        {
            return Name;
        }
        #endregion
    }
}
