using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Net.Http;
using Umbraco.Core.Logging;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using FelixWebsite.Bll.Helpers.VDAB;
using FelixWebsite.Bll.Services.Interfaces;
using FelixWebsite.Bll.Helpers;

namespace FelixWebsite.Bll.Services
{
    public class VdabService : IVdabService
    {
        private static readonly HttpClient client = new HttpClient();

        private static void SetHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            client.DefaultRequestHeaders.Add("X-IBM-Client-Id", ConfigHelper.AuthorisationKeyVDAB);
            client.DefaultRequestHeaders.Add("accept", "application/xml");
        }

        private HttpContent LoadXmlInBody(XmlDocument xml)
        {
            StringWriter sw = new Utf8StringWriter();
            xml.Save(sw);
            String formattedXml = sw.ToString();
            //formattedXml = formattedXml.Replace("<ns2:Value>", "<ns2:Value><![CDATA[");
            //formattedXml = formattedXml.Replace("</ns2:Value>", "]]></ns2:Value>");
            return new StringContent(formattedXml, Encoding.UTF8, "application/xml");
        }

        public async Task<string> PostToOnlineAssistant(XmlDocument xml)
        {
            try
            {
                HttpResponseMessage res = null;
                using (var client = new HttpClient())
                {
                    SetHeaders(client);
                    res = await client.PostAsync(ConfigHelper.UrlJobApplication + "/kwaliteit", LoadXmlInBody(xml));
                }
                return await res.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something failed when posting the following xml to the online assistant: {xml}", e);
                return null;
            }
        }

        public async Task<string> PostToVdab(XmlDocument xml)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    SetHeaders(client);
                    var res = await client.PostAsync(ConfigHelper.UrlJobApplication, LoadXmlInBody(xml));
                    if (!res.IsSuccessStatusCode)
                    {
                        throw new Exception($"Got following incorrect response from VDAB service: {await res.Content.ReadAsStringAsync()}");
                    }
                    return await res.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something failed when posting the following xml to the VDAB database: {xml}", e);
                return null;
            }
        }
    }
}
