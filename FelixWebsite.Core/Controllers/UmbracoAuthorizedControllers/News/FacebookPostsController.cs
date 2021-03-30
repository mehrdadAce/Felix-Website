using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using FelixWebsite.Core.Helpers;
using FelixWebsite.Core.Models.SocialMedia;
using FelixWebsite.Core.Models.SocialMedia.FbModels;
using FelixWebsite.Shared.enums;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Controllers.UmbracoAuthorizedControllers.News
{
    public class FacebookPostsController : NewsBaseController
    {

        public FacebookPostsController() : base(SocialMediaType.Facebook)
        {
        }

        public override JsonResult GetArticles(string pageId, string accessToken)
        {
            try
            {
                var newsPosts = new List<FacebookPost>();
                using (var client = new HttpClient())
                {
                    var requestUri = $"https://graph.facebook.com/v3.2/{pageId}/feed?fields=full_picture,link,id,message,created_time,from";
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage result = System.Threading.Tasks.Task.Run(() => client.GetAsync(requestUri)).GetAwaiter().GetResult();
                    if(!result.IsSuccessStatusCode)
                    {
                        LogHelper.Error(GetType(), "Error making call to Facebook, status: " + result.StatusCode + " " + result.ReasonPhrase, new HttpRequestException());
                        return Json(string.Empty, JsonRequestBehavior.AllowGet);
                    }
                    var x = System.Threading.Tasks.Task.Run(() => result.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                    newsPosts = System.Threading.Tasks.Task.Run(() => result.Content.ReadAsAsync<FacebookResponse>()).GetAwaiter().GetResult().Data.ToList();
                }
                return Json(newsPosts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), "Getting the facebook reviews, the thrown error is: ", e);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public string SubmitSelectedValues(IEnumerable<FacebookPost> formData, int storeId)
        {
            try
            {
                var count = 0;
                var selectedArticles = formData.Where(article => article.Selected);
                var path = "/";
                foreach (var article in selectedArticles)
                {
                    var reviewId = MakeNewArticle(article, storeId);
                    if (count != 0) continue;
                    path = "/umbraco/#/content/content/edit/" + reviewId;
                    count++;
                }

                return path;
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), "Something went wrong while submitting the values to the contentpages", e);
                return "/";
            }
        }

        protected override int SetContentPage(IContentType contentType, IContent parent, BaseArticle article, IEnumerable<IContent> existingNews)
        {
            var fbReview = (FacebookPost)article;

            if (existingNews != null && existingNews.Any())
            {
                var existingArticle = existingNews.ToList().Where(x => x.GetValue("postId") != null && x.GetValue("postId").ToString() == fbReview.Id).FirstOrDefault();
                if (existingArticle != null)
                    return existingArticle.Id;
            }

            var title = fbReview.StatusMessage.Length > 40 ? fbReview.StatusMessage.Substring(0, 40) + "..." : fbReview.StatusMessage;

            var contentPage = new Content(title, parent, contentType);
            contentPage.SetTypedValue<SocialMediaArticle, string>(r => r.PostId, fbReview.Id);
            contentPage.SetTypedValue<SocialMediaArticle, string>(r => r.Text, fbReview.StatusMessage);
            contentPage.SetTypedValue<SocialMediaArticle, string>(r => r.Author, fbReview.User.Name);
            contentPage.SetTypedValue<SocialMediaArticle, string>(r => r.ImgUrl, fbReview.PhotoUrl);

            Services.ContentService.Save(contentPage);
            return contentPage.Id;
        }
    }
}
