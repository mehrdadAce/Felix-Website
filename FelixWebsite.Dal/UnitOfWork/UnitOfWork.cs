using System;
using FelixWebsite.Dal.Entities;
using FelixWebsite.Dal.Entities.Base;
using FelixWebsite.Dal.Repositories;
using FelixWebsite.Dal.Repositories.Interfaces;
using FelixWebsite.Dal.Repositories.Interfaces.Base;

namespace FelixWebsite.Dal.UnitOfWork
{
    public class UnitOfWork
    {
        public FelixEntities.FelixEntities context;
        public IAcquisitionRepository AcquisitionRepository { get; set; }
        public IPhotoRepository PhotoRepository { get; set; }

        public ISchemeDamageRepository SchemeDamageRepository { get; set; }

        public UnitOfWork()
        {
            context = new FelixEntities.FelixEntities();
            AcquisitionRepository = new AcquisitionRepository(context);
            PhotoRepository = new PhotoRepository(context);
            SchemeDamageRepository = new SchemeDamageRepository(context);
        }

        public IBaseRepository<T> GetRepository<T>() where T : BaseEntity
        {
            switch (typeof(T))
            {
                case Type t when t == typeof(UserInformation):
                    return (IBaseRepository<T>)AcquisitionRepository;
                case Type t when t == typeof(PhotoInfo):
                    return (IBaseRepository<T>) PhotoRepository;
                case Type t when t == typeof(SchemeDamage):
                    return (IBaseRepository<T>)SchemeDamageRepository;
                default:
                    throw new NotImplementedException("No repository found for the given entity");
            }
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
