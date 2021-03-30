using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FelixWebsite.Bdo.Models.Acquisition;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace FelixWebsite.Bll.Extensions
{
    public static  class PhotoInfoExtensions
    {
        public static string GetUrl(this PhotoInfo photoInfo, IMediaService mediaService, UmbracoHelper umbraco)
        {
            var media = mediaService.GetById(photoInfo.MediaId);
            var imageUrl = umbraco.Media(media?.Id);
            return imageUrl?.Url;
        }
    }
}
