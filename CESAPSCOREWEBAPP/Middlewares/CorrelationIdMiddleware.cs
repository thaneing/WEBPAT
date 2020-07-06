using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;


        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.OnStarting((Func<Task>)(() =>
            {
                httpContext.Response.Headers.Add("X-Correlation-Id", Guid.NewGuid().ToString());
                return Task.CompletedTask;
            }));
            try
            {
                await _next(httpContext);

            }
            catch (Exception)
            {

                httpContext.Response.StatusCode = 500;
            }

        }
    }
}
