using FelixWebsite.Core.Models.SocialMedia.Interfaces;

namespace FelixWebsite.Core.Models.SocialMedia
{
    public class BaseArticle: ISelectable
    {
        public bool Selected { get; set; }
    }
}
