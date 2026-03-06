using Hiper.Erp.Aplicacao.Dtos.ServicosExternos;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos;
using Hiper.Erp.Infraestrutura.Cache;

namespace Hiper.Erp.Apresentacao.Api.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext, ICacheService cacheService, IServicoAdministrador servicoAdmin)
        {
            // 1. Tenta obter o TenantId do Header
            if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantId))
            {
                tenantContext.TenantId = tenantId;

                // 2. Tenta obter a configuração do Cache
                var cacheKey = $"tenant_config_{tenantId}";
                var config = cacheService.Get<ConfiguracaoTenantDto>(cacheKey);

                if (config == null)
                {
                    // 3. Se não estiver no cache, consulta o Administrador (Mockado)
                    var resultado = await servicoAdmin.ObtenhaConfiguracaoTenant(tenantId);

                    if (resultado.Sucesso && resultado.Dados != null)
                    {
                        config = resultado.Dados;
                        // Grava no cache por 1 hora
                        cacheService.Set(cacheKey, config, TimeSpan.FromHours(1));
                    }
                }

                // 4. Preenche o contexto da requisição atual
                if (config != null)
                {
                    tenantContext.ConnectionString = config.ConnectionString;
                    tenantContext.TipoSgdb = config.TipoSgdb;
                }
            }

            await _next(context);
        }
    }
}
