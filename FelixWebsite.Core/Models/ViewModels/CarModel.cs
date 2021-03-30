using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelixWebsite.Core.Models.ViewModels
{
    public class CarInformation
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Clutch { get; set; }
        public string Secondhand { get; set; }
        public string Gas { get; set; }
        public string Id { get; set; }
        public string Url { get; set; }
        public CarInformation(string[] queryParts, string absoluteUrl)
        {
            Brand = queryParts[0];
            Model = queryParts[1];
            Clutch = queryParts[2];
            Secondhand = queryParts[3];
            Gas = queryParts[4];
            Id = queryParts[5];
            Url = absoluteUrl;
        }
    }
}
