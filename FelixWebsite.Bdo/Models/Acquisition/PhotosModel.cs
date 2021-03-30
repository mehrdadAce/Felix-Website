
namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class PhotosModel
    {
        public PhotosModel(int amountOfDamagePhotos, bool isTakeOver)
        {
            IsTakeOver = isTakeOver;
            AmountOfDamagePhotos = amountOfDamagePhotos;
        }

        public int AmountOfDamagePhotos { get; set; }

        public bool IsTakeOver { get; set; }

    }
}
