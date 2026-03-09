using Hiper.Adm.Utilitarios.CriptografiaHelper;
using Hiper.Erp.Aplicacao.Dtos.ServicosExternos;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos;
using Hiper.Erp.Infraestrutura.Cache;
using System.Security.Claims;

namespace Hiper.Erp.Apresentacao.Api.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public TenantMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext, ICacheService cacheService, IServicoAdministrador servicoAdmin)
        {
            // 1. Tenta obter o TenantId da Claim "X-Tenant-Id" dentro do JWT
            var tenantId = context.User.FindFirst("X-Tenant-Id")?.Value ?? context.Request.Headers["x-tenant-id"].FirstOrDefault();                       

            // 2. Fallback para o Header (útil para testes iniciais ou se não estiver no token)
            if (string.IsNullOrEmpty(tenantId) && context.Request.Headers.TryGetValue("X-Tenant-Id", out var headerTenantId))
            {
                tenantId = headerTenantId;
            }

            if (!string.IsNullOrEmpty(tenantId))
            {
                tenantContext.TenantId = tenantId;

                var cacheKey = $"tenant_config_{tenantId}";
                var config = cacheService.Get<ConfiguracaoTenantDto>(cacheKey);

                if (config == null)
                {
                    var resultado = await servicoAdmin.ObtenhaConfiguracaoTenant(tenantId);

                    if (resultado.Sucesso && resultado.Dados != null)
                    {
                        config = resultado.Dados;
                        cacheService.Set(cacheKey, config, TimeSpan.FromHours(1));
                    }
                }

                if (config != null)
                {
                    var chaveAes = _configuration["Seguranca:ChaveAesConnectionString"] ?? "ChavePadrao";
                    tenantContext.ConnectionString = AES.Decrypt(config.ConnectionString, chaveAes);
                    tenantContext.TipoSgdb = config.TipoSgdb;
                }
            }

            await _next(context);
        }
    }
}
