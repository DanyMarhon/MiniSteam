using MiniSteam.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MiniSteam.Entities
{
    public class Publisher : IEntity
    {
        #region Constructors
        public Publisher() { }

        public Publisher(string name)
        {
            SetName(name);
        }
        #endregion

        #region Properties
        public int Id { get; set; } // público para cumplir con IEntity y EF

        [StringLength(250)]
        public string Name { get; private set; }
        #endregion

        #region Setters y Getters controlados
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del publicador no puede estar vacío.");

            if (name.Length > 250)
                throw new ArgumentException("El nombre del publicador no puede tener más de 250 caracteres.");

            Name = name.Trim();
        }

        public string GetName()
        {
            return Name;
        }
        #endregion
    }
}
