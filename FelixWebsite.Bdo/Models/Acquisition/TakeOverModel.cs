using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core.Models;

namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class TakeOverModel
    {
        public int AmountOfDamagePhotos { get; set; }

        public string PopupTitle { get; set; }

        public IHtmlString PopupText { get; set; }

        public string PopupSchemeTitle { get; set; }

        public IHtmlString PopupSchemeText { get; set; }

        public IPublishedContent CurrentPage { get; set; }

        public bool IsTakeOver { get; set; }
    }
}
