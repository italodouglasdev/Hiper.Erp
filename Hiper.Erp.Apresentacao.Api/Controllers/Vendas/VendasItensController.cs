using Hiper.Erp.Aplicacao.Dtos.Filtros;
using Hiper.Erp.Aplicacao.Dtos.Vendas;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Vendas;
using Microsoft.AspNetCore.Mvc;

namespace Hiper.Erp.Apresentacao.Api.Controllers.Vendas
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendasItensController : Controller
    {
        private IServicoVendasItens servicoVendasItens;

        public VendasItensController(IServicoVendasItens servicoVendasItens)
        {
            this.servicoVendasItens = servicoVendasItens;
        }

        [HttpGet("ObtenhaPorCodigo/{codigo}")]
        [ProducesResponseType(typeof(DtoVendaItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenhaPorCodigo(int codigo)
        {
            var resultadoServico = await servicoVendasItens.ObtenhaPorCodigo(codigo);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoVendaItem>.NotFound(resultadoServico));
            return Ok(ResponseHttp<DtoVendaItem>.Ok(resultadoServico));
        }

        [HttpGet("ObtenhaLista")]
        [ProducesResponseType(typeof(List<DtoVendaItem>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenhaLista()
        {
            var resultadoServico = await servicoVendasItens.ObtenhaLista();
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<List<DtoVendaItem>>.NotFound(resultadoServico));
            return Ok(ResponseHttp<List<DtoVendaItem>>.Ok(resultadoServico));
        }

        [HttpPost("ObtenhaListaComFiltros")]
        [ProducesResponseType(typeof(List<DtoVendaItem>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenhaListaComFiltros(DtoFiltro dtoFiltro)
        {
            var resultadoServico = await servicoVendasItens.ObtenhaListaComFiltros(dtoFiltro);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<List<DtoVendaItem>>.NotFound(resultadoServico));
            return Ok(ResponseHttp<List<DtoVendaItem>>.Ok(resultadoServico));
        }

        [HttpPost("Cadastrar")]
        [ProducesResponseType(typeof(DtoVendaItem), StatusCodes.Status200OK)]
        public async Task<IActionResult> Cadastrar(DtoVendaItem dto)
        {
            var resultadoServico = await servicoVendasItens.Cadastrar(dto);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoVendaItem>.NotFound(resultadoServico));
            return Ok(ResponseHttp<DtoVendaItem>.Ok(resultadoServico));
        }

        [HttpPut("Atualizar")]
        [ProducesResponseType(typeof(DtoVendaItem), StatusCodes.Status200OK)]
        public async Task<IActionResult> Atualizar(DtoVendaItem dto)
        {
            var resultadoServico = await servicoVendasItens.Atualizar(dto);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoVendaItem>.NotFound(resultadoServico));
            return Ok(ResponseHttp<DtoVendaItem>.Ok(resultadoServico));
        }

        [HttpDelete("Deletar/{codigo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Deletar(int codigo)
        {
            var resultadoServico = await servicoVendasItens.Deletar(codigo);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<bool>.NotFound(resultadoServico));
            return Ok(ResponseHttp<bool>.Ok(resultadoServico));
        }
    }
}
