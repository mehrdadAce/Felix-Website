using System.Web.Mvc;
using FelixWebsite.Bdo.Models;
using FelixWebsite.Core.App_GlobalResources;

namespace FelixWebsite.Core.Controllers.SurfaceController
{
    public class GlobalResourcesController:Umbraco.Web.Mvc.SurfaceController
    {
        [System.Web.Http.HttpGet]
        public ActionResult GetAcquisitionNotificationMessages()
        {
            var error = FelixResources.acquisition_notification_error;
            var notification = new Notification {ErrorMessage = error};
            return Json( notification, JsonRequestBehavior.AllowGet);
        }

    }
}
