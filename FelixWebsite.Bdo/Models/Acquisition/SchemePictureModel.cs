using FelixWebsite.Core.enums;
using FelixWebsite.Shared.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class SchemePictureModel
    {
        public string SchemePartUrl { get; set; }

        public string ImageType { get; set; }

        public PhotoIdentifier PhotoIdentifier { get; set; }

        public SchemeEntry SchemeEntry { get; set; }

        public IEnumerable<MediaValuesModel> SchemePictures { get; set; }

        public SchemePictureModel(string schemePartUrl, string imageType, PhotoIdentifier photoIdentifier)
        {
            SchemePartUrl = schemePartUrl;
            ImageType = imageType;
            PhotoIdentifier = photoIdentifier;
        }
    }
}
