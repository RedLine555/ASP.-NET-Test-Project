using HomeWork3Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork3Data.Interfases
{
    interface IImageRepository : IGenericRepository<Image>
        {
            Image FindByID(int id);

            Image ImageContentToImage(byte[] content, string name);
        }
}
