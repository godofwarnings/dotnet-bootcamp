using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApplication12.Middlewares
{
    public class HeaderValidationMiddleware
    {
        private readonly RequestDelegate next;

        public HeaderValidationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("X-Client-Name"))
            {
                context.Response.StatusCode = 400;

                await context.Response.WriteAsync("Missing required header: X-Client-Name");

                return;
            }

            await next(context);
        }
    }
}
