using System.Net.Http.Headers;

namespace Hiper.Erp.Apresentacao.Web.Handlers
{
    public class TenantHandler : DelegatingHandler
    {    
        public static string? JwtToken { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(JwtToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
