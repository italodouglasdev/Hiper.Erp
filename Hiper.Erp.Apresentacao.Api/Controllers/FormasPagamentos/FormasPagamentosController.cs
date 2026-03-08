using Hiper.Erp.Aplicacao.Dtos.FormasPagamentos;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.FormasPagamentos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hiper.Erp.Apresentacao.Api.Controllers.FormasPagamentos
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FormasPagamentosController : Controller
    {
        private IServicoFormasPagamentos servicoFormasPagamentos;

        public FormasPagamentosController(IServicoFormasPagamentos servicoFormasPagamentos)
        {
            this.servicoFormasPagamentos = servicoFormasPagamentos;
        }

        [HttpGet("ObtenhaPorCodigo/{codigo}")]
        [ProducesResponseType(typeof(DtoFormaPagamento), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenhaPorCodigo(int codigo)
        {
            var resultadoServico = await servicoFormasPagamentos.ObtenhaPorCodigo(codigo);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoFormaPagamento>.NotFound(resultadoServico));
            return Ok(ResponseHttp<DtoFormaPagamento>.Ok(resultadoServico));
        }

        [HttpGet("ObtenhaLista")]
        [ProducesResponseType(typeof(List<DtoFormaPagamento>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenhaLista()
        {
            var resultadoServico = await servicoFormasPagamentos.ObtenhaLista();
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<List<DtoFormaPagamento>>.NotFound(resultadoServico));
            return Ok(ResponseHttp<List<DtoFormaPagamento>>.Ok(resultadoServico));
        }

        [HttpPost("ObtenhaListaComFiltros")]
        [ProducesResponseType(typeof(List<DtoFormaPagamento>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenhaListaComFiltros(DtoFiltro dtoFiltro)
        {
            var resultadoServico = await servicoFormasPagamentos.ObtenhaListaComFiltros(dtoFiltro);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<List<DtoFormaPagamento>>.NotFound(resultadoServico));
            return Ok(ResponseHttp<List<DtoFormaPagamento>>.Ok(resultadoServico));
        }

        [HttpPost("Cadastrar")]
        [ProducesResponseType(typeof(DtoFormaPagamento), StatusCodes.Status200OK)]
        public async Task<IActionResult> Cadastrar(DtoFormaPagamento dto)
        {
            var resultadoServico = await servicoFormasPagamentos.Cadastrar(dto);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoFormaPagamento>.NotFound(resultadoServico));
            return Ok(ResponseHttp<DtoFormaPagamento>.Ok(resultadoServico));
        }

        [HttpPut("Atualizar")]
        [ProducesResponseType(typeof(DtoFormaPagamento), StatusCodes.Status200OK)]
        public async Task<IActionResult> Atualizar(DtoFormaPagamento dto)
        {
            var resultadoServico = await servicoFormasPagamentos.Atualizar(dto);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoFormaPagamento>.NotFound(resultadoServico));
            return Ok(ResponseHttp<DtoFormaPagamento>.Ok(resultadoServico));
        }

        [HttpDelete("Deletar/{codigo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Deletar(int codigo)
        {
            var resultadoServico = await servicoFormasPagamentos.Deletar(codigo);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<bool>.NotFound(resultadoServico));
            return Ok(ResponseHttp<bool>.Ok(resultadoServico));
        }
    }
}
