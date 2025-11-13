using MiniSteam.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MiniSteam.Entities
{
    public class Platform : IEntity
    {
        #region Constructors
        public Platform()
        {
            PlatformPerGames = new HashSet<PlatformPerGame>();
        }

        public Platform(string name)
        {
            SetName(name);
            PlatformPerGames = new HashSet<PlatformPerGame>();
        }
        #endregion

        #region Properties
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; private set; }

        public virtual ICollection<PlatformPerGame> PlatformPerGames { get; private set; }
        #endregion

        #region Setters y Getters controlados
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre de la plataforma no puede estar vacío.");

            if (name.Length > 50)
                throw new ArgumentException("El nombre de la plataforma no puede tener más de 50 caracteres.");

            Name = name;
        }

        public string GetName()
        {
            return Name;
        }
        #endregion
    }
}
