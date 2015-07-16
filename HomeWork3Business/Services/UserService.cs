using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeWork3Data.DataModel;
using HomeWork3Data;
using HomeWork3Common.Helpers.Security;
using HomeWork3Common;
using HomeWork3Common.Services;
using HomeWork3Common.Loger;

namespace HomeWork3Business.Services
{
    public class UserService : HomeWork3Business.Interfaces.IUserService
    {
        public IUserRepository _userRepository;
        public IEmailService _emailService;
        public UserService(IUserRepository userRepository, IEmailService emailService)
        {
            _emailService = emailService;
            _userRepository = userRepository;
        }
        public IQueryable<User> GetAll()
        {
            /*EmailModel m = new EmailModel();
            m.To = "nik.evdik@mail.ru";
            m.From = "blackrivergames@gmail.com";
            m.Subject = "Test";
            m.Body = "TestBody";
            _emailService.SendEmail(m);*/
            NLogLogger logger2 = new NLogLogger();
            logger2.Info("testing");
            return _userRepository.GetAll();
        }
        public User FindByID(int id)
        {
            return _userRepository.FindByID(id);
        }
        public void Add(User item)
        {
            item.DateCreated = DateTime.Now;
            item.Password = MySecurity.GetHashString(item.Password);
            _userRepository.Add(item);

            EmailModel m = new EmailModel();
            m.To = item.Email.TrimEnd();
            m.From = "blackrivergames@gmail.com";
            m.Subject = "Registration";
            m.Body = "You have been successfuly registeren on our site!";
            _emailService.SendEmail(m);
        }
        public void Delete(int id)
        {
            var item = _userRepository.FindByID(id);
            _userRepository.Delete(item);
        }
        public void Edit(User item)
        {
            if (_userRepository.isActive(item.UId) && !item.IsActive)
            {
                EmailModel m = new EmailModel();
                m.To = item.Email.TrimEnd();
                m.From = "blackrivergames@gmail.com";
                m.Subject = "You are banned";
                m.Body = string.Format("Sorry, but you are banned on our site. Reason:{0}.",item.BlockDescription);
                _emailService.SendEmail(m);
            }
            _userRepository.Edit(item);
        }

        public User Contains(string login, string password)
        {
            string passwordHASH = MySecurity.GetHashString(password);
            return _userRepository.GetAll().ToList().Find(user => user.Login.TrimEnd() == login && user.Password.TrimEnd() == passwordHASH);
        }
    }
}
