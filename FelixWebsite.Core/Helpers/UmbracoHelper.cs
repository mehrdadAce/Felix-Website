using Umbraco.Core.Models;
using Umbraco.Web;

namespace FelixWebsite.Core.Helpers
{
    public static class UmbracoHelper
    {
        public static T GetTypedContentFromRoot<T>(IPublishedContent currentPage)
        {
            return GetTypedContentFromSelfOrChildren<T>(currentPage.AncestorOrSelf(1));
        }

        public static T GetTypedContentFromSelfOrChildren<T>(IPublishedContent root)
        {
            if (root is T)
            {
                return (T)root;
            }

            foreach (var child in root.Children)
            {
                var matchingItem = GetTypedContentFromSelfOrChildren<T>(child);
                if (matchingItem is T)
                    return matchingItem;
            }

            return default(T);
        }

    }
}
