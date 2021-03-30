using System.Web.Mvc;
using FelixWebsite.Core.ActionFilters;

namespace FelixWebsite.Core.App_Start
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new RootActionFilter());
        }
    }
}
