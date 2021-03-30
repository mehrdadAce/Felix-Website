using System.Web;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Models
{
    public class JobApplicationForm : ContactForm
    {
        public HttpPostedFileBase attachment { get; set; }
        public string JobTitle { get; set; }
        public string BrandTitle { get; set; }
        public string BusinessName { get; set; }
        public string BusinessLocation { get; set; }
        public string PageUrl { get; set; }

    }
}
