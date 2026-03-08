using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Dtos.Vendas;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Vendas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hiper.Erp.Apresentacao.Api.Controllers.Vendas
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VendasController : Controller
    {
        private IServicoVendas servicoVendas;

        public VendasController(IServicoVendas servicoVendas)
        {
            this.servicoVendas = servicoVendas;
        }

        [HttpGet("ObtenhaPorCodigo/{codigo}")]
        [ProducesResponseType(typeof(DtoVenda), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenhaPorCodigo(int codigo)
        {
            var resultadoServico = await servicoVendas.ObtenhaPorCodigo(codigo);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoVenda>.NotFound(resultadoServico));
            return Ok(ResponseHttp<DtoVenda>.Ok(resultadoServico));
        }

        [HttpGet("ObtenhaLista")]
        [ProducesResponseType(typeof(List<DtoVenda>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenhaLista()
        {
            var resultadoServico = await servicoVendas.ObtenhaLista();
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<List<DtoVenda>>.NotFound(resultadoServico));
            return Ok(ResponseHttp<List<DtoVenda>>.Ok(resultadoServico));
        }

        [HttpPost("ObtenhaListaComFiltros")]
        [ProducesResponseType(typeof(List<DtoVenda>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenhaListaComFiltros(DtoFiltro dtoFiltro)
        {
            var resultadoServico = await servicoVendas.ObtenhaListaComFiltros(dtoFiltro);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<List<DtoVenda>>.NotFound(resultadoServico));
            return Ok(ResponseHttp<List<DtoVenda>>.Ok(resultadoServico));
        }

        [HttpPost("Cadastrar")]
        [ProducesResponseType(typeof(DtoVenda), StatusCodes.Status200OK)]
        public async Task<IActionResult> Cadastrar(DtoVenda dto)
        {
            var resultadoServico = await servicoVendas.Cadastrar(dto);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoVenda>.NotFound(resultadoServico));
            return Ok(ResponseHttp<DtoVenda>.Ok(resultadoServico));
        }

        [HttpPut("Atualizar")]
        [ProducesResponseType(typeof(DtoVenda), StatusCodes.Status200OK)]
        public async Task<IActionResult> Atualizar(DtoVenda dto)
        {
            var resultadoServico = await servicoVendas.Atualizar(dto);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoVenda>.NotFound(resultadoServico));
            return Ok(ResponseHttp<DtoVenda>.Ok(resultadoServico));
        }

        [HttpDelete("Deletar/{codigo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Deletar(int codigo)
        {
            var resultadoServico = await servicoVendas.Deletar(codigo);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<bool>.NotFound(resultadoServico));
            return Ok(ResponseHttp<bool>.Ok(resultadoServico));
        }
    }
}
