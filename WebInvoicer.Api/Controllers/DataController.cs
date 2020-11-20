using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebInvoicer.Api.Extensions;
using WebInvoicer.Core.Services;

namespace WebInvoicer.Api.Controllers
{
    public class DataController<TCreateDto, TUpdateDto> : ControllerBase
    {
        private readonly IDataService<TCreateDto, TUpdateDto> service;

        public DataController(IDataService<TCreateDto, TUpdateDto> service) =>
            this.service = service;

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public virtual async Task<IActionResult> Create([FromBody] TCreateDto data)
        {
            return (await service.Create(data, HttpContext.GetEmailFromClaims()))
                .GetActionResult(this);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            return (await service.Get(id, HttpContext.GetEmailFromClaims())).GetActionResult(this);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            return (await service.GetAll(HttpContext.GetEmailFromClaims())).GetActionResult(this);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public virtual async Task<IActionResult> Update([FromBody] TUpdateDto data)
        {
            return (await service.Update(data, HttpContext.GetEmailFromClaims()))
                .GetActionResult(this);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Delete(int id)
        {
            return (await service.Delete(id, HttpContext.GetEmailFromClaims()))
                .GetActionResult(this);
        }
    }
}
