using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HomeWork3Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T FindByID(int id);
        void Add(T item);
        void Delete(T item);
        void Edit(T item);
    }
}
