using AutoMapper;
using WebInvoicer.Core.Dtos.Counterparty;
using WebInvoicer.Core.Dtos.Employee;
using WebInvoicer.Core.Dtos.Product;
using WebInvoicer.Core.Dtos.User;
using WebInvoicer.Core.Models;

namespace WebInvoicer.Core.Dtos
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ApplicationUser, UserDataDto>();

            CreateMap<CreateCounterpartyDto, Models.Counterparty>().ReverseMap();
            CreateMap<CounterpartyDto, Models.Counterparty>().ReverseMap();

            CreateMap<CreateEmployeeDto, Models.Employee>().ReverseMap();
            CreateMap<EmployeeDto, Models.Employee>().ReverseMap();

            CreateMap<CreateProductDto, Models.Product>().ReverseMap();
            CreateMap<ProductDto, Models.Product>().ReverseMap();
        }
    }
}
