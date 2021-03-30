using System.Collections.Generic;

namespace FelixWebsite.Bdo.Models.JobOffer
{
    public class CompetencePattern
    {
        public string Id { get; set; }
        public List<Competence> competences { get; set; }
    }
}
