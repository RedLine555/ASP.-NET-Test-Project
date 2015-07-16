using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork3Data
{
    public class UserRepository : GenericRepository<DataModel.User>, IUserRepository
    {
        public UserRepository(DbContext cont) : base(cont)
        {
        }

        public override void Edit(DataModel.User item)
        {
            _entities.Entry(item).State = EntityState.Modified;
            if (item.Image != null)
            _entities.Entry(item.Image).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public override void Delete(DataModel.User item)
        {
            _entities.Set<DataModel.Image>().Remove(item.Image);
            base.Delete(item);
        }

        public bool isActive(int id)
        {
            DataModel.User us = _entities.Set<DataModel.User>().Find(id);
            _entities.Entry(us).State = EntityState.Detached;
            return us.IsActive;
        }
    }
}
