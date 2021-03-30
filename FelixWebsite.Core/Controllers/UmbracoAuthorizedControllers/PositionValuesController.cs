using FelixWebsite.Bll.Services.Interfaces;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace FelixWebsite.Core.Controllers.UmbracoAuthorizedControllers
{
    public class PositionValuesController : UmbracoAuthorizedController
    {
        private IPositionClassificationService positionClassificationService;
        private IPositionScheduleService positionScheduleService;

        public PositionValuesController(IPositionClassificationService _positionClassificationService, IPositionScheduleService _positionScheduleService)
        {
            positionScheduleService = _positionScheduleService;
            positionClassificationService = _positionClassificationService;
        }

        [HttpGet]
        public ActionResult GetPositionClassifications()
        {
            var positionClassifications = positionClassificationService.GetPositionClassifications();
            return Json(positionClassifications, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPositionSchedules()
        {
            var positionSchedules = positionScheduleService.GetPositionSchedules();
            return Json(positionSchedules, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPositionScheduleType()
        {
            var positionScheduleType = positionScheduleService.GetPositionScheduleType();
            return Json(positionScheduleType, JsonRequestBehavior.AllowGet);
        }
    }
}
