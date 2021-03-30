using FelixWebsite.Bll.Helpers;
using FelixWebsite.Core.Models.SocialMedia.GoogleModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Core.Logging;

namespace FelixWebsite.Core.Controllers.UmbracoAuthorizedControllers.Authentication
{
    public class GoogleAuthController : AuthenticationBaseController
    {
        private static GoogleAccessToken accessTokenObject;
        public ActionResult GoToInBetween(string pageId, string storeId)
        {
            currentPageId = pageId;
            currentStoreId = storeId;
            return View("~/Views/Backend/GoogleReviews/InBetween.cshtml");
        }

        public ActionResult StartAuth()
        {
            var clientId = ConfigHelper.GoogleClientId;
            var domain = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host;
            var redirectUri = domain + ConfigHelper.GoogleRedirectUri;
            var scope = ConfigHelper.GoogleScope;
            var responseType = "response_type=code";
            var authUrl = ConfigHelper.GoogleAuthUrl;
            guid = Guid.NewGuid();
            var uri = $"{authUrl}?{scope}&{responseType}&" +
                      $"client_id={clientId}&" +
                      $"redirect_uri={redirectUri}&" +
                      $"state={guid}";
            return new RedirectResult(uri);
        }

        protected override async Task<string> ContinueAuthChild(string code, string redirectUri)
        {
            try
            {

                var clientId = ConfigHelper.GoogleClientId;
                var clientSecret = ConfigHelper.GoogleSecret;
                var graphUrl = ConfigHelper.GoogleGraphUrl;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var requestUri = graphUrl;
                    var content = new Dictionary<string, string>
                    {
                        {"client_id", clientId},
                        {"client_secret", clientSecret},
                        {"redirect_uri", redirectUri},
                        {"code", code},
                        {"grant_type", "authorization_code"}
                    };
                    var stringContent = new FormUrlEncodedContent(content);
                    var result = await client.PostAsync(requestUri, stringContent);
                    var str = await result.Content.ReadAsStringAsync();
                    accessTokenObject = await result.Content.ReadAsAsync<GoogleAccessToken>();
                }

                var route = "/umbraco/#/" + App_Plugins.Reviews.Constants.REVIEW_SECTION_ALIAS + "/" +
                            App_Plugins.Reviews.Constants.REVIEW_TREE_ALIAS + "/GoogleReviews?storeId=" + currentStoreId + "&pageId=" + currentPageId + "&accessToken=" + accessTokenObject.AccessToken;
                return route;
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), "Something went wrong while continueing the Google review authentication: ", e);
                return null;
            }
        }
    }
}
