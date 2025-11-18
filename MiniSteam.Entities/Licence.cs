using MiniSteam.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSteam.Entities
{
    public class Licence : IEntity
    {
        #region Constructors
        public Licence() { }

        public Licence(int idGamerUser, int idGame)
        {
            SetIdUser(idGamerUser);
            SetIdGame(idGame);
        }

        public Licence(GamerUser gamerUser, Game game)
        {
            SetGamerUser(gamerUser);
            SetGame(game);
        }
        #endregion

        #region Properties
        public int Id { get; set; }

        [ForeignKey(nameof(GamerUser))]
        public int IdGamerUser { get; private set; }

        public virtual GamerUser GamerUser { get; private set; }

        [ForeignKey(nameof(Game))]
        public int IdGame { get; private set; }

        public virtual Game Game { get; private set; }

        public Guid LicenceKey { get; private set; }
        #endregion

        #region Setters controlados
        public void SetIdUser(int idGamerUser)
        {
            if (idGamerUser <= 0)
                throw new ArgumentException("User Id must be greater than 0.");

            IdGamerUser = idGamerUser;
        }

        public void SetIdGame(int idGame)
        {
            if (idGame <= 0)
                throw new ArgumentException("Game Id must be greater than 0.");

            IdGame = idGame;
        }

        public void SetGamerUser(GamerUser gamerUser)
        {
            if (gamerUser is null)
                throw new ArgumentNullException(nameof(gamerUser));

            GamerUser = gamerUser;
            IdGamerUser = gamerUser.Id;
        }

        public void SetGame(Game game)
        {
            if (game is null)
                throw new ArgumentNullException(nameof(game));

            Game = game;
            IdGame = game.Id;
        }

        public Guid GenerateKey()
        {
            LicenceKey = Guid.NewGuid();
            return LicenceKey;
        }
        #endregion
    }
}

