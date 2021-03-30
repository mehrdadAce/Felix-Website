using FelixWebsite.Bll.Helpers;
using FelixWebsite.Core.Models.SocialMedia.FbModels;
using FelixWebsite.Shared.enums;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using Umbraco.Core.Logging;

namespace FelixWebsite.Core.Controllers.UmbracoAuthorizedControllers.Authentication
{
    public class FacebookAuthController : AuthenticationBaseController
    {
        private static FacebookReviewArray allReviews;
        private static FacebookAccessToken accessTokenObject;
        private static FacebookPageAccessTokens pageAccessTokensObject;

        public ActionResult GoToInBetween(string storeId, string pageId, string type)
        {
            currentPageId = pageId;
            currentStoreId = storeId;
            switch (type)
            {
                case "reviews":
                    apiType = SectionType.Reviews;
                    break;
                case "news":
                    apiType = SectionType.News;
                    break;
            }
            return View("~/Views/Backend/FacebookReviews/InBetween.cshtml");
        }

        public ActionResult StartAuth()
        {
            var authVersion = ConfigHelper.FacebookVersion;
            var clientId = ConfigHelper.FacebookClientId;
            var scope = ConfigHelper.FacebookScope;
            var domain = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host;
            var redirectUri = domain + ConfigHelper.FacebookReviewsRedirectUri;
            var fbAuthUrl = ConfigHelper.FacebookAuthUrl;
            guid = Guid.NewGuid();
            var uri =
                $"{fbAuthUrl}{authVersion}" +
                $"/dialog/oauth?client_id={clientId}&" +
                $"redirect_uri={redirectUri}&" +
                $"state={guid}&{scope}";
            return new RedirectResult(uri);
        }

        protected override async Task<string> ContinueAuthChild(string code, string redirectUri)
        {
            try
            {
                var authVersion = ConfigHelper.FacebookVersion;
                var clientId = ConfigHelper.FacebookClientId;
                var clientSecret = ConfigHelper.FacebookSecret;
                var fbGraphUrl = ConfigHelper.FacebookGraphUrl;
                using (var client = new HttpClient())
                {
                    // Get normal access token
                    var requestUri = $"{fbGraphUrl}{authVersion}/oauth/access_token?" +
                                     $"client_id={clientId}&" +
                                     $"redirect_uri={redirectUri}&" +
                                     $"client_secret={clientSecret}&" +
                                     $"code={code}";
                    var result = await client.GetAsync(requestUri);
                    accessTokenObject = await result.Content.ReadAsAsync<FacebookAccessToken>();

                    // Get all page access tokens w/ normal access token (required for reviews)
                    var requestUriPage = $"{fbGraphUrl}{authVersion}/me/accounts";
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenObject.AccessToken);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var resultCall2 = await client.GetAsync(requestUriPage);
                    var responseString = await resultCall2.Content.ReadAsStringAsync();
                    if (resultCall2.IsSuccessStatusCode)
                    {
                        System.IO.File.WriteAllText(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data/FacebookAccountsResponse.txt"), responseString);
                    }
                    else
                    {
                        LogHelper.Error(GetType(), $"Error getting page access tokens, got response: {responseString}", new Exception("Unexpected response"));
                    }
                    pageAccessTokensObject = await resultCall2.Content.ReadAsAsync<FacebookPageAccessTokens>();
                }

                var selectedRoute = SelectRoute();
                var requiredToken = pageAccessTokensObject.PageTokensList.Where(x => x.Id == currentPageId).FirstOrDefault();
                var route = "/umbraco/#/" + selectedRoute + "?storeId=" + currentStoreId + "&pageId=" + currentPageId + "&accessToken=" + requiredToken.AccessToken;
                return route;
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something went wrong in the second OAuth step. pageAccesTokensObject is: {JsonConvert.SerializeObject(pageAccessTokensObject)} ", e);
                return "/umbraco/#/reviewsAlias";
            }
        }

        protected string SelectRoute()
        {
            switch (apiType)
            {
                case SectionType.Reviews:
                    return App_Plugins.Reviews.Constants.REVIEW_SECTION_ALIAS + "/" +
                                App_Plugins.Reviews.Constants.REVIEW_TREE_ALIAS + "/FacebookReviews";
                case SectionType.News:
                    return App_Plugins.News.Constants.SECTION_ALIAS + "/" + App_Plugins.News.Constants.TREE_ALIAS + "/FacebookNews";
                default:
                    return "";
            }
        }

    }
}
