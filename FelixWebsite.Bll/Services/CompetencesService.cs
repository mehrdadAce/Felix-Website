using System;
using System.Net;
using System.Xml;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using Umbraco.Core.Logging;
using System.Threading.Tasks;
using FelixWebsite.Bll.Helpers;
using FelixWebsite.Bll.Services.Interfaces;

namespace FelixWebsite.Bll.Services
{
    public class CompetencesService : ICompetencesService
    {
        private static readonly HttpClient client = new HttpClient();

        public CompetencesService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        public async Task<string> GetCompetences(string keyword)
        {
            try
            {
                // Base Authentication
                byte[] byteArray = Encoding.ASCII.GetBytes(ConfigHelper.CompetencesUser + ":" + ConfigHelper.CompetencesPassword);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                // Post Body
                HttpContent key = new StringContent("{\"key\":\"" + keyword + "\"}", Encoding.UTF8, "application/json");

                // Post Request
                HttpResponseMessage res = await client.PostAsync(ConfigHelper.UrlCompetencesPattern + keyword, key);
                return await ConvertXMLToJSON(res);
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something failed when getting competencepatterns with keyword: {keyword}", e);
                return null;
            }
        }

        public async Task<string> GetCompetencePatternAsync(string code)
        {
            try
            {
                HttpResponseMessage res = await client.GetAsync(ConfigHelper.UrlCompetences + code);
                return await ConvertXMLToJSON(res);
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something failed when getting competencepattern with code: {code}", e);
                return null;
            }
        }

        private async Task<string> ConvertXMLToJSON(HttpResponseMessage result)
        {
            // Define response as XML
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(await result.Content.ReadAsStringAsync());

            // Convert XML to JSON
            return JsonConvert.SerializeXmlNode(xmlDoc).Replace("@", "").Replace("#", "");
        }
    }
}
