using Hiper.Erp.Aplicacao.Dtos.Agentes;
using Hiper.Erp.Aplicacao.Dtos.Mensageria;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Agentes;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Mensageria;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hiper.Erp.Apresentacao.Api.Controllers.Agentes
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AgentesController : Controller
    {

        private IServicoAgentes servicoAgentes;
        private readonly IMensageriaServico _mensageriaServico;

        public AgentesController(IServicoAgentes servicoAgentes, IMensageriaServico mensageriaServico)
        {
            this.servicoAgentes = servicoAgentes;
            _mensageriaServico = mensageriaServico;
        }


        [HttpGet("ObtenhaPorCodigo/{codigo}")]
        [ProducesResponseType(typeof(DtoAgente), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenhaPorCodigo(int codigo)
        {
            var resultadoServico = await servicoAgentes.ObtenhaPorCodigo(codigo);

            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoAgente>.NotFound(resultadoServico));

            return Ok(ResponseHttp<DtoAgente>.Ok(resultadoServico));
        }

        [HttpGet("ObtenhaPorCnpjCpf/{CnpjCpf}")]
        [ProducesResponseType(typeof(DtoAgente), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenhaPorCnpjCpf(string CnpjCpf)
        {
            var resultadoServico = await servicoAgentes.ObtenhaPorCnpjCpf(CnpjCpf);

            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoAgente>.NotFound(resultadoServico));

            return Ok(ResponseHttp<DtoAgente>.Ok(resultadoServico));
        }



        [HttpGet("ObtenhaLista")]
        [ProducesResponseType(typeof(List<DtoAgente>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenhaLista()
        {
            var resultadoServico = await servicoAgentes.ObtenhaLista();

            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<List<DtoAgente>>.NotFound(resultadoServico));

            return Ok(ResponseHttp<List<DtoAgente>>.Ok(resultadoServico));

        }

        [HttpPost("ObtenhaListaComFiltros")]
        [ProducesResponseType(typeof(List<DtoAgente>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenhaListaComFiltros(DtoFiltro dtoFiltro)
        {
            var resultadoServico = await servicoAgentes.ObtenhaListaComFiltros(dtoFiltro);

            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<List<DtoAgente>>.NotFound(resultadoServico));

            return Ok(ResponseHttp<List<DtoAgente>>.Ok(resultadoServico));

        }



        [HttpPost("Cadastrar")]
        [ProducesResponseType(typeof(DtoAgente), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Cadastrar(DtoAgente dto)
        {
            var resultadoServico = await servicoAgentes.Cadastrar(dto);

            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoAgente>.NotFound(resultadoServico));
    
            await _mensageriaServico.PublicarMensagemAsync(
                "fila-email-boas-vindas",
                new MensagemBoasVindasDto
                {
                    CodigoAgente = resultadoServico.Dados.Codigo,
                    RazaoNome = resultadoServico.Dados.RazaoNome,
                    FantasiaApelido = resultadoServico.Dados.FantasiaApelido,
                    CnpjCpf = resultadoServico.Dados.CnpjCpf,
                    Tipo = "BoasVindas",
                    DataCriacao = DateTime.UtcNow,
                    Descricao = $"Novo cliente '{resultadoServico.Dados.RazaoNome}' cadastrado. Enviar e-mail de boas-vindas."
                }
            );

            return Ok(ResponseHttp<DtoAgente>.Ok(resultadoServico));
        }



        [HttpPut("Atualizar")]
        [ProducesResponseType(typeof(DtoAgente), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(DtoAgente dto)
        {
            var resultadoServico = await servicoAgentes.Atualizar(dto);

            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoAgente>.NotFound(resultadoServico));

            return Ok(ResponseHttp<DtoAgente>.Ok(resultadoServico));
        }



        [HttpDelete("Deletar/{codigo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deletar(int codigo)
        {
            var resultadoServico = await servicoAgentes.Deletar(codigo);

            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<bool>.NotFound(resultadoServico));

            return Ok(ResponseHttp<bool>.Ok(resultadoServico));

        }

    }
}
