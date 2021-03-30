using FelixWebsite.Core.enums;
using System.Collections.Generic;
using System.Linq;

namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class OverviewDamageModel: OverviewModel
    {
        public IEnumerable<SchemePictureModel> SchemePictures { get; set; }

        public override void SetOverviewUrls()
        {
            base.SetOverviewUrls();

            foreach(var schemePicture in SchemePictures)
            {
                OverviewUrls.AddRange(schemePicture.SchemePictures.Select(x => x?.GetDamageEntry(true)));
            }           
        }
    }
}
