using FelixWebsite.Dal.Entities;
using FelixWebsite.Dal.Repositories.Base;
using FelixWebsite.Dal.Repositories.Interfaces;

namespace FelixWebsite.Dal.Repositories
{
    public class SchemeDamageRepository: BaseRepository<SchemeDamage>, ISchemeDamageRepository
    {
        public SchemeDamageRepository(FelixEntities.FelixEntities context): base(context)
        {
        }
    }
}
