using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Helpers
{
    public static class ActionsColorHelper
    {
        private static readonly string defaultSolidBackground = "rgba(207, 2, 39)";
        private static readonly string defaultBackgroundGradient = "rgba(214, 214, 214, 0.9)";
        
        public static string GetBackgroundGradient(this IPublishedContent currentPage)
        {
            string color = string.Empty;
            if (currentPage is Brand)
            {
                color = currentPage.OfType<Brand>().Color.Color.ConvertHexToRgba(7);
            }
            else if (currentPage is BusinessBrand)
            {
                var brand = currentPage.OfType<BusinessBrand>().Brand.FirstOrDefault() as Brand;
                color = brand.Color.Color.ConvertHexToRgba(7);
            }
            else if(currentPage is BusinessCombined)
            {
                color = currentPage.OfType<BusinessCombined>().Color.Color.ConvertHexToRgba(7);
            }
            else
            {
                color = defaultBackgroundGradient;
            }
            return "0deg," + color + "," + color;
        }

        public static string GetSolidBackground(this IPublishedContent currentPage)
        {
            string color = string.Empty;
            if (currentPage is Brand)
            {
                color = currentPage.OfType<Brand>().Color.Color.ConvertHexToRgba(999);
            }
            else if(currentPage is BusinessBrand)
            {
                color = currentPage.OfType<BusinessBrand>().Brand.FirstOrDefault().OfType<Brand>().Color.Color.ConvertHexToRgba(999);
            }
            else if (currentPage is BusinessCombined)
            {
                color = currentPage.OfType<BusinessCombined>().Color.Color.ConvertHexToRgba(999);
            }
            else
            {
                color = defaultSolidBackground;
            }
            return color;
        }
    }
}
