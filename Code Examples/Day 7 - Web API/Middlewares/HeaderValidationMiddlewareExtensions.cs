using Microsoft.AspNetCore.Builder;

namespace WebApplication12.Middlewares
{
    public static class HeaderValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseHeaderValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HeaderValidationMiddleware>();
        }
    }
}
