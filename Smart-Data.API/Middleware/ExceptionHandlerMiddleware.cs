using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Smart_Data.Application.Exceptions;
using Smart_Data.Application.Responses;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace one_access.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        /// <param name="env"></param>
        public ExceptionHandlerMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ConvertException(context, ex);
            }

        }

        private Task ConvertException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiException();

            HttpStatusCode httpStatusCode;
            switch (ex)
            {
                case ValidationException validationException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    response.ValidationErrors = validationException.ValidationErrors;
                    break;
                default:
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    break;
            }
            context.Response.StatusCode = (int)httpStatusCode;

            if (httpStatusCode == HttpStatusCode.InternalServerError)
            {               
                response.Message = _env.IsDevelopment() ? ex.Message : "INTERNAL SERVER ERROR";
            }
            else
            {
                response.Message = ex.Message;
            }
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);
            return context.Response.WriteAsync(json);
        }
    }
}
