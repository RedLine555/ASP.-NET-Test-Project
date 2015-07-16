using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using HomeWork3Data.DataMappers;
using HomeWork3Data.Classes;
using HomeWork3Data.DataModel;
using HomeWork3Common.Helpers.Converter;
using HomeWork3Data.Interfases;

namespace HomeWork3Data
{
    public class ADOUserRepository : ADOGenericRepository<User>, IUserRepository
    {
        private IImageRepository imageRep;
        private int? maxId = null;

        public ADOUserRepository(string connString)
            : base(connString)
        {
            imageRep = new ADOImageRepository(connectionString);
        }
        public override IQueryable<User> GetAll()
        {
            IQueryable<User> users = base.GetAll();

            IQueryable<Image> images = imageRep.GetAll();

            foreach (User user in users)
            {
                Image correspImage = images.Where(img => img.IId == user.ImageID).FirstOrDefault();
                user.Image = correspImage;
            }

            if (maxId == null)
                maxId = users.Max(usr => usr.UId);

            return users;
        }

        public override void Add(User instance)
        {
            if (maxId == null)
                GetAll();

            instance.UId = (int)maxId + 1;
            maxId++;
            base.Add(instance);
        }

        public User FindByID(int id)
        {
            User res = null;
            try
            {
                conn.Open();
                SqlCommand query = new SqlCommand(String.Format("SELECT * FROM [User] WHERE UId = {0}", id), conn);

                using (SqlDataReader reader = query.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        res = StaticEntitiesConverter.ReaderRowToEntity<User>(reader);
                    }
                }
            }
            finally
            {
                conn.Close();
            }

            if (res.ImageID != null)
                res.Image = imageRep.FindByID((int)res.ImageID);

            return res;
        }

        public bool isActive(int id)
        {
            return FindByID(id).IsActive;
        }

    }
}
