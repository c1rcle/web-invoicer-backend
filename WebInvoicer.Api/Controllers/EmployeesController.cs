using Microsoft.AspNetCore.Mvc;
using WebInvoicer.Core.Dtos.Employee;
using WebInvoicer.Core.Services;

namespace WebInvoicer.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class EmployeesController : DataController<CreateEmployeeDto, EmployeeDto>
    {
        public EmployeesController(IDataService<CreateEmployeeDto, EmployeeDto> dataService)
            : base(dataService)
        {
        }
    }
}
