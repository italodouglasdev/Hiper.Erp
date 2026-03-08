using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Dtos.Produtos;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Produtos;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Produtos;
using Hiper.Erp.Aplicacao.Validadores.Produtos;
using Hiper.Erp.Dominio.Entidades.Produtos;

namespace Hiper.Erp.Aplicacao.Servicos.Produtos
{
    public class ServicoProdutos : ServicoBase, IServicoProdutos
    {

        #region Propriedades

        private IRepositorioProdutos repProduto;

        #endregion

        #region Construtores
        public ServicoProdutos(IMapper mapper, IRepositorioProdutos repProduto) : base(mapper)
        {
            this.repProduto = repProduto;
        }

        #endregion

        #region Métodos Públicos

        public async Task<ResultadoOperacao<DtoProduto>> ObtenhaPorCodigo(int codigo)
        {
            var resultado = new ResultadoOperacao<DtoProduto>();

            try
            {
                var repResultado = await repProduto.ObtenhaPorCodigoAsync(codigo);
                resultado.Dados = mapeador.Map<DtoProduto>(repResultado.Dados);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
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
                resultado.AdicionarPaginacao(1, repResultado?.Dados?.Count ?? 0, 100);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
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
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<DtoProduto>> Cadastrar(DtoProduto dtoProduto)
        {
            var resultado = new ResultadoOperacao<DtoProduto>();

            try
            {
                var validacaoDto = DtoProdutoValidador.Cadastrar(dtoProduto);
                if (!validacaoDto.Sucesso)
                {
                    resultado.AdicionarErros(validacaoDto.Erros);
                    return resultado;
                }

                var entidade = mapeador.Map<EntidadeProduto>(dtoProduto);
                var repResultado = await repProduto.CadastrarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoProduto>(repResultado.Dados);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<DtoProduto>> Atualizar(DtoProduto dtoProduto)
        {
            var resultado = new ResultadoOperacao<DtoProduto>();

            try
            {
                var validacaoDto = DtoProdutoValidador.Atualizar(dtoProduto);
                if (!validacaoDto.Sucesso)
                {
                    resultado.AdicionarErros(validacaoDto.Erros);
                    return resultado;
                }

                var entidade = mapeador.Map<EntidadeProduto>(dtoProduto);
                var repResultado = await repProduto.AtualizarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoProduto>(repResultado.Dados);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
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
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        #endregion

    }
}
