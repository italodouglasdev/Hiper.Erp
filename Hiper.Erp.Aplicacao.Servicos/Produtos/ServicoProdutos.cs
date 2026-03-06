using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.Filtros;
using Hiper.Erp.Aplicacao.Dtos.Produtos;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Produtos;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Produtos;
using Hiper.Erp.Dominio.Entidades.Produtos;

namespace Hiper.Erp.Aplicacao.Servicos.Produtos
{
    public class ServicoProdutos : ServicoBase, IServicoProdutos
    {
        private IRepositorioProdutos repProduto;

        public ServicoProdutos(IMapper mapper, IRepositorioProdutos repProduto) : base(mapper)
        {
            this.repProduto = repProduto;
        }

        public async Task<ResultadoOperacao<DtoProduto>> ObtenhaPorCodigo(int codigo)
        {
            var resultado = new ResultadoOperacao<DtoProduto>();
            try
            {
                var repResultado = await repProduto.ObtenhaPorCodigoAsync(codigo);
                resultado.Dados = mapeador.Map<DtoProduto>(repResultado.Dados);
            }
            catch (Exception)
            {
                resultado.AdicionarErro("Erro inesperado.");
            }
            return resultado;
        }

        public async Task<ResultadoOperacao<List<DtoProduto>>> ObtenhaLista()
        {
            var resultado = new ResultadoOperacao<List<DtoProduto>>();
            try
            {
                var repResultado = await repProduto.ObtenhaListaAsync();
                resultado.Dados = mapeador.Map<List<DtoProduto>>(repResultado.Dados);
                resultado.AdicionarPaginacao(1, repResultado.Dados.Count, 100);
            }
            catch (Exception)
            {
                resultado.AdicionarErro("Erro inesperado.");
            }
            return resultado;
        }

        public async Task<ResultadoOperacao<List<DtoProduto>>> ObtenhaListaComFiltros(DtoFiltro filtro)
        {
            var resultado = new ResultadoOperacao<List<DtoProduto>>();
            try
            {
                var repResultado = await repProduto.ObtenhaListaComFiltrosAsync(filtro);
                resultado.Dados = mapeador.Map<List<DtoProduto>>(repResultado.Dados);
                resultado.AdicionarPaginacao(1, repResultado.Dados?.Count ?? 0, 100);
            }
            catch (Exception)
            {
                resultado.AdicionarErro("Erro inesperado.");
            }
            return resultado;
        }

        public async Task<ResultadoOperacao<DtoProduto>> Cadastrar(DtoProduto dtoProduto)
        {
            var resultado = new ResultadoOperacao<DtoProduto>();
            try
            {
                var entidade = mapeador.Map<EntidadeProduto>(dtoProduto);
                var repResultado = await repProduto.CadastrarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoProduto>(repResultado.Dados);
            }
            catch (Exception)
            {
                resultado.AdicionarErro("Erro interno.");
            }
            return resultado;
        }

        public async Task<ResultadoOperacao<DtoProduto>> Atualizar(DtoProduto dtoProduto)
        {
            var resultado = new ResultadoOperacao<DtoProduto>();
            try
            {
                var entidade = mapeador.Map<EntidadeProduto>(dtoProduto);
                var repResultado = await repProduto.AtualizarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoProduto>(repResultado.Dados);
            }
            catch (Exception)
            {
                resultado.AdicionarErro("Erro inesperado ao atualizar.");
            }
            return resultado;
        }

        public async Task<ResultadoOperacao<bool>> Deletar(int codigo)
        {
            var resultado = new ResultadoOperacao<bool>();
            try
            {
                var repResultado = await repProduto.DeletarAsync(codigo);
                resultado.Dados = repResultado.Dados;
            }
            catch (Exception)
            {
                resultado.AdicionarErro("Erro inesperado ao deletar.");
            }
            return resultado;
        }
    }
}
