using FelixWebsite.Core.Models.SocialMedia.Interfaces;

namespace FelixWebsite.Core.Models.SocialMedia
{
    public class BaseReview: ISelectable
    {
        public int Id { get; set; }
        public bool Selected { get; set; }

    }
}
