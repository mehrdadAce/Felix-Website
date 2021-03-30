using FelixWebsite.Core.Helpers;
using System;
using System.Web.Mvc;
using umbraco;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.ActionFilters
{
    public class RootActionFilter : ActionFilterAttribute
    {
        protected static Umbraco.Web.UmbracoHelper Umbraco
        {
            get
            {
                if (UmbracoContext.Current.IsFrontEndUmbracoRequest)
                    return new Umbraco.Web.UmbracoHelper(UmbracoContext.Current);
                return null;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentPage = Umbraco?.AssignedContentItem;
            if (currentPage != null)
            {
                if (currentPage.IsHomePage())
                {
                    filterContext.HttpContext.Session.SetBreadcrumbRoot(currentPage);
                }
                else
                {
                    if (filterContext.HttpContext.Request.UrlReferrer != null)
                    {
                        var node = uQuery.GetNodeByUrl(filterContext.HttpContext.Request.UrlReferrer.AbsoluteUri);
                        var referrer = Umbraco.TypedContent(node.Id);
                        if (referrer == null)
                            return;
                        else if (referrer.IsHomePage())
                        {
                            filterContext.HttpContext.Session.SetBreadcrumbRoot(referrer);
                        }
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
