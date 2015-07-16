using HomeWork3Common.Helpers.Converter;
using HomeWork3Data.DataModel;
using HomeWork3Data.Interfases;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork3Data.Classes
{
    class ADOImageRepository: ADOGenericRepository<Image>, IImageRepository
    {
        private int? maxId = null;

        public ADOImageRepository(string connString) : base(connString) {}

        public override IQueryable<Image> GetAll()
        {
           IQueryable<Image> res = base.GetAll();

           if (maxId == null)
               maxId = res.Max(img => img.IId);

           return res;
        }

        public override void Add(Image instance)
        {
            if (maxId == null)
                GetAll();
            instance.IId = (int)maxId + 1;
            maxId++;

            base.Add(instance);
        }

        public Image FindByID(int id)
        {
            Image res = null;
            try
            {
                conn.Open();
                SqlCommand query = new SqlCommand(String.Format("SELECT * FROM [Image] WHERE IId = {0}", id), conn);

                using (SqlDataReader reader = query.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        res = StaticEntitiesConverter.ReaderRowToEntity<Image>(reader);
                    }
                }
            }
            finally
            {
                conn.Close();
            }

            //get image

            return res;
        }

        public Image ImageContentToImage(byte[] content, string name)
        {
            if (content.Length > 0)
            {
                Image newImage = new Image();

                if (maxId == null)
                    GetAll();
                newImage.IId = (int)maxId + 1;
                maxId++;

                newImage.ImageName = name;
                newImage.ImageContent = content;
                return newImage;
            }
            else
                return null;
        }
    }
}
