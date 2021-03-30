using System.Collections.Generic;
using FelixWebsite.Dal.Entities;
using FelixWebsite.Dal.Repositories.Base;
using FelixWebsite.Dal.Repositories.Interfaces;

namespace FelixWebsite.Dal.Repositories
{
    public class AcquisitionRepository : BaseRepository<UserInformation>, IAcquisitionRepository
    {
        public AcquisitionRepository(FelixEntities.FelixEntities context) : base(context)
        {
        }
    }
}
