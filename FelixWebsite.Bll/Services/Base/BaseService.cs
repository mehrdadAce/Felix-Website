using AutoMapper;
using FelixWebsite.Bdo.Models.Base;
using FelixWebsite.Bll.Services.Interfaces.Base;
using FelixWebsite.Dal.Entities.Base;
using FelixWebsite.Dal.Repositories.Interfaces.Base;
using FelixWebsite.Dal.UnitOfWork;
using System;
using System.Collections.Generic;
using Umbraco.Core.Logging;

namespace FelixWebsite.Bll.Services.Base
{
    public class BaseService<TModel, TEntity> : IBaseService<TModel> where TModel : BaseModel where TEntity : BaseEntity
    {
        protected UnitOfWork UnitOfWork;
        protected IBaseRepository<TEntity> Repository;

        public BaseService(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            Repository = UnitOfWork.GetRepository<TEntity>();
        }

        public bool DeleteModel(int id, int userId)
        {
            try
            {
                var entity = Repository.GetById(id);
                entity.Delete(userId);
                UnitOfWork.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something went wrong when deleting a {typeof(TModel)} with id: {id} and userId: {userId}. The thrown error is: ", e);
                return false;
            }
        }

        public IEnumerable<TModel> GetAll()
        {
            try
            {
                var allEntities = Repository.GetAll();
                return Mapper.Map<IEnumerable<TModel>>(allEntities);
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something went wrong while getting all {typeof(TModel)}. The thrown error is: ", e);
                return null;
            }
        }

        public TModel GetById(int id)
        {
            try
            {
                var entity = Repository.GetById(id);
                return Mapper.Map<TModel>(entity);
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something went wrong when getting a {typeof(TModel)} with id: {id}. The thrown error is: ", e);
                return null;
            }
        }

        public int SaveModel(TModel model)
        {
            try
            {
                var entity = Mapper.Map<TEntity>(model);
                Repository.SaveEntity(entity);
                UnitOfWork.SaveChanges();
                return entity.Id;
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something went wrong when saving a {typeof(TModel)}. The thrown error is: ", e);
                return 0;
            }
        }

        public int UpdateModel(TModel model)
        {
            try
            {
                var dalEntity = Repository.GetById(model.Id);
                Mapper.Map(model, dalEntity);
                UnitOfWork.SaveChanges();
                return model.Id;
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something went wrong when updating a {typeof(TModel)} with id: {model.Id}. The thrown error is: ", e);
                return 0;
            }
        }
    }
}