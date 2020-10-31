using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebInvoicer.Core.Attributes;
using WebInvoicer.Core.Dtos.Counterparty;
using WebInvoicer.Core.Gus;
using WebInvoicer.Core.Services;

namespace WebInvoicer.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class CounterpartiesController : DataController<CreateCounterpartyDto, CounterpartyDto>
    {
        private readonly IGusService gusService;

        public CounterpartiesController(IGusService gusService,
            IDataService<CreateCounterpartyDto, CounterpartyDto> dataService) : base(dataService)
        {
            this.gusService = gusService;
        }

        [HttpGet("details/{nip}")]
        [ProducesResponseType(typeof(QueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetCounterpartyDetails([ValidNip] string nip)
        {
            return (await gusService.GetCounterpartyDetails(nip)).GetActionResult(this);
        }
    }
}
