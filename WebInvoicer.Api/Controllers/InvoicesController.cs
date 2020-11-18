using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebInvoicer.Core.Dtos.Invoice;
using WebInvoicer.Core.Models;
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

        public override async Task<IActionResult> Create([FromBody] CreateInvoiceDto data)
        {
            if (!IsValidRequest(data))
            {
                return BadRequest("Invalid property values for type!");
            }

            return await base.Create(data);
        }

        public override async Task<IActionResult> Update([FromBody] InvoiceDto data)
        {
            if (!IsValidRequest(data))
            {
                return BadRequest("Invalid property values for type!");
            }

            return await base.Update(data);
        }

        private bool IsValidRequest(CreateInvoiceDto data)
        {
            return data switch
            {
                var x when x.Type == InvoiceType.Receipt 
                    && x.CounterpartyId == null 
                    && x.PaymentType == null 
                    && x.PaymentDeadline == null => true,

                var x when x.Type != InvoiceType.Receipt 
                    && x.CounterpartyId != null 
                    && x.PaymentType != null 
                    && x.PaymentDeadline != null => true,

                _ => false
            };
        }

        private bool IsValidRequest(InvoiceDto data)
        {
            return !(data switch
            {
                var x when x.Type == InvoiceType.Receipt 
                    && (x.CounterpartyId != null 
                    || x.PaymentType != null 
                    || x.PaymentDeadline != null) => true,

                _ => false
            });
        }
    }
}
