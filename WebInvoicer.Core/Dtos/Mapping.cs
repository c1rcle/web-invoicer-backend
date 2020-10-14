using AutoMapper;
using WebInvoicer.Core.Dtos.User;
using WebInvoicer.Core.Models;

namespace WebInvoicer.Core.Dtos
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ApplicationUser, UserDataDto>();
        }
    }
}
