using System;
namespace HomeWork3Business.Interfaces
{
    public interface IUserService
    {
        void Add(HomeWork3Data.DataModel.User item);
        HomeWork3Data.DataModel.User Contains(string login, string password);
        void Delete(int id);
        void Edit(HomeWork3Data.DataModel.User item);
        HomeWork3Data.DataModel.User FindByID(int id);
        System.Linq.IQueryable<HomeWork3Data.DataModel.User> GetAll();
    }
}
