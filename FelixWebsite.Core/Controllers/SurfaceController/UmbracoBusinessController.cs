using FelixWebsite.Core.Helpers;
using FelixWebsite.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Controllers.SurfaceController
{
    /// <summary>
    /// Use mailadres from carflow to return a businessBrand object from Umbraco.
    /// </summary>
    public class UmbracoBusinessController : Umbraco.Web.Mvc.SurfaceController
    {
        [HttpGet]
        public ActionResult GetUmbracoBusinessByMailAdress(string mailAddress, string carModel)
        {
            try
            {
                GroupHome home = Umbraco.TypedContentAtRoot().OfType<GroupHome>().FirstOrDefault();
                Businesses businesses = home.Children.OfType<Businesses>().FirstOrDefault();
                List<BusinessBrand> businessesConnectedToBrand = businesses.Children.OfType<BusinessBrand>().ToList();
                BusinessBrand res = businessesConnectedToBrand.FirstOrDefault(x => x.BusinessMail == mailAddress);
                if (res == null)
                {
                    var root = Session.GetBreadcrumbRoot(home);
                    if (root is BusinessBrand)
                    {
                        res = root as BusinessBrand;
                    }
                    else
                    {
                        //Fallback to the first BusinessBrand
                        res = businesses.Children.OfType<BusinessBrand>().FirstOrDefault();
                    }
                }
                return Json(new UmbracoBusinessInfo
                {
                    Id = res.Id,
                    Link = res.Url,
                    ImageUrl = res.BusinessImage.Url,
                    Brand = res.Brand.FirstOrDefault().OfType<Brand>().Title,
                    Name = res.BusinessName,
                    Mail = res.BusinessMail,
                    Phone = res.BusinessPhone,
                    Adress = res.BusinessStreetName + " " + res.BusinessBuildingNumber + ", " + res.BusinessPostalCode + " " + res.BusinessMunicipality,
                    Location = res.BusinessMunicipality,
                    CarModel = carModel
                }, JsonRequestBehavior.AllowGet) ;
            }
            catch (System.Exception e)
            {
                LogHelper.Debug(GetType(), "Error while getting umbraco business " + e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
