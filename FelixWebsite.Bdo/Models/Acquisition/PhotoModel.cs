namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class PhotoModel:UpdateModel
    {
        public string TitleText { get; set; }
        public string Text { get; set; }
        public string EncompassingDiv { get; set; }
        public bool Active { get; set; }
        public bool Multiple { get; set; }
        public bool CanSkip { get; set; }
        public string TooltipText { get; set; }

        public PhotoModel(string mediaFile, string value, string titleText, string text, string encompassingDiv, bool active = false, 
            bool multiple = false, bool canSkip = false, string tooltipText = null) :base(mediaFile, value)
        {
            MediaFile = mediaFile;
            Value = value;
            TitleText = titleText;
            Text = text;
            EncompassingDiv = encompassingDiv;
            Active = active;
            Multiple = multiple;
            CanSkip = canSkip;
            Update = false;
            TooltipText = tooltipText;
        }
    }
}
