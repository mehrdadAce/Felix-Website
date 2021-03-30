using System.Linq;
using FelixWebsite.Dal.Entities;
using FelixWebsite.Dal.Repositories.Interfaces.Base;
using FelixWebsite.Core.enums;

namespace FelixWebsite.Dal.Repositories.Interfaces
{
    public interface IPhotoRepository:IBaseRepository<PhotoInfo>
    {
        IQueryable<PhotoInfo> GetAllWithUserId(int id);
        PhotoInfo GetWithPhotoIdentifierAndUserId(PhotoIdentifier photoIdentifier, int userId);
        IQueryable<PhotoInfo> GetAllWithIdentifier(PhotoIdentifier identifier, int userId);
    }
}
