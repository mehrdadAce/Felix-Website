using FelixWebsite.Core.Helpers;
using System.Linq;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Controllers.SurfaceController
{
    public class BaseCarrosserieLinkedSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {
        protected string GetCarrosseriePlatformLinkFromBreadcrumbRoot()
        {
            var root = Umbraco.TypedContentAtRoot().FirstOrDefault();
            var itemLinkedToCarrosseriePlatform = Session.GetBreadcrumbRootOfTypeOrFirstInBusinesses<ILinkedToCarrosseriePlatform>(root);
            var carrosseriePlatFormLink = itemLinkedToCarrosseriePlatform.CarrosseriePlatformLink;
            return carrosseriePlatFormLink;
        }
    }
}