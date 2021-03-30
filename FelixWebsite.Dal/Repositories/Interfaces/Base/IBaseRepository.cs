using System.Collections.Generic;
using System.Linq;
using FelixWebsite.Dal.Entities.Base;

namespace FelixWebsite.Dal.Repositories.Interfaces.Base
{
    public interface IBaseRepository<TEntity> where TEntity:BaseEntity
    {
        IQueryable<TEntity> GetAll();
        TEntity GetById(int id);
        void SaveEntity(TEntity entity);
    }
}
