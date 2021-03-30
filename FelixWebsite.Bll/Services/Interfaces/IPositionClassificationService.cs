using FelixWebsite.Bdo.Models.JobOffer;
using System.Collections.Generic;

namespace FelixWebsite.Bll.Services.Interfaces
{
    public interface IPositionClassificationService
    {
        List<PositionClassification> GetPositionClassifications();
    }
}
