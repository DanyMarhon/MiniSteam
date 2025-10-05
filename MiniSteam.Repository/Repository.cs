using MiniSteam.Abstractions;

namespace MiniSteam.Repository
{
    public interface IRepository<T> : IDbOperation<T>
    {
    }
    public class Repository<T> : IRepository<T> where T : class
    {
        IDbContext<T> _db;
        public Repository(IDbContext<T> db)
        {
            _db = db;
        }
        public void Delete(int id)
        {
            _db.Delete(id);
        }

        public IList<T> GetAll()
        {
            return _db.GetAll();
        }

        public T GetById(int id)
        {
            return _db.GetById(id);
        }

        public T Save(T entity)
        {
            return _db.Save(entity);
        }
    }
}
