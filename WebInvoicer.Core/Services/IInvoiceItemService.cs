using System.Threading.Tasks;
using WebInvoicer.Core.Dtos.InvoiceItem;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Services
{
    public interface IInvoiceItemService
    {
        Task<ResultHandler> Create(CreateInvoiceItemDto data, string email);

        Task<ResultHandler> GetAll(int invoiceId, string email);

        Task<ResultHandler> Update(InvoiceItemDto data, string email);

        Task<ResultHandler> Delete(int invoiceItemId, string email);        
    }
}
