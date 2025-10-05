using Microsoft.EntityFrameworkCore;
using MiniSteam.Abstractions;

namespace MiniSteam.DataAccess
{
    public class DbContext<T> : IDbContext<T> where T : class, IEntity
    {
        DbSet<T> _Items;
        DbDataAccess _ctx;
        public DbContext(DbDataAccess ctx)
        {
            _ctx = ctx;
            _Items = _ctx.Set<T>();
        }

        public void Delete(int id)
        {
            var entity = _Items.FirstOrDefault(i => i.Id == id);
            if (entity != null) { _Items.Remove(entity); }
            _ctx.SaveChanges();
        }

        public IList<T> GetAll()
        {
            return _Items.ToList();
        }

        public T GetById(int id)
        {
            return _Items.FirstOrDefault(i => i.Id == id);
        }

        public T Save(T entity)
        {
            if (entity.Id.Equals(0))
            {
                _Items.Add(entity);
            }
            else
            {
                var entityDb = GetById(entity.Id);
                _ctx.Entry(entityDb).State = EntityState.Detached;
                _Items.Update(entity);
            }
            _ctx.SaveChanges();
            return entity;
        }
    }
}
