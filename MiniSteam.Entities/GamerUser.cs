using MiniSteam.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace MiniSteam.Entities
{
    public class GamerUser : IEntity
    {
        #region Constructors
        public GamerUser()
        {
            Name = string.Empty;
            Email = string.Empty;
            PasswordHash = string.Empty;
        }

        public GamerUser(string name, string email, DateTime dateOfBirth, string password)
            : this()
        {
            SetName(name);
            SetEmail(email);
            SetDateOfBirth(dateOfBirth);
            SetPassword(password);
        }
        #endregion

        #region Properties
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; private set; }

        [StringLength(150)]
        [EmailAddress]
        public string Email { get; private set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; private set; }

        [StringLength(256)]
        public string PasswordHash { get; private set; }
        #endregion

        #region Controlled Setters and Utility Methods
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            if (name.Length > 50)
                throw new ArgumentException("Name cannot exceed 50 characters.");

            Name = name.Trim();
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.");

            if (email.Length > 150)
                throw new ArgumentException("Email cannot exceed 150 characters.");

            if (!email.Contains("@") || !email.Contains("."))
                throw new ArgumentException("Email format is invalid.");

            Email = email.Trim();
        }

        public void SetDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth > DateTime.Now)
                throw new ArgumentException("Date of birth cannot be in the future.");

            DateOfBirth = dateOfBirth;
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            if (password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters.");

            PasswordHash = HashPassword(password);
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }

        public bool VerifyPassword(string password)
        {
            return PasswordHash == HashPassword(password);
        }
        #endregion
    }
}
