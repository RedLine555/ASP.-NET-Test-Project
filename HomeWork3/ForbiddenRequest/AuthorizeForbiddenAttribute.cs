using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeWork3.ForbiddenRequest
{
    public class AuthorizeForbiddenAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpForbiddenResult();
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}