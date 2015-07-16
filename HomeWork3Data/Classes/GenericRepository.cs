using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork3Data
{
    public abstract class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        protected DbContext _entities;
        public GenericRepository(DbContext cont)
    {
                _entities = cont;
    }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> enumer = _entities.Set<T>();
            return enumer;
        }

        public virtual T FindByID(int id)
        {
            return _entities.Set<T>().Find(id);
        }

        public virtual void Add(T item)
        {
            _entities.Set<T>().Add(item);
            _entities.SaveChanges();
        }

        public virtual void Delete(T item)
        {
            _entities.Set<T>().Remove(item);
            _entities.SaveChanges();
        }

        public virtual void Edit(T item)
        {
            _entities.Entry(item).State = EntityState.Modified;
            _entities.SaveChanges();
        }

    }
}
