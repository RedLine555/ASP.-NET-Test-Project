using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace HomeWork3Data.DataMappers
{
    class DataMapper
    {
        public static DataModel.User DataToUser(IDataReader data)
        {
            DataModel.User user = new DataModel.User
            {
                UId = (int)data["UId"],
                Login = (string)data["Login"],
                Age = (int)data["Age"],
                Phone = (string)data["Phone"],
                Password = (string)data["Password"],
                FirstName = (string)data["FirstName"],
                LastName = (string)data["LastName"],
                IsActive = (bool)data["IsActive"],
                ImageID = (int)data["ImageID"],
                Email = (string)data["Email"],
                BirthDay = (DateTime)data["BirthDay"],
                DateCreated = (DateTime)data["DateCreated"],
                DateUpdated = (DateTime)data["DateUpdated"],
                BlockDescription = (string)data["BlockDescription"],
            };
            DataModel.Image img = new DataModel.Image
            {
                IId = (int)data["IId"],
                ImageName = (string)data["ImageName"],
                ImageContent = (byte[])data["ImageContent"]
            };
            user.Image = img;
            return user;
        }
    }
}
