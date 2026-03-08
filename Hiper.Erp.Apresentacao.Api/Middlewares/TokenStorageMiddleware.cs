using System.Net.Http.Headers;

namespace Hiper.Erp.Apresentacao.Api.Middlewares
{
    public class TokenStorageMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenStorageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Extrai o token do header Authorization
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                // Armazena o token para ser usado pelo HttpClient
                TokenStorage.JwtToken = token;
            }

            await _next(context);
        }
    }

    // Classe para armazenar o token
    public static class TokenStorage
    {
        public static string? JwtToken { get; set; }
    }
}
