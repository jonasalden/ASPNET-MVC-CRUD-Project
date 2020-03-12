using System.Web;
using System.Web.Mvc;

namespace Projektarbete_MVC1_JonasAlden
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
