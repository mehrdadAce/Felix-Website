using FelixWebsite.Bdo.Models.Acquisition;
using FelixWebsite.Bll.Services;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Services;
using Umbraco.Core.Models;
using Umbraco.Core.Logging;
using FelixWebsite.Bll.Services.Interfaces;
using System.IO;

namespace FelixWebsite.Bll.Helpers
{
    public static class PhotoHelper
    {
        public static List<SchemePictureModel> GetPhotoInfosForScheme(int userInformationId, IPhotoService photoService, IMediaService mediaService, ILogger logger)
        {
            var photoInfos = photoService.GetAllPhotoIdsOfSchemeWithUserId(userInformationId);
            var res = new List<SchemePictureModel>();
            var orderedPhotoInfos = photoInfos.OrderBy(x => x.PhotoIdentifier);
            var amountOfIdentifiers = orderedPhotoInfos.Select(x => x.PhotoIdentifier).Distinct().ToList();
            for (var i = 0; i < amountOfIdentifiers.Count; i++)
            {
                var ids = orderedPhotoInfos.Where(x => x.PhotoIdentifier == amountOfIdentifiers[i]).Select(x => x.MediaId);
                var list = GetMediaValuesList(ids, mediaService, logger);
                var imageLocation = PhotoService.GetImageLocationForIdentifier(amountOfIdentifiers[i]);
                var imageType = SchemeDamageService.GetReadableName(SchemeDamageService.GetSchemeEntryForPhotoIdentifier(amountOfIdentifiers[i]));
                var pictureModel = new SchemePictureModel(imageLocation, imageType, amountOfIdentifiers[i]) { SchemePictures = list };
                res.Add(pictureModel);
            }
            return res;
        }

        public static IEnumerable<MediaValuesModel> GetMediaValuesList(IEnumerable<int> mediaImageIds, IMediaService mediaService
            , ILogger logger)
        {
            var mediaValues = new List<MediaValuesModel>();
            foreach (var imageId in mediaImageIds)
            {
                var media = mediaService.GetById(imageId);
                var mediaName = media.Name;
                var mediaNameWithoutSpecialCharacters = mediaName.Replace(" ", "").Replace("(", "").Replace(")", "");
                var mediaUrl = media.GetUrl("umbracoFile", logger);
                mediaValues.Add(new MediaValuesModel(media.Id, mediaNameWithoutSpecialCharacters, mediaUrl));
            }
            return mediaValues;
        }

        public static byte[] ReadMediaToByteArray(string mediaUrl, IMediaService mediaService)
        {
            if (!string.IsNullOrWhiteSpace(mediaUrl))
            {
                byte[] res = null;
                using (var fileStream = mediaService.GetMediaFileContentStream(mediaUrl))
                {
                    if (fileStream.CanSeek) fileStream.Seek(0, 0);
                    byte[] buffer = new byte[16 * 1024];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        res = ms.ToArray();
                    }
                }
                return res;
            }
            return null;
        }
    }
}