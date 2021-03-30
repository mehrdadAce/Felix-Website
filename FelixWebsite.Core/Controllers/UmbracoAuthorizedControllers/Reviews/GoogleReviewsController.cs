using FelixWebsite.Core.Helpers;
using FelixWebsite.Core.Models.SocialMedia;
using FelixWebsite.Core.Models.SocialMedia.GoogleModels;
using FelixWebsite.Shared.enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Controllers.UmbracoAuthorizedControllers.Reviews
{
    public class GoogleReviewsController : ReviewController
    {
        public GoogleReviewsController() : base(SocialMediaType.Google)
        {
        }


        [HttpGet]
        public override JsonResult GetReviews(string pageId, string accessToken)
        {
            //https://mybusiness.googleapis.com/v4/accounts
            //https://mybusiness.googleapis.com/v4/accounts/108332478757398791791/locations/4055916097034165656/reviews
            try
            {
                var reviews = new List<GoogleReview>();
                using (var client = new HttpClient())
                {
                    var requestUri = "https://mybusiness.googleapis.com/v4/accounts";
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage result = System.Threading.Tasks.Task.Run(() => client.GetAsync(requestUri)).GetAwaiter().GetResult();
                    if (!result.IsSuccessStatusCode)
                    {
                        LogHelper.Error(GetType(), "Error making call to Google, status: " + result.StatusCode + " " + result.ReasonPhrase, new HttpRequestException());
                        return Json(string.Empty, JsonRequestBehavior.AllowGet);
                    }
                    var accountResponse = System.Threading.Tasks.Task.Run(() => result.Content.ReadAsAsync<GoogleAccountResponse>()).GetAwaiter().GetResult();
                    if (accountResponse == null || !accountResponse.Accounts.Any())
                    {
                        LogHelper.Error(GetType(), "No google accounts found!", new Exception("No accounts"));
                        return Json(string.Empty, JsonRequestBehavior.AllowGet);
                    }

                    //To check which locations are linked to this account (Can be commented out)
                    requestUri = $"https://mybusiness.googleapis.com/v4/accounts/{accountResponse.Accounts.FirstOrDefault().AccountId}/locations";
                    result = System.Threading.Tasks.Task.Run(() => client.GetAsync(requestUri)).GetAwaiter().GetResult();
                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        System.IO.File.WriteAllText(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data/GoogleLocationsResponse.txt"), responseString);
                    }

                    requestUri = $"https://mybusiness.googleapis.com/v4/accounts/{accountResponse.Accounts.FirstOrDefault().AccountId}/locations/{pageId}/reviews";
                    result = System.Threading.Tasks.Task.Run(() => client.GetAsync(requestUri)).GetAwaiter().GetResult();
                    if (!result.IsSuccessStatusCode)
                    {
                        var jsonResponse = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        LogHelper.Error(GetType(), "Error making call to Google, status: " + result.StatusCode + " " + result.ReasonPhrase + " Full response = " + jsonResponse, new HttpRequestException());
                        return Json(string.Empty, JsonRequestBehavior.AllowGet);
                    }
                    reviews = System.Threading.Tasks.Task.Run(() => result.Content.ReadAsAsync<GoogleReviewResponse>()).GetAwaiter().GetResult().Reviews.ToList();
                }
                return Json(reviews, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), "Getting the facebook reviews, the thrown error is: ", e);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public string SubmitSelectedValues(IEnumerable<GoogleReview> formData, int storeId)
        {
            try
            {
                var count = 0;
                var selectedReviews = formData.Where(review => review.Selected);
                var firstReviewPath = "/";
                foreach (var review in selectedReviews)
                {
                    var reviewId = MakeNewReviewEntry(review, storeId);
                    if (count != 0) continue;
                    firstReviewPath = "/umbraco/#/content/content/edit/" + reviewId;
                    count++;
                }

                return firstReviewPath;
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), "Something went wrong when submitting values to the document nodes: ", e);
                return "/";
            }
        }

        protected override int SetContentPage(IContentType contentType, IContent parent, BaseReview review, IEnumerable<IContent> reviews)
        {
            var googleReview = (GoogleReview)review;
            var contentPage = new Content("Review van " + googleReview.Reviewer.DisplayName, parent, contentType);
            //contentPage.SetTypedValue<Review, DateTime>(r => r.CreatedDateTime, googleReview.CreatedDateTime);
            contentPage.SetTypedValue<Review, string>(r => r.Text, googleReview.Comment);
            contentPage.SetTypedValue<Review, string>(r => r.Username, googleReview.Reviewer.DisplayName);
            contentPage.SetTypedValue<Review, string>(r => r.UserId, googleReview.ReviewId);
            contentPage.SetTypedValue<Review, string>(r => r.UserPictureUrl, googleReview.Reviewer.ProfilePhotoUrl);
            contentPage.SetTypedValue<Review, string>(r => r.Rating, (googleReview.Rating == GoogleStarRating.StarRatingUnspecified ? 5 : (int)googleReview.Rating).ToString());

            Services.ContentService.Save(contentPage);
            return contentPage.Id;
        }
    }
}
