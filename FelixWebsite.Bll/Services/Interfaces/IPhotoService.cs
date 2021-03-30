using System.Collections.Generic;
using System.Web;
using FelixWebsite.Bdo.Models.Acquisition;
using FelixWebsite.Bll.Services.Interfaces.Base;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace FelixWebsite.Bll.Services.Interfaces
{
    public interface IPhotoService:IBaseService<PhotoInfo>
    {
        IEnumerable<PhotoInfo> GetAllPhotoIdsWithUserInfoId(int id);

        IEnumerable<PhotoInfo> GetAllPhotoIdsOfSchemeWithUserId(int id);

        PhotoInfo GetByImageType(string imageType, int userId, bool isOverview = false, int index = 0);
        OverviewModel GetOverviewModelData(int id, IMediaService mediaService, UmbracoHelper Umbraco);

        OverviewDamageModel GetOverviewModelDataDamages(int id, IMediaService mediaService, UmbracoHelper Umbraco, ILogger logger, HttpServerUtilityBase server);

        IEnumerable<PhotoInfo> GetAllByImageType(string imageType, int userId);
        PhotoInfo GetByMediaId(int mediaId, int userId);
    }
}
