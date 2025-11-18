using MiniSteam.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSteam.Entities
{
    public class GenrePerGame : IEntity
    {
        #region Constructors
        public GenrePerGame() { }

        public GenrePerGame(int idGenre, int idGame)
        {
            SetIdGenre(idGenre);
            SetIdGame(idGame);
        }

        public GenrePerGame(Genre genre, Game game)
        {
            SetGenre(genre);
            SetGame(game);
        }
        #endregion

        #region Properties
        public int Id { get; set; }

        [ForeignKey(nameof(Genre))]
        public int IdGenre { get; private set; }

        [ForeignKey(nameof(Game))]
        public int IdGame { get; private set; }

        public virtual Genre Genre { get; private set; }
        public virtual Game Game { get; private set; }
        #endregion

        #region Setters controlados
        public void SetIdGenre(int idGenre)
        {
            if (idGenre <= 0)
                throw new ArgumentException("El Id del género debe ser mayor que 0.");
            IdGenre = idGenre;
        }

        public void SetIdGame(int idGame)
        {
            if (idGame <= 0)
                throw new ArgumentException("El Id del juego debe ser mayor que 0.");
            IdGame = idGame;
        }

        public void SetGenre(Genre genre)
        {
            if (genre is null)
                throw new ArgumentNullException(nameof(genre), "El género no puede ser nulo.");

            Genre = genre;
            IdGenre = genre.Id;
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
