using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Middlewares
{
    public class SetHeaderMidleware
    {
        private readonly RequestDelegate _next;
        public SetHeaderMidleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();
            context.Response.OnStarting(state => {
                var httpContext = (HttpContext)state;
                httpContext.Response.Headers.Add("X-Response-Time-Milliseconds", new[] { watch.ElapsedMilliseconds.ToString() });

                return Task.CompletedTask;
            }, context);

            await _next(context);
        }
    }
}
