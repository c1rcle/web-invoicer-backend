using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoicer.Core.Models;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Repositories.Data
{
    public interface IInvoiceItemRepository
    {
        Task<TaskResult<InvoiceItem>> Create(InvoiceItem data);

        Task<TaskResult<IEnumerable<InvoiceItem>>> GetAll(int invoiceId);

        Task<TaskResult> Update(InvoiceItem data);

        Task<TaskResult> Delete(int invoiceItemId);
    }
}
