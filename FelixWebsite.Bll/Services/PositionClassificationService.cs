using FelixWebsite.Bdo.Models.JobOffer;
using FelixWebsite.Bll.Services.Interfaces;
using System.Collections.Generic;

namespace FelixWebsite.Bll.Services
{
    public class PositionClassificationService : IPositionClassificationService
    {
        public List<PositionClassification> GetPositionClassifications()
        {
            return new List<PositionClassification>()
            {
                new PositionClassification() {Code = "Temp to Hire", Beschrijving = "Tijdelijke job" },
                new PositionClassification() {Code = "Direct Hire", Beschrijving = "Gewoon contract" },
                new PositionClassification() {Code = "Apprenticeship", Beschrijving = "Leercontract" },
                new PositionClassification() {Code = "Flexijob", Beschrijving = "Flexijob" },
                new PositionClassification() {Code = "x:Early Retirement Replacement", Beschrijving = "vervanging voor brugpensioen" },
                new PositionClassification() {Code = "Limited Employment", Beschrijving = "Minder dan 13 uren per week" },
                new PositionClassification() {Code = "x:Apprenticeship Entreprise", Beschrijving = "Ondernemingsopleiding" },
                new PositionClassification() {Code = "x:Baby Minder", Beschrijving = "Onthaalouder" },
                new PositionClassification() {Code = "x:Student Job", Beschrijving = "Studentenjob" },
                new PositionClassification() {Code = "x:Self-Employed Activity", Beschrijving = "Zelfstandige activiteit" },
                new PositionClassification() {Code = "x:Service Voucher Employment", Beschrijving = "Dienstencheque baan" },
                new PositionClassification() {Code = "x:Spontaneous Application / Reserve", Beschrijving = "Spontane sollicitatie / werfreserve" },
            };
        }
    }
}
