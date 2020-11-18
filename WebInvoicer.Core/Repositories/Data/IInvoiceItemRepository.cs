using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoicer.Core.Models;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Repositories.Data
{
    public interface IInvoiceItemRepository
    {
        Task<TaskResult> Create(InvoiceItem data, string email);

        Task<TaskResult<IEnumerable<InvoiceItem>>> GetAll(int invoiceId, string email);

        Task<TaskResult> Update(InvoiceItem data, string email);

        Task<TaskResult> Delete(int invoiceItemId, string email);
    }
}
