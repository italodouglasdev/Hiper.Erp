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
    public class AuthController : ControllerBase
    {
        private readonly IServicoAdministrador _servicoAdmin;

        public AuthController(IServicoAdministrador servicoAdmin)
        {
            _servicoAdmin = servicoAdmin;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var resultado = await _servicoAdmin.Login(loginDto.Email, loginDto.Password);

            if (!resultado.Sucesso)
                return Unauthorized(ResponseHttp<object>.Unauthorized(resultado.Mensagem));

            return Ok(ResponseHttp<UsuarioLogadoDto>.Ok(resultado.Dados, "Login realizado com sucesso."));
        }

        [HttpGet("Lojas")]
        public async Task<IActionResult> Lojas()
        {
            var resultado = await _servicoAdmin.Lojas();

            if (!resultado.Sucesso)
                return BadRequest(ResponseHttp<object>.BadRequest(resultado.Mensagem));

            return Ok(ResponseHttp<List<LojaDto>>.Ok(resultado.Dados));
        }
    }
}
