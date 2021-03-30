using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FelixWebsite.Bll.Helpers.Configuration;
using FelixWebsite.Core.App_Start;
using FelixWebsite.Core.Helpers.EventHandlers;
using Umbraco.Core;

namespace FelixWebsite.Core
{
    public class UmbracoStartup : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
            Umbraco.Core.Services.ContentService.Saving += JobOfferContentEventHandlers.SavingHandler;
            Umbraco.Core.Services.ContentService.Deleting += JobOfferContentEventHandlers.DeletingHandler;
            Umbraco.Core.Services.ContentService.Trashing += JobOfferContentEventHandlers.TrashingHandler;
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
           
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AutomapperConfig.ConfigureMapper();
            //NinjectWebCommon.Start();
        }
    }
}
