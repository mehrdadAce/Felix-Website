using FelixWebsite.Bll.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Core;

namespace FelixWebsite.Core.Controllers.SurfaceController
{
    public class InsuranceApiController : BaseCarrosserieLinkedSurfaceController
    {
        public async Task<ActionResult> GetInsurances()
        {
            try
            {
                Logger.Debug(GetType(), "Getting insurances");
                string carrosseriePlatFormLink = GetCarrosseriePlatformLinkFromBreadcrumbRoot();
                var insuranceUrl = carrosseriePlatFormLink + ConfigHelper.InsuranceApiUrl;
                var authToken = ConfigHelper.CarrosserieApiAuthToken;
                Logger.Debug(GetType(), $"Using URL {insuranceUrl} with token {authToken}");
                List<string> insuranceList = new List<string>();
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Auth-Token", authToken);
                    var json = await client.GetAsync(insuranceUrl);
                    Logger.Debug(GetType(), $"Http call returned json {json}");
                    var jsonString = await json.Content.ReadAsStringAsync();
                    var insurances = JsonConvert.DeserializeObject<IEnumerable<string>>(jsonString);
                    insuranceList = insurances.ToList();
                    Logger.Debug(GetType(), $"{insuranceList.Count} insurances found");
                }

                insuranceList.Sort();
                insuranceList.RemoveAll(str => string.IsNullOrEmpty(str)
                         || str.All(x => x.IsUpperCase())
                         || str.Contains("EIGEN SCHADE niet via verzekering"));

                Logger.Debug(GetType(), $"filtered list has {insuranceList.Count} insurances remaining");

                return Json(insuranceList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Logger.Error(GetType(), "Something went wrong while getting all the insurances: ", e);
                return null;
            }
        }
    }
}