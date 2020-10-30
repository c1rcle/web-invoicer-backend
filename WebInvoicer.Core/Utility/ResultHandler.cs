using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace WebInvoicer.Core.Utility
{
    public class ResultHandler
    {
        private HttpStatusCode StatusCode { get; }

        private object Payload { get; set; }

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

        public void MapPayload<TSource, TDestination>(IMapper mapper)
        {
            Payload = Payload switch
            {
                IEnumerable<TSource> data => data.Select(x => mapper.Map<TDestination>(x)),
                _ => mapper.Map<TDestination>(Payload)
            };
        }

        public static ResultHandler HandleTaskResult<T>(TaskResult<T> result)
        {
            if (result.Success)
            {
                return new ResultHandler(HttpStatusCode.OK, result.Payload);
            }

            var statusCode = GetStatusCodeForErrorType(result.ErrorType);
            return result.Errors == null
                ? new ResultHandler(statusCode)
                : new ResultHandler(statusCode, result.Errors);
        }

        public static ResultHandler HandleTaskResult(TaskResult result)
        {
            if (result.Success)
            {
                return new ResultHandler(HttpStatusCode.NoContent);
            }

            var statusCode = GetStatusCodeForErrorType(result.ErrorType);
            return result.Errors == null
                ? new ResultHandler(statusCode)
                : new ResultHandler(statusCode, result.Errors);
        }

        private static HttpStatusCode GetStatusCodeForErrorType(TaskErrorType type)
        {
            return type switch
            {
                TaskErrorType.NotFound => HttpStatusCode.NotFound,
                TaskErrorType.Unauthorized => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.UnprocessableEntity
            };
        }
    }
}
