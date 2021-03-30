using System.Web.Mvc;

namespace FelixWebsite.Core.Controllers.SurfaceController
{
    public class NewsletterController :  Umbraco.Web.Mvc.SurfaceController
    {
        [HttpGet]
        public ActionResult ShowSuccesMessage()
        {
            TempData["NewsStatus"] = true ;
            return null;
        }
    }
}
