using System.Web.Http;
using Umbraco.Core.Models;
using Umbraco.Web.WebApi;

namespace FelixWebsite.Core.Controllers.UmbracoAuthorizedApiControllers
{
    public class MediaProblemHelperController : UmbracoApiController
    {
        //http://felix.localtest.me/Umbraco/Api/MediaProblemHelper/ResaveAllImages
        [HttpGet]
        public string ResaveAllImages()
        {
            var mediaService = Services.MediaService;
            var rootmedia = mediaService.GetRootMedia();
            foreach (var media in rootmedia)
            {
                mediaService.Save(media);

                foreach (var item in media.Descendants())
                {
                    mediaService.Save(item);
                }
            }
            return "All media items saved";
        }
    }
}
