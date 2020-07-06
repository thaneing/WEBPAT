using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Middlewares
{
    public class SerialMiddleware
    {
        readonly RequestDelegate _next;
        public SerialMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (HardwareInfoMiddleware.PutRegitry() != null)
            {
                var url = "/Home/Serial";
                //check if the current url contains the path of the installation url
                if (context.Request.Path.Value.IndexOf(url, StringComparison.CurrentCultureIgnoreCase) == -1)
                {
                    context.Response.Redirect(url);
                    return;
                }
            }
            await _next(context);
        }
    }

}

