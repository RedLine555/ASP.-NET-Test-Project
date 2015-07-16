using HomeWork3.CustomRoute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HomeWork3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "User/Index",
                url: "all-user-profiles/",
                defaults: new { controller = "User", action = "Index" }
            );

            routes.MapRoute(
                name: "User/Edit",
                url: "user-profile/edit-{id}",
                defaults: new { controller = "User", action = "Edit"},
                constraints: new { id = new IdRoute() }
            );

            routes.MapRoute(
                "Admin_elmah",
                "Admin/elmah/{type}",
                new { action = "Index", controller = "Elmah", type = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
            );
        }
    }

    class ElmahResult : ActionResult
    {
        private string _resouceType;

        public ElmahResult(string resouceType)
        {
            _resouceType = resouceType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var factory = new Elmah.ErrorLogPageFactory();

            if (!string.IsNullOrEmpty(_resouceType))
            {
                var pathInfo = "." + _resouceType;
                HttpContext.Current.RewritePath(PathForStylesheet(), pathInfo, HttpContext.Current.Request.QueryString.ToString());
            }

            var httpHandler = factory.GetHandler(HttpContext.Current, null, null, null);
            httpHandler.ProcessRequest(HttpContext.Current);
        }

        private string PathForStylesheet()
        {
            return _resouceType != "stylesheet" ? HttpContext.Current.Request.Path.Replace(String.Format("/{0}", _resouceType), string.Empty) : HttpContext.Current.Request.Path;
        }
    }
}
