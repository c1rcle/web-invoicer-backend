using Microsoft.AspNetCore.Mvc;
using WebInvoicer.Core.Dtos.Product;
using WebInvoicer.Core.Services;

namespace WebInvoicer.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ProductsController : DataController<CreateProductDto, ProductDto>
    {
        public ProductsController(IDataService<CreateProductDto, ProductDto> dataService)
            : base(dataService)
        {
        }
    }
}
