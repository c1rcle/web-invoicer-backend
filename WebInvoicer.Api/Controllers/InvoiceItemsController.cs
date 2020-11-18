using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebInvoicer.Api.Extensions;
using WebInvoicer.Core.Dtos.InvoiceItem;
using WebInvoicer.Core.Services;

namespace WebInvoicer.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class InvoiceItemsController : ControllerBase
    {
        private readonly IInvoiceItemService invoiceItemService;

        public InvoiceItemsController(IInvoiceItemService invoiceItemService) =>
            this.invoiceItemService = invoiceItemService;

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] CreateInvoiceItemDto data)
        {
            return (await invoiceItemService.Create(data, HttpContext.GetEmailFromClaims()))
                .GetActionResult(this);
        }

        [HttpGet("{invoiceId}")]
        [ProducesResponseType(typeof(InvoiceItemDto[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll(int invoiceId)
        {
            return (await invoiceItemService.GetAll(invoiceId, HttpContext.GetEmailFromClaims()))
                .GetActionResult(this);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update([FromBody] InvoiceItemDto data)
        {
            return (await invoiceItemService.Update(data, HttpContext.GetEmailFromClaims()))
                .GetActionResult(this);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            return (await invoiceItemService.Delete(id, HttpContext.GetEmailFromClaims()))
                .GetActionResult(this);
        }
    }
}
