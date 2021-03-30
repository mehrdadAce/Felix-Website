using FelixWebsite.Bdo.Models;
using FelixWebsite.Bdo.Models.Acquisition;
using FelixWebsite.Bll.Services.Interfaces.Base;
using System.Collections.Generic;

namespace FelixWebsite.Bll.Services.Interfaces
{
    public interface IAcquisitionInformationService:IBaseService<UserInformation>
    {
        IEnumerable<UserInformation> GetAllCompletedAcquisition();

        IEnumerable<UserInformation> GetAllCompletedDamages();
    }
}
