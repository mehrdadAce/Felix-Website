using AutoMapper;
using FelixWebsite.Bdo.Models.Acquisition;
using FelixWebsite.Bll.Services;
using FelixWebsite.Bll.Services.Interfaces;
using FelixWebsite.Shared.enums;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FelixWebsite.Core.Controllers.SurfaceController
{
    public class AcquisitionSchemeController : Umbraco.Web.Mvc.SurfaceController
    {
        private ISchemeDamageService schemeDamageService;

        public AcquisitionSchemeController(ISchemeDamageService schemeDamageService)
        {
            this.schemeDamageService = schemeDamageService;
        }

        public ActionResult DeleteOnPageLoad(int userId)
        {
            schemeDamageService.DeleteAll(userId);
            return Json("Alles verwijderd bij laden", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSchemeData(int userId, string imageEntry)
        {
            var userEntry = schemeDamageService.GetUserEntry(userId, imageEntry);
            if(userEntry == null)
            {
                userEntry = new SchemeDamage();
            }
            
            return Json(userEntry, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PostSchemeData(SchemeDamageData schemeDamageData)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(schemeDamageData, JsonRequestBehavior.AllowGet);
            }

            var schemeDamage = Mapper.Map<SchemeDamage>(schemeDamageData);
            schemeDamage.SchemeEntry = SchemeDamageService.SelectSchemeEntry(schemeDamageData.ImageType);

            var databaseEntry = schemeDamageService.GetUserEntry(schemeDamage.UserId, schemeDamageData.ImageType);
            int result;
            if (databaseEntry != null)
            {
                schemeDamage.Id = databaseEntry.Id;
                result = schemeDamageService.UpdateModel(schemeDamage);
            }
            else
            {
                result = schemeDamageService.SaveModel(schemeDamage);
            }

            return Json(schemeDamage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSchemeOverview(int userId)
        {
            var schemeOverviewModel = schemeDamageService.GetSchemeOverviewModel(userId);
            return PartialView("~/Views/Partials/Acquisition/SchemeOverview.cshtml", schemeOverviewModel.ToArray());
        }
        [HttpGet]
        public ActionResult GetSchemeOverviewWithAllDamages(int userId)
        {
            var schemeOverviewModel = schemeDamageService.GetSchemeOverviewModelWithAllDamages(userId);
            return PartialView("~/Views/Partials/Acquisition/SchemeOverview.cshtml", schemeOverviewModel.ToArray());
        }
    }
}
