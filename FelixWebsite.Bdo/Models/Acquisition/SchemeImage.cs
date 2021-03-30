
namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class SchemeImage
    {
        public int OrderNumber { get; set; }

        public string ImageLocation { get; set; }

        public string Name { get; set; }

        public string ReadableName { get; set; }

        public bool IsRed { get; set; }

        public SchemeImage(int orderNumber, string imageLocation, string name, string readableName)
        {
            OrderNumber = orderNumber;
            ImageLocation = imageLocation;
            Name = name;
            ReadableName = readableName;
        }
    }
}
