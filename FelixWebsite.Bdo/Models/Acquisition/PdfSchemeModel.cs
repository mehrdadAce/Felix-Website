using System.Collections.Generic;
namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class PdfSchemeModel
    {
        public IEnumerable<SchemePictureModel> SchemePictures { get; set; }

        public IEnumerable<SchemeDamage> SchemeDamages { get; set; }

        public PdfSchemeModel(IEnumerable<SchemeDamage> schemeDamages, IEnumerable<SchemePictureModel> schemePictures)
        {
            SchemePictures = schemePictures;
            SchemeDamages = schemeDamages;
        }
    }
}
