using System.Net.Http.Headers;
using Hiper.Erp.InterfaceUsuarios.RetaguardaWeb.Servicos;

namespace Hiper.Erp.Apresentacao.Web.Handlers
{
    public class TenantHandler : DelegatingHandler
    {
        private readonly StorageServico _storage;

        public TenantHandler(StorageServico storage)
        {
            _storage = storage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _storage.GetItem<string>("auth_token");
            var tenantId = await _storage.GetItem<string>("tenant_id");

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("XTenantId", tenantId);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}