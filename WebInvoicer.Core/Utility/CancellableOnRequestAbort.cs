using System.Threading;
using Microsoft.AspNetCore.Http;

namespace WebInvoicer.Core.Utility
{
    public class CancellableOnRequestAbort
    {
        private readonly IHttpContextAccessor contextAccessor;

        public CancellableOnRequestAbort(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        protected CancellationToken GetCancellationToken()
        {
            return contextAccessor.HttpContext.RequestAborted;
        }
    }
}
