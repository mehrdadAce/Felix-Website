using System;
using System.Collections.Generic;

namespace FelixWebsite.Bdo.Models.JobOffer
{
    public class ConvertedUmbracoJobOffer
    {
        public string Id { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Status { get; set; }
        public string VdabName { get; set; }
        public string VdabJobEmail { get; set; }
        public string PositionTitle { get; set; }
        public string JobDescription { get; set; }
        public string RequiredQualifications { get; set; }
        public string RenumerationDescription { get; set; }
        public int ExperienceInMonths { get; set; }
        public CompanyInfo CompanyInfo { get; set; }
        public CompetencePattern Competences { get; set; }
        public LaborContract LaborContract { get; set; }
        public LaborSystem LaborSystem { get; set; }
        public LaborArrangement LaborArrangement { get; set; }
        public IEnumerable<Study> DiplomaSelector { get; set; }
        public IEnumerable<LanguageCompetence> Languages { get; set; }
        public IEnumerable<DriversLicense> DriverLicense { get; set; }
    }
}
