using AutoMapper;
using WebInvoicer.Core.Dtos.Counterparty;
using WebInvoicer.Core.Dtos.Employee;
using WebInvoicer.Core.Dtos.Invoice;
using WebInvoicer.Core.Dtos.InvoiceItem;
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
            CreateMap<SetCompanyDetailsDto, ApplicationUser>();

            CreateMap<CreateCounterpartyDto, Models.Counterparty>();
            CreateMap<CounterpartyDto, Models.Counterparty>().ReverseMap();

            CreateMap<CreateEmployeeDto, Models.Employee>();
            CreateMap<EmployeeDto, Models.Employee>().ReverseMap();

            CreateMap<CreateProductDto, Models.Product>();
            CreateMap<ProductDto, Models.Product>().ReverseMap();

            CreateMap<CreateInvoiceDto, Models.Invoice>();
            CreateMap<InvoiceDto, Models.Invoice>()
                .ForMember(x => x.Type, x => x.Ignore());
            CreateMap<Models.Invoice, InvoiceDto>();

            
            CreateMap<CreateInvoiceItemDto, Models.InvoiceItem>();
            CreateMap<InvoiceItemDto, Models.InvoiceItem>().ReverseMap();
        }
    }
}
