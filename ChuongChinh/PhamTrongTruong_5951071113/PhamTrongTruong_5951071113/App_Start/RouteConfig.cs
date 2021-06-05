using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhamTrongTruong_5951071113
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
 name: "BaiHoc",
 url: "BaiHoc/{id}",
 defaults: new { controller = "Home", action = "BaiHoc", id = UrlParameter.Optional }
);
           routes.MapRoute(
           name: "HocBai",
           url: "HocBai/{id}",
           defaults: new { controller = "Home", action = "HocBai", id = UrlParameter.Optional }
       );


            routes.MapRoute(
  name: "SeachDethi",
  url: "SeachDethi/{id}",
  defaults: new { controller = "Home", action = "SeachDethi", id = UrlParameter.Optional }
);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "GioiThieu", id = UrlParameter.Optional }
            );
        }
    }
}
