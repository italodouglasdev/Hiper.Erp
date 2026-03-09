using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Dtos.ServicosExternos;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos;
using Hiper.Erp.Dominio.Enumeradores;
using Microsoft.Extensions.Configuration;

namespace Hiper.Erp.Aplicacao.Servicos.ServicosExternos
{
    public class ServicoAdministradorMock : IServicoAdministrador
    {
        private readonly IConfiguration _configuration;

        public ServicoAdministradorMock(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<ResultadoOperacao<UsuarioLogadoDto>> Login(string email, string senha)
        {
            throw new NotImplementedException();
        }

        public Task<ResultadoOperacao<List<LojaDto>>> Lojas()
        {
            throw new NotImplementedException();
        }

        public async Task<ResultadoOperacao<ConfiguracaoTenantDto>> ObtenhaConfiguracaoTenant(string tenantId)
        {
            await Task.Delay(50); // Simula latência

            if (string.IsNullOrEmpty(tenantId))
                return ResultadoOperacao<ConfiguracaoTenantDto>.Falha("TenantId não fornecido.");

            var connectionStringSqlServer = _configuration["ConnectionStringsMock:SqlServer"]
                ?? "Data Source=localhost;Initial Catalog=HiperErp_Loja_0001;User ID=SA;Password=ALTERAR;TrustServerCertificate=True;";

            var connectionStringPostgreSql = _configuration["ConnectionStringsMock:PostgreSQL"]
                ?? "Host=localhost;Database=HiperErp_Loja_0002;User ID=postgres;Password=ALTERAR;SSL Mode=Disable;";

            var config = new ConfiguracaoTenantDto
            {
                ConnectionString = tenantId == "cliente_pg"
                    ? connectionStringPostgreSql
                    : connectionStringSqlServer,
                TipoSgdb = tenantId == "cliente_pg" ? EnumTipoSgdb.PostgreSQL : EnumTipoSgdb.SQLServer
            };

            return ResultadoOperacao<ConfiguracaoTenantDto>.Ok(config);
        }
    }
}
