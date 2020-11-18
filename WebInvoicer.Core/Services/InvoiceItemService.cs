using System.Threading.Tasks;
using AutoMapper;
using WebInvoicer.Core.Dtos.InvoiceItem;
using WebInvoicer.Core.Models;
using WebInvoicer.Core.Repositories.Data;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Services
{
    public class InvoiceItemService : IInvoiceItemService
    {
        private readonly IInvoiceItemRepository repository;

        private readonly IMapper mapper;

        public InvoiceItemService(IInvoiceItemRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ResultHandler> Create(CreateInvoiceItemDto data, string email)
        {
            return ResultHandler
                .HandleTaskResult(await repository.Create(mapper.Map<InvoiceItem>(data), email));
        }

        public async Task<ResultHandler> GetAll(int invoiceId, string email)
        {
            var result = ResultHandler.HandleTaskResult(await repository.GetAll(invoiceId, email));
            result.MapPayload<InvoiceItem, InvoiceItemDto>(mapper);
            return result;
        }

        public async Task<ResultHandler> Update(InvoiceItemDto data, string email)
        {
            return ResultHandler
                .HandleTaskResult(await repository.Update(mapper.Map<InvoiceItem>(data), email));
        }

        public async Task<ResultHandler> Delete(int invoiceItemId, string email)
        {
            return ResultHandler.HandleTaskResult(await repository.Delete(invoiceItemId, email));
        }
    }
}
