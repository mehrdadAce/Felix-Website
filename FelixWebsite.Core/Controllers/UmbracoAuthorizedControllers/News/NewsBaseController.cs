using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FelixWebsite.Core.Models.SocialMedia;
using FelixWebsite.Shared.enums;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Controllers.UmbracoAuthorizedControllers.News
{
    public abstract class NewsBaseController: UmbracoAuthorizedController
    {
        protected SocialMediaType SocialMedia { get; set; }

        protected NewsBaseController(SocialMediaType media)
        {
        }

        protected int MakeNewArticle(BaseArticle article, int storeId)
        {
            var business = Services.ContentService.GetById(storeId);
            var allContentTypes = Services.ContentTypeService.GetAllContentTypes();
            var socialMediaNewsId = allContentTypes.FirstOrDefault(cType => string.Equals(cType.Alias, nameof(SocialMediaNews), StringComparison.CurrentCultureIgnoreCase))?.Id ?? 0;
            var socialMediaNewsType = Services.ContentTypeService.GetContentType(socialMediaNewsId);

            var newsNode = business.Children().Where(x => x.ContentType.Id == socialMediaNewsType.Id).FirstOrDefault();

            // If news node doesn't exist -> create it.
            if (business.Children().Where(x => x.ContentType.Id == socialMediaNewsType.Id).FirstOrDefault() == null)
            {
                var contentPage = new Content("Nieuws", business, socialMediaNewsType);
                Services.ContentService.Save(contentPage);
            }

            var socialMediaNews = business.Children().Where(x => x.ContentType.Id == socialMediaNewsType.Id).FirstOrDefault();
            var articles = socialMediaNews.Children();

            var articleId = allContentTypes.FirstOrDefault(cType => string.Equals(cType.Alias, nameof(SocialMediaArticle), StringComparison.CurrentCultureIgnoreCase))?.Id ?? 0;
            var articleType = Services.ContentTypeService.GetContentType(articleId);

            var existingArticles = socialMediaNews.Children().Where(x => x.ContentTypeId == articleId);

            articleType.ParentId = socialMediaNews.Id;

            return SetContentPage(articleType, socialMediaNews, article, existingArticles);
        }

        protected abstract int SetContentPage(IContentType contentType, IContent parent, BaseArticle article, IEnumerable<IContent> store);
        public abstract JsonResult GetArticles(string pageId, string accessToken);
    }
}
