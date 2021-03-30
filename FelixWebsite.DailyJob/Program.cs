using System;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using FelixWebsite.Core.Models;
using FelixWebsite.Bll.Helpers;
using System.Xml.Serialization;
using System.Collections.Generic;
using FelixWebsite.Core.Models.JobOffer;
using System.Linq;
using System.Text;
using System.Net.Http.Headers;

namespace FelixWebsite.DailyJob
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private string VdabUrl { get; set; }
        public Program()
        {
            VdabUrl = ConfigHelper.VdabUrl;
        }
        static void Main(string[] args)
        {
            MainAsyncFunction().Wait();
            Console.ReadKey();
        }

        
    }
}
