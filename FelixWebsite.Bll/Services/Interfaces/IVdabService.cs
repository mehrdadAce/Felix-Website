using System.Threading.Tasks;
using System.Xml;

namespace FelixWebsite.Bll.Services.Interfaces
{
    public interface IVdabService
    {
        Task<string> PostToVdab(XmlDocument xml);
        Task<string> PostToOnlineAssistant(XmlDocument xml);
    }
}
