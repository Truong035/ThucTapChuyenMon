using System.Web;
using System.Web.Mvc;

namespace PhamTrongTruong_5951071113
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
