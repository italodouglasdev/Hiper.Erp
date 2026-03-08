using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Dtos.ServicosExternos;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hiper.Erp.Apresentacao.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly IServicoAdministrador _servicoAdmin;

        public TenantsController(IServicoAdministrador servicoAdmin)
        {
            _servicoAdmin = servicoAdmin;
        }

        [HttpGet("ObtenhaConfiguracaoTenantPorXTenantId/{xTenantId}")]
        public async Task<IActionResult> ObtenhaConfiguracaoTenantPorXTenantId(string xTenantId)
        {
            var resultado = await _servicoAdmin.ObtenhaConfiguracaoTenant(xTenantId);
            if (!resultado.Sucesso)
                return NotFound(ResponseHttp<object>.NotFound(resultado.Mensagem));

            return Ok(ResponseHttp<ConfiguracaoTenantDto>.Ok(resultado.Dados));
        }
    }
}
