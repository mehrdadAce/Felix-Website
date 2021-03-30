using FelixWebsite.Bll.Helpers;
using FelixWebsite.Bll.Services.Interfaces;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace FelixWebsite.Core.Controllers.UmbracoAuthorizedControllers
{
    public class DamageToolController: UmbracoAuthorizedController
    {
        private IAcquisitionInformationService acquisitionInformationService;
        private IPhotoService photoService;
        private IMediaService mediaService;
        private ISchemeDamageService schemeDamageService;

        public DamageToolController(IAcquisitionInformationService acquisitionInformationService, IPhotoService photoService, ISchemeDamageService schemeDamageService)
        {
            this.acquisitionInformationService = acquisitionInformationService;
            this.photoService = photoService;
            mediaService = Services.MediaService;
            this.schemeDamageService = schemeDamageService;
        }

        [HttpGet]
        public ActionResult GetOverviewData(int id)
        {
            var model = photoService.GetOverviewModelDataDamages(id, mediaService, Umbraco, Logger, Server);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSchemeOverview(int userId)
        {
            var schemeOverviewModel = schemeDamageService.GetSchemeOverviewModelWithAllDamages(userId);
            return PartialView("~/Views/Partials/Acquisition/SchemeOverview.cshtml", schemeOverviewModel.ToArray());
        }
    }
}
