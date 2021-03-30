using FelixWebsite.Bdo.Models.Acquisition;
using FelixWebsite.Bll.Services.Interfaces.Base;
using FelixWebsite.Shared.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelixWebsite.Bll.Services.Interfaces
{
    public interface ISchemeDamageService:IBaseService<SchemeDamage>
    {
        SchemeDamage GetUserEntry(int userId, string imageType);
        void DeleteAll(int userId);

        IEnumerable<SchemeImage> GetSchemeOverviewModel(int userId);

        IEnumerable<SchemeImage> GetSchemeOverviewModelWithAllDamages(int userId);

        IEnumerable<SchemeDamage> GetAllForUserId(int userId);
    }
}
