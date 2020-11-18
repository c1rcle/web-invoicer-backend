using Microsoft.AspNetCore.Mvc;
using WebInvoicer.Core.Dtos.Invoice;
using WebInvoicer.Core.Services;

namespace WebInvoicer.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class InvoicesController : DataController<CreateInvoiceDto, InvoiceDto>
    {
        public InvoicesController(IDataService<CreateInvoiceDto, InvoiceDto> dataService)
            : base(dataService)
        {
        }
    }
}
