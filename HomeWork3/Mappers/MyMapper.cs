using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HomeWork3Data.DataModel;
using HomeWork3.Models;
using System.IO;

namespace HomeWork3.Mappers
{
    public class MyMapper
    {
        public static UserViewModel UserModelToView (User user)
        {
            UserViewModel viewmodel = new UserViewModel
            {
                UId = user.UId,
                Login = user.Login.TrimEnd(),
                Phone = user.Phone,
                Password = user.Password.TrimEnd(),
                FirstName = user.FirstName.TrimEnd(),
                LastName = user.LastName.TrimEnd(),
                IsActive = user.IsActive,
                BlockDescription = user.BlockDescription.TrimEnd(),
                Email = user.Email.TrimEnd(),
                BirthDay = user.BirthDay,
                ImageID = user.ImageID,
                DateCreated = user.DateCreated,
                Role = user.Role
            };

            return viewmodel;
        }

        public static User UserViewToModel(UserViewModel viewmodel)
        {
            User user = new User
            {
                UId = viewmodel.UId,
                Login = viewmodel.Login,
                Age = (DateTime.Now - viewmodel.BirthDay).Days / 365,
                Phone = viewmodel.Phone,
                Password = viewmodel.Password,
                FirstName = viewmodel.FirstName,
                LastName = viewmodel.LastName,
                IsActive = viewmodel.IsActive,
                ImageID = viewmodel.ImageID,
                Email = viewmodel.Email,
                BirthDay = viewmodel.BirthDay,
                DateCreated = viewmodel.DateCreated,
                DateUpdated = DateTime.Now,
                Role = viewmodel.Role
            };
            user.BlockDescription = (!user.IsActive) ? viewmodel.BlockDescription : "";

            Image img = new Image();
            if (viewmodel.Image != null)
            {
                img.IId = user.ImageID;
                img.ImageName = viewmodel.Image.FileName;

                MemoryStream target = new MemoryStream();
                viewmodel.Image.InputStream.CopyTo(target);
                img.ImageContent = target.ToArray();
                user.Image = img;
            }
            return user;
        }
    }
}