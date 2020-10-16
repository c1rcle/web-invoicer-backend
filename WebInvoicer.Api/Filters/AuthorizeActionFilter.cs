using Microsoft.AspNetCore.Mvc.Authorization;

namespace WebInvoicer.Api.Filters
{
    public class AuthorizeActionFilter : AuthorizeFilter
    {
        public AuthorizeActionFilter() : base()
        {
        }
    }
}
