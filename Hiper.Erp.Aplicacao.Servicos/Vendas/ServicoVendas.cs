using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Dtos.Vendas;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Vendas;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Vendas;
using Hiper.Erp.Aplicacao.Validadores.Vendas;
using Hiper.Erp.Dominio.Entidades.Vendas;

namespace Hiper.Erp.Aplicacao.Servicos.Vendas
{
    public class ServicoVendas : ServicoBase, IServicoVendas
    {

        #region Propriedades

        private IRepositorioVendas repVenda;
        private IRepositorioVendasItens repVendaItem;

        #endregion

        #region Construtores

        public ServicoVendas(
            IMapper mapper,
            IRepositorioVendas repVenda,
            IRepositorioVendasItens repVendaItem) : base(mapper)
        {
            this.repVenda = repVenda;
            this.repVendaItem = repVendaItem;
        }

        #endregion

        #region Métodos Públicos


        public async Task<ResultadoOperacao<DtoVenda>> ObtenhaPorCodigo(int codigo)
        {
            var resultado = new ResultadoOperacao<DtoVenda>();

            try
            {
                var repResultado = await repVenda.ObtenhaPorCodigoAsync(codigo);
                resultado.Dados = mapeador.Map<DtoVenda>(repResultado.Dados);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<List<DtoVenda>>> ObtenhaLista()
        {
            var resultado = new ResultadoOperacao<List<DtoVenda>>();

            try
            {
                var repResultado = await repVenda.ObtenhaListaAsync();
                resultado.Dados = mapeador.Map<List<DtoVenda>>(repResultado.Dados);
                resultado.AdicionarPaginacao(1, repResultado?.Dados?.Count ?? 0, 100);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<List<DtoVenda>>> ObtenhaListaComFiltros(DtoFiltro filtro)
        {
            var resultado = new ResultadoOperacao<List<DtoVenda>>();

            try
            {
                var repResultado = await repVenda.ObtenhaListaComFiltrosAsync(filtro);
                resultado.Dados = mapeador.Map<List<DtoVenda>>(repResultado.Dados);
                resultado.AdicionarPaginacao(1, repResultado.Dados?.Count ?? 0, 100);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<DtoVenda>> Cadastrar(DtoVenda dtoVenda)
        {
            var resultado = new ResultadoOperacao<DtoVenda>();

            try
            {
                var validacaoDto = DtoVendaValidador.Cadastrar(dtoVenda);
                if (!validacaoDto.Sucesso)
                {
                    resultado.AdicionarErros(validacaoDto.Erros);
                    return resultado;
                }

                var entidade = mapeador.Map<EntidadeVenda>(dtoVenda);
                var repResultado = await repVenda.CadastrarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoVenda>(repResultado.Dados);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<DtoVenda>> Atualizar(DtoVenda dtoVenda)
        {
            var resultado = new ResultadoOperacao<DtoVenda>();

            try
            {

                var validacaoDto = DtoVendaValidador.Atualizar(dtoVenda);
                if (!validacaoDto.Sucesso)
                {
                    resultado.AdicionarErros(validacaoDto.Erros);
                    return resultado;
                }

                var entidade = mapeador.Map<EntidadeVenda>(dtoVenda);
                var repResultado = await repVenda.AtualizarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoVenda>(repResultado.Dados);
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

                var validarSeVendaPossuiItens = await repVendaItem.ObtenhaListaAsync();
                if (validarSeVendaPossuiItens.Dados?.Count > 0)
                {
                    resultado.AdicionarErro("Não é possível deletar a venda, existem itens vinculadas.");
                    return resultado;
                }

                var repResultado = await repVenda.DeletarAsync(codigo);
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
