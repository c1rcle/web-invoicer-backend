using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebInvoicer.Core.Utility
{
    public class ResultHandler
    {
        public HttpStatusCode StatusCode { get; }

        public object Payload { get; set; }

        public ResultHandler(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public ResultHandler(HttpStatusCode statusCode, object payload)
        {
            StatusCode = statusCode;
            Payload = payload;
        }

        public IActionResult GetActionResult(ControllerBase controller)
        {
            if (Payload != null)
            {
                return controller.StatusCode((int)StatusCode, Payload);
            }
            return controller.StatusCode((int)StatusCode);
        }

        public void MapPayload<TDestination>(IMapper mapper)
        {
            Payload = mapper.Map<TDestination>(Payload);
        }

        public static async Task<ResultHandler> HandleRepositoryCall<TData, TResult>(
            Func<TData, Task<TResult>> call, TData data)
        {
            var result = await call(data);
            var statusCode = result switch
            {
                IdentityResult { Succeeded: true } => HttpStatusCode.NoContent,
                object => HttpStatusCode.OK,
                _ => HttpStatusCode.UnprocessableEntity
            };

            return statusCode == HttpStatusCode.OK
                ? new ResultHandler(statusCode, result)
                : new ResultHandler(statusCode);
        }
    }
}
