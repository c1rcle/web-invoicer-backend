using System.Threading.Tasks;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Services
{
    public interface IGusService
    {
        Task<ResultHandler> GetCounterpartyDetails(string nip);
    }
}
