using System.Web.Mvc;

namespace FelixWebsite.Core.Controllers.SurfaceController
{
    public class ErrorController: Umbraco.Web.Mvc.SurfaceController
    {
        public ActionResult ErrorThrown500()
        {
            return View("Errorpage500");
        }
    }
}
