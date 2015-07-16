using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace HomeWork3.CustomRoute
{
    public class IdRoute : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName,
                           RouteValueDictionary values, RouteDirection routeDirection)
        {
            int val;
            if (values[parameterName].ToString().Length > 6)
                return false;
            if (int.TryParse(values[parameterName].ToString(), out val))
                return val >= 0;
            else
                return false;
        }
    }
}