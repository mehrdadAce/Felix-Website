using System.Threading.Tasks;
using System.Web.Mvc;

namespace FelixWebsite.Bll.Services.Interfaces
{
    public interface ICompetencesService
    {
        Task<string> GetCompetences(string keyword);
        Task<string> GetCompetencePatternAsync(string code);
    }
}
