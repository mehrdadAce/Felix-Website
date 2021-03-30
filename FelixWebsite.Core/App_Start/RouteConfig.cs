using System.Web.Mvc;
using System.Web.Routing;
using umbraco;

namespace FelixWebsite.Core
{
    /// <summary>
    /// Summary description for RouteConfig
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Error",
                url: "Error/500",
                defaults: new { controller = "Error", action = "ErrorThrown500", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: GlobalSettings.UmbracoMvcArea + "/backoffice/{controller}/{action}/{id}",
                namespaces: new[] { "FelixWebsite.Core.Controllers.UmbracoAuthorizedControllers" },
                defaults: new { controller = "AcquisitionTool", action = "GetMe", id = UrlParameter.Optional}
            );
        }   
    }
}