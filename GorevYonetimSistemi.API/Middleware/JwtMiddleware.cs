using System.Net;
using GorevYonetimSistemi.Business.Services.Interfaces;

namespace GorevYonetimSistemi.API.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ITokenService tokenService)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                try
                {
                    var validatedToken = tokenService.ValidateToken(token);

                    if (validatedToken == null)
                    {
                        httpContext.Response.StatusCode = 401;
                        await httpContext.Response.WriteAsync("Invalid or expired token.");
                        return;
                    }
                }
                catch (Exception)
                {
                    httpContext.Response.StatusCode = 401; // Unauthorized
                    await httpContext.Response.WriteAsync("Invalid or expired token.");
                    return;
                }
            }

            await _next(httpContext);
        }
    }
}