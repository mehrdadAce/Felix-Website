using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Helpers
{
    public static class FelixCacheHelper
    {
        private const string BREAD_CRUMB_ROOT = "rootBreadcrumb";

        public static void SetBreadcrumbRoot(this HttpSessionStateBase session, IPublishedContent root)
        {
            session[BREAD_CRUMB_ROOT] = root;
        }

        public static IPublishedContent GetBreadcrumbRoot(this HttpSessionStateBase session, IPublishedContent currentPage)
        {
            try
            {
                var root = session[BREAD_CRUMB_ROOT] as IPublishedContent;

                if (root == null)
                    return currentPage.AncestorOrSelf(1).OfType<GroupHome>();

                return root;
            }
            catch (System.Exception)
            {
                return currentPage;
            }
        }

        public static T GetBreadcrumbRootOfTypeOrFirstInBusinesses<T>(this HttpSessionStateBase session, IPublishedContent currentPage) where T : IPublishedContent
        {
            var home = currentPage.AncestorsOrSelf().FirstOrDefault();
            var root = session.GetBreadcrumbRoot(home);
            if (root is T)
            {
                return (T)root;
            }
            else
            {
                //Fallback to the first BusinessBrand
                var businesses = home.Children.OfType<Businesses>().FirstOrDefault();
                return businesses.Children.OfType<T>().FirstOrDefault();
            }
        }
    }
}