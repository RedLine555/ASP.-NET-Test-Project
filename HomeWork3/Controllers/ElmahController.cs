using HomeWork3.ForbiddenRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeWork3.Controllers
{
    public class ElmahController : Controller
    {
        //
        // GET: /Elmah/
        [AuthorizeForbiddenAttribute(Roles = "Admin")]
        public ActionResult Index(string type)
        {
            return new ElmahResult(type);
        }
	}
}