using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using FelixWebsite.Core.Exceptions;
using FelixWebsite.Shared.enums;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace FelixWebsite.Core.Controllers.UmbracoAuthorizedControllers.Authentication
{
    public abstract class AuthenticationBaseController: UmbracoAuthorizedController
    {
        protected static string currentPageId;
        protected static string currentStoreId;
        protected static SectionType apiType;
        protected static Guid guid;

        public async Task<ActionResult> ContinueAuth(string code, Guid state)
        {
            try
            {
                if (state != guid)
                {
                    throw new ApiStateException();
                }
                var appUrl = Request.Url;
                if (appUrl == null) return null;
                var redirectUri = appUrl.AbsoluteUri.Split('?')[0];
                var domainUri = appUrl.AbsoluteUri.Split(new[] { "/umbraco" }, StringSplitOptions.None)[0];
                var route = await ContinueAuthChild(code, redirectUri);
                
                return
                    new RedirectResult(domainUri + route);
            }
            catch (ApiStateException e)
            {
                LogHelper.Error(GetType(), "The saved guid was not equal to the received guid. For security, this request will be dropped: ", e);
                var appUrl = Request.Url;
                if (appUrl == null) return null;
                var domainUri = appUrl.AbsoluteUri.Split(new[] { "/Umbraco" }, 1, StringSplitOptions.None);
                return new RedirectResult(domainUri + "/umbraco/#/reviewsAlias");
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), "Something went wrong in the second OAuth step. The thrown error is: ", e);
                return new RedirectResult("/umbraco/#/reviewsAlias");
            }
        }

        protected abstract Task<string> ContinueAuthChild(string code, string redirectUri);
    }
}
