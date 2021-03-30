using System;
using System.Collections.Generic;
using System.Linq;
using FelixWebsite.Dal.Entities;
using FelixWebsite.Dal.Entities.Base;
using FelixWebsite.Dal.Repositories.Interfaces.Base;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace FelixWebsite.Dal.Repositories.Base
{
    public abstract class BaseRepository<TEntity>: IBaseRepository<TEntity> where TEntity:BaseEntity
    {
        protected Database Database;
        public virtual IQueryable<TEntity> Entities { get; set; }
        protected FelixEntities.FelixEntities context;
        protected BaseRepository(FelixEntities.FelixEntities context)
        {
            Database = ApplicationContext.Current.DatabaseContext.Database;
            this.context = context;
            Entities = context.Set<TEntity>();
        }
        public IQueryable<TEntity> GetAll()
        {
            return Entities;
        }

        public TEntity GetById(int id)
        {
            return Entities.FirstOrDefault(entity => entity.Id == id);
        }

        public void SaveEntity(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }
    }
}
