using PhamTrongTruong_5951071113.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TracNghiemOnline.Controllers
{
    public class BaseController : Controller
    {
        //GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (TaiKhoan)Session["user"];
            if (session == null)
             {
                   filterContext.Result = new RedirectToRouteResult(
                  new System.Web.Routing.RouteValueDictionary( new { controller = "Account", action = "Login" } ));
           }


              Session["user"] = session;


            base.OnActionExecuting(filterContext);
        }

    }
}