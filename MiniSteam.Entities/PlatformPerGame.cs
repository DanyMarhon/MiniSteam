using MiniSteam.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSteam.Entities
{
    public class PlatformPerGame : IEntity
    {
        #region Constructors
        public PlatformPerGame() { }

        public PlatformPerGame(int idPlatform, int idGame)
        {
            SetIdPlatform(idPlatform);
            SetIdGame(idGame);
        }

        public PlatformPerGame(Platform platform, Game game)
        {
            SetPlatform(platform);
            SetGame(game);
        }
        #endregion

        #region Properties
        public int Id { get; set; }

        [ForeignKey(nameof(Platform))]
        public int IdPlatform { get; private set; }

        [ForeignKey(nameof(Game))]
        public int IdGame { get; private set; }

        public virtual Platform Platform { get; private set; }
        public virtual Game Game { get; private set; }
        #endregion

        #region Setters controlados
        public void SetIdPlatform(int idPlatform)
        {
            if (idPlatform <= 0)
                throw new ArgumentException("El Id de la plataforma debe ser mayor que 0.");
            IdPlatform = idPlatform;
        }

        public void SetIdGame(int idGame)
        {
            if (idGame <= 0)
                throw new ArgumentException("El Id del juego debe ser mayor que 0.");
            IdGame = idGame;
        }

        public void SetPlatform(Platform platform)
        {
            if (platform is null)
                throw new ArgumentNullException(nameof(platform), "La plataforma no puede ser nula.");

            Platform = platform;
            IdPlatform = platform.Id;
        }

        public void SetGame(Game game)
        {
            if (game is null)
                throw new ArgumentNullException(nameof(game), "El juego no puede ser nulo.");

            Game = game;
            IdGame = game.Id;
        }
        #endregion
    }
}
