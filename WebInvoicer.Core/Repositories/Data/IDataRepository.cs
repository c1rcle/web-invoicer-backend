using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Repositories.Data
{
    public interface IDataRepository<TData>
    {
        Task<TaskResult> Create(TData data, string email);

        Task<TaskResult<IEnumerable<TData>>> GetAll(string email);

        Task<TaskResult> Update(TData data, string email);

        Task<TaskResult> Delete(int resourceId, string email);
    }
}
