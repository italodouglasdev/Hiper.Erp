using Hiper.Erp.Aplicacao.Dtos.ServicosExternos;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiper.Erp.Aplicacao.Servicos.ServicosExternos
{
    public class ServicoHiperAdm : IServicoAdministrador
    {
        public Task<ResultadoOperacao<ConfiguracaoTenantDto>> ObtenhaConfiguracaoTenant(string tenantId)
        {

            throw new NotImplementedException();


        }
    }
}
