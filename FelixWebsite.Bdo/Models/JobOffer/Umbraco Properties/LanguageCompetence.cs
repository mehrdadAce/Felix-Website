using FelixWebsite.Shared.enums;

namespace FelixWebsite.Bdo.Models.JobOffer
{
    public class LanguageCompetence
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public Score Score { get; set; }
    }
}
