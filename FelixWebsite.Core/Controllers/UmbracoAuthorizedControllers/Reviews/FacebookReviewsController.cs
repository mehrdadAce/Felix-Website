using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using FelixWebsite.Core.Helpers;
using FelixWebsite.Core.Models.SocialMedia;
using FelixWebsite.Core.Models.SocialMedia.FbModels;
using FelixWebsite.Shared.enums;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Controllers.UmbracoAuthorizedControllers.Reviews
{
    public class FacebookReviewsController: ReviewController
    {
        public FacebookReviewsController() : base(SocialMediaType.Facebook)
        {
        }

        [HttpGet]
        public override JsonResult GetReviews(string pageId, string accessToken)
        {
            try
            {
                var reviews = new List<FacebookReview>();
                using (var client = new HttpClient())
                {
                    var requestUri = "https://graph.facebook.com/v3.2/"+pageId+"/ratings?fields=created_time,recommendation_type,review_text,reviewer{id,name,picture},has_rating,rating,has_review";
                                                                                                         
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage result = System.Threading.Tasks.Task.Run(() => client.GetAsync(requestUri)).GetAwaiter().GetResult();
                    if (!result.IsSuccessStatusCode)
                    {
                        LogHelper.Error(GetType(), "Error making call to Facebook, status: " + result.StatusCode + " " + result.ReasonPhrase, new HttpRequestException());
                        return Json(string.Empty, JsonRequestBehavior.AllowGet);
                    }
                    reviews = System.Threading.Tasks.Task.Run(() => result.Content.ReadAsAsync<FacebookReviewArray>()).GetAwaiter().GetResult().Data.ToList();
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
        public string SubmitSelectedValues(IEnumerable<FacebookReview> formData, int storeId)
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

        protected override int SetContentPage(IContentType contentType, IContent parent, BaseReview review, IEnumerable<IContent> existingReviews)
        {
            var fbReview = (FacebookReview)review;

            if (existingReviews.Any())
            {
                var exisitingReview = existingReviews.ToList().Where(x => x.GetValue("userId").ToString() == fbReview.FbUser.Id).FirstOrDefault();
                if (exisitingReview != null)
                    return exisitingReview.Id;
            }

            var contentPage = new Content("Review van " + fbReview.FbUser.UserName, parent, contentType);
            contentPage.SetTypedValue<Review, string>(r => r.Text, fbReview.Text);
            contentPage.SetTypedValue<Review, string>(r => r.Username, fbReview.FbUser.UserName);
            contentPage.SetTypedValue<Review, string>(r => r.UserId, fbReview.FbUser.Id);
            contentPage.SetTypedValue<Review, string>(r => r.UserPictureUrl, fbReview.FbUser.Picture.FacebookPicture.Url);
            contentPage.SetTypedValue<Review, string>(r => r.Rating, fbReview.StarRating);

            Services.ContentService.Save(contentPage);
            return contentPage.Id;
        }
    }
}
