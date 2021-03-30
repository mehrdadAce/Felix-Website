using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FelixWebsite.Core.Models.SocialMedia;
using FelixWebsite.Shared.enums;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Controllers.UmbracoAuthorizedControllers.Reviews
{
    public abstract class ReviewController: UmbracoAuthorizedController
    {
        protected SocialMediaType SocialMedia { get; set; }
        protected ReviewController(SocialMediaType media)
        {
        }

        protected int MakeNewReviewEntry(BaseReview review, int storeId)
        {
            var business = Services.ContentService.GetById(storeId);
            var allContentTypes = Services.ContentTypeService.GetAllContentTypes();
            var socialMediaReviewsId = allContentTypes.FirstOrDefault(cType => string.Equals(cType.Alias, nameof(SocialMediaReviews), StringComparison.CurrentCultureIgnoreCase))?.Id ?? 0;
            var socialMediaReviewsType = Services.ContentTypeService.GetContentType(socialMediaReviewsId);

            var reviewsNode = business.Children().Where(x => x.ContentType.Id == socialMediaReviewsType.Id).FirstOrDefault();
            
            // If reviews node doesn't exist -> create it.
            if (reviewsNode == null) {
                var contentPage = new Content("Reviews", business, socialMediaReviewsType);
                Services.ContentService.Save(contentPage);
            }

            var socialMediaReviews = reviewsNode;
            var reviews = socialMediaReviews.Children();

            var reviewId = allContentTypes.FirstOrDefault(cType => string.Equals(cType.Alias, nameof(Review), StringComparison.CurrentCultureIgnoreCase))?.Id ?? 0;
            var reviewType = Services.ContentTypeService.GetContentType(reviewId);

            var existingReviews = socialMediaReviews.Children().Where(x => x.ContentTypeId == reviewId);

            reviewType.ParentId = socialMediaReviews.Id;

            return SetContentPage(reviewType, socialMediaReviews, review, existingReviews);
        }

        protected abstract int SetContentPage(IContentType contentType, IContent parent, BaseReview review, IEnumerable<IContent> existingReviews);
        public abstract JsonResult GetReviews(string pageId, string accessToken);
    }
}
