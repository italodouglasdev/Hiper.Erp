using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Dtos.Produtos;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Produtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hiper.Erp.Apresentacao.Api.Controllers.Produtos
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : Controller
    {
        private IServicoProdutos servicoProdutos;

        public ProdutosController(IServicoProdutos servicoProdutos)
        {
            this.servicoProdutos = servicoProdutos;
        }

        [HttpGet("ObtenhaPorCodigo/{codigo}")]
        [ProducesResponseType(typeof(DtoProduto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenhaPorCodigo(int codigo)
        {
            var resultadoServico = await servicoProdutos.ObtenhaPorCodigo(codigo);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoProduto>.NotFound(resultadoServico));
            return Ok(ResponseHttp<DtoProduto>.Ok(resultadoServico));
        }

        [HttpGet("ObtenhaLista")]
        [ProducesResponseType(typeof(List<DtoProduto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenhaLista()
        {
            var resultadoServico = await servicoProdutos.ObtenhaLista();
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<List<DtoProduto>>.NotFound(resultadoServico));
            return Ok(ResponseHttp<List<DtoProduto>>.Ok(resultadoServico));
        }

        [HttpPost("ObtenhaListaComFiltros")]
        [ProducesResponseType(typeof(List<DtoProduto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenhaListaComFiltros(DtoFiltro dtoFiltro)
        {
            var resultadoServico = await servicoProdutos.ObtenhaListaComFiltros(dtoFiltro);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<List<DtoProduto>>.NotFound(resultadoServico));
            return Ok(ResponseHttp<List<DtoProduto>>.Ok(resultadoServico));
        }

        [HttpPost("Cadastrar")]
        [ProducesResponseType(typeof(DtoProduto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Cadastrar(DtoProduto dto)
        {
            var resultadoServico = await servicoProdutos.Cadastrar(dto);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoProduto>.NotFound(resultadoServico));
            return Ok(ResponseHttp<DtoProduto>.Ok(resultadoServico));
        }

        [HttpPut("Atualizar")]
        [ProducesResponseType(typeof(DtoProduto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Atualizar(DtoProduto dto)
        {
            var resultadoServico = await servicoProdutos.Atualizar(dto);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<DtoProduto>.NotFound(resultadoServico));
            return Ok(ResponseHttp<DtoProduto>.Ok(resultadoServico));
        }

        [HttpDelete("Deletar/{codigo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Deletar(int codigo)
        {
            var resultadoServico = await servicoProdutos.Deletar(codigo);
            if (!resultadoServico.Sucesso)
                return NotFound(ResponseHttp<bool>.NotFound(resultadoServico));
            return Ok(ResponseHttp<bool>.Ok(resultadoServico));
        }
    }
}
