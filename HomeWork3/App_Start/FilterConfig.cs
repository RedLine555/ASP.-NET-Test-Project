using HomeWork3.ExeptionAttribute;
using System.Web;
using System.Web.Mvc;

namespace HomeWork3
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MyHandleErrorAttribute());
        }
    }
}
