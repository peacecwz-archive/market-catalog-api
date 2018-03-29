using AktuelListesi.Models.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AktuelListesi.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this._next.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var response = new ActionResultModel();
                response.Messages.Add(ex.Message);
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    response.Messages.Add(innerException.Message);
                    innerException = innerException.InnerException;
                }
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}
