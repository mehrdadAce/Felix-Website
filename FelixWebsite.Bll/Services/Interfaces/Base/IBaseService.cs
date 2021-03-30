using System.Collections.Generic;
using FelixWebsite.Bdo.Models.Base;

namespace FelixWebsite.Bll.Services.Interfaces.Base
{
    public interface IBaseService<TModel> where TModel: BaseModel
    {
        IEnumerable<TModel> GetAll();
        TModel GetById(int id);
        int SaveModel(TModel model);
        int UpdateModel(TModel model);
        bool DeleteModel(int id, int userId);
    }
}
