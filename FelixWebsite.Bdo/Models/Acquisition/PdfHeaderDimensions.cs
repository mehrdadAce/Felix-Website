namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class PdfHeaderDimensions
    {
        public int LogoXOffset { get; set; }

        public int LogoYOffset { get; set; }

        public int HeaderXOffset { get; set; }

        public int HeaderYOffset { get; set; }

        public int BusinessLogoWidth { get; set; }

        public int BrandLogoWidth { get; set; }

        public double BusinessLogoRatio { get; set; }

        public double BrandLogoRatio { get; set; }


        public PdfHeaderDimensions()
        {
            LogoXOffset = 0;
            LogoYOffset = 0;
            HeaderXOffset = 0;
            HeaderYOffset = 0;
            BusinessLogoWidth = 0;
            BrandLogoWidth = 0;
            BusinessLogoRatio = 0;
            BrandLogoRatio = 0;
        }
    }
}