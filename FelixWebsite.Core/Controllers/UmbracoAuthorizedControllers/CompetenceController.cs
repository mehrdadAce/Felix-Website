using System.Web.Mvc;
using Umbraco.Web.Mvc;
using FelixWebsite.Bll.Services.Interfaces;
using System.Threading.Tasks;
using FelixWebsite.Bll.Helpers;

namespace FelixWebsite.Core.Controllers.UmbracoAuthorizedControllers
{
    public class CompetenceController : UmbracoAuthorizedController
    {
        private ICompetencesService competencesService;

        public CompetenceController(ICompetencesService _competencesService)
        {
            this.competencesService = _competencesService;
        }
[HttpGet]
        public async Task<ActionResult> GetCompetences(string keyword)
        {
            var competences = await competencesService.GetCompetences(keyword);
            return Json(competences, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetCompetencePattern(string code)
        {
            var competencePattern = await competencesService.GetCompetencePatternAsync(code);
            return Json(competencePattern, JsonRequestBehavior.AllowGet);
        }

    }
}
