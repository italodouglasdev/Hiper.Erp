using System.Net.Http.Headers;

namespace Hiper.Erp.Apresentacao.Web.Handlers
{
    public class TenantHandler : DelegatingHandler
    {    
        public static string? TenantId { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(TenantId))
            {
                request.Headers.Add("X-Tenant-Id", TenantId);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
