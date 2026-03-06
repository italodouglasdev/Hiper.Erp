using Hiper.Erp.Aplicacao.Dtos.ServicosExternos;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos;
using Hiper.Erp.Dominio.Enumeradores;

namespace Hiper.Erp.Aplicacao.Servicos
{
    // Mock para simular o Servidor de Administração
    public class ServicoAdministradorMock : IServicoAdministrador
    {
        public async Task<ResultadoOperacao<ConfiguracaoTenantDto>> ObtenhaConfiguracaoTenant(string tenantId)
        {
            await Task.Delay(50); // Simula latência

            if (string.IsNullOrEmpty(tenantId))
                return ResultadoOperacao<ConfiguracaoTenantDto>.Falha("TenantId não fornecido.");

            // Mock de dados
            var config = new ConfiguracaoTenantDto
            {
                ConnectionString = tenantId == "cliente_pg"
                    ? "Host=localhost;Database=idsaaserpdb;User ID=postgres;Password=Y5hAmR9cJNKmGeY;SSL Mode=Disable;"
                    : "Data Source=localhost;Initial Catalog=idSaasErpDb; User ID=SA; Password=Y5hAmR9cJNKmGeY;TrustServerCertificate=True;",
                TipoSgdb = tenantId == "cliente_pg" ? EnumTipoSgdb.PostgreSQL : EnumTipoSgdb.SQLServer
            };

            return ResultadoOperacao<ConfiguracaoTenantDto>.Ok(config);
        }
    }
}
