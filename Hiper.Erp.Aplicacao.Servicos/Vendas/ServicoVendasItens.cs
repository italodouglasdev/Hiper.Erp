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
    public class ServicoVendasItens : ServicoBase, IServicoVendasItens
    {

        #region Propriedades

        private IRepositorioVendasItens repVendaItem;

        #endregion

        #region Construtores

        public ServicoVendasItens(IMapper mapper, IRepositorioVendasItens repVendaItem) : base(mapper)
        {
            this.repVendaItem = repVendaItem;
        }

        #endregion

        #region Métodos Públicos

        public async Task<ResultadoOperacao<DtoVendaItem>> ObtenhaPorCodigo(int codigo)
        {
            var resultado = new ResultadoOperacao<DtoVendaItem>();

            try
            {
                var repResultado = await repVendaItem.ObtenhaPorCodigoAsync(codigo);
                resultado.Dados = mapeador.Map<DtoVendaItem>(repResultado.Dados);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<List<DtoVendaItem>>> ObtenhaLista()
        {
            var resultado = new ResultadoOperacao<List<DtoVendaItem>>();

            try
            {
                var repResultado = await repVendaItem.ObtenhaListaAsync();
                resultado.Dados = mapeador.Map<List<DtoVendaItem>>(repResultado.Dados);
                resultado.AdicionarPaginacao(1, repResultado.Dados.Count, 100);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<List<DtoVendaItem>>> ObtenhaListaComFiltros(DtoFiltro filtro)
        {
            var resultado = new ResultadoOperacao<List<DtoVendaItem>>();

            try
            {
                var repResultado = await repVendaItem.ObtenhaListaComFiltrosAsync(filtro);
                resultado.Dados = mapeador.Map<List<DtoVendaItem>>(repResultado.Dados);
                resultado.AdicionarPaginacao(1, repResultado.Dados?.Count ?? 0, 100);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<DtoVendaItem>> Cadastrar(DtoVendaItem dtoVendaItem)
        {
            var resultado = new ResultadoOperacao<DtoVendaItem>();

            try
            {

                var validacaoDto = DtoVendaItemValidador.Cadastrar(dtoVendaItem);
                if (!validacaoDto.Sucesso)
                {
                    resultado.AdicionarErros(validacaoDto.Erros);
                    return resultado;
                }

                var entidade = mapeador.Map<EntidadeVendaItem>(dtoVendaItem);
                var repResultado = await repVendaItem.CadastrarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoVendaItem>(repResultado.Dados);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<DtoVendaItem>> Atualizar(DtoVendaItem dtoVendaItem)
        {
            var resultado = new ResultadoOperacao<DtoVendaItem>();

            try
            {
                var validacaoDto = DtoVendaItemValidador.Atualizar(dtoVendaItem);
                if (!validacaoDto.Sucesso)
                {
                    resultado.AdicionarErros(validacaoDto.Erros);
                    return resultado;
                }

                var entidade = mapeador.Map<EntidadeVendaItem>(dtoVendaItem);
                var repResultado = await repVendaItem.AtualizarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoVendaItem>(repResultado.Dados);
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
                var repResultado = await repVendaItem.DeletarAsync(codigo);
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
