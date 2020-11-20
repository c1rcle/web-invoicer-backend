using System.Threading.Tasks;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Services
{
    public interface IDataService<in TCreateDto, in TUpdateDto>
    {
        Task<ResultHandler> Create(TCreateDto data, string email);

        Task<ResultHandler> Get(int resourceId, string email);

        Task<ResultHandler> GetAll(string email);

        Task<ResultHandler> Update(TUpdateDto data, string email);

        Task<ResultHandler> Delete(int resourceId, string email);
    }
}
