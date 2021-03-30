using System.Linq;
using FelixWebsite.Dal.Entities;
using FelixWebsite.Dal.Repositories.Base;
using FelixWebsite.Dal.Repositories.Interfaces;
using FelixWebsite.Core.enums;

namespace FelixWebsite.Dal.Repositories
{
    public class PhotoRepository:BaseRepository<PhotoInfo>, IPhotoRepository
    {
        public PhotoRepository(FelixEntities.FelixEntities context) : base(context)
        {
        }

        public IQueryable<PhotoInfo> GetAllWithUserId(int id)
        {
            return Entities.Where(entity => entity.UserId == id);
        }

        public PhotoInfo GetWithPhotoIdentifierAndUserId(PhotoIdentifier photoIdentifier, int userId)
        {
            return Entities.FirstOrDefault(photoId =>
                photoId.UserId == userId && photoId.PhotoIdentifier == photoIdentifier);
        }

        public IQueryable<PhotoInfo> GetAllWithIdentifier(PhotoIdentifier identifier, int userId)
        {
            return Entities.Where(entity => entity.PhotoIdentifier == identifier && entity.UserId == userId);
        }
    }
}
