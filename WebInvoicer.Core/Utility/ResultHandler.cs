using System.Net;
using AutoMapper;
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

        public static ResultHandler HandleTaskResult(TaskResult result)
        {
            var statusCode = result switch
            {
                { Success: true, Payload: null } => HttpStatusCode.NoContent,
                { Success: true, Payload: not null } => HttpStatusCode.OK,
                _ => HttpStatusCode.UnprocessableEntity
            };

            return statusCode == HttpStatusCode.OK
                ? new ResultHandler(statusCode, result)
                : new ResultHandler(statusCode);
        }
    }
}
