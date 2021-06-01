using PhamTrongTruong_5951071113.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhamTrongTruong_5951071113.Areas.Admin
{
    public class BaseController : Controller
    {
        //GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (TaiKhoan)Session["Admin"];
            if (session == null)
             {
                   filterContext.Result = new RedirectToRouteResult(
                  new System.Web.Routing.RouteValueDictionary( new { controller = "Login", action = "Index" } ));
           }


              Session["Admin"] = session;


            base.OnActionExecuting(filterContext);
        }

    }
}