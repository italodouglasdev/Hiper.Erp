using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.Filtros;
using Hiper.Erp.Aplicacao.Dtos.Vendas;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Vendas;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Vendas;
using Hiper.Erp.Dominio.Entidades.Vendas;

namespace Hiper.Erp.Aplicacao.Servicos.Vendas
{
    public class ServicoVendasItens : ServicoBase, IServicoVendasItens
    {
        private IRepositorioVendasItens repVendaItem;

        public ServicoVendasItens(IMapper mapper, IRepositorioVendasItens repVendaItem) : base(mapper)
        {
            this.repVendaItem = repVendaItem;
        }

        public async Task<ResultadoOperacao<DtoVendaItem>> ObtenhaPorCodigo(int codigo)
        {
            var resultado = new ResultadoOperacao<DtoVendaItem>();
            try
            {
                var repResultado = await repVendaItem.ObtenhaPorCodigoAsync(codigo);
                resultado.Dados = mapeador.Map<DtoVendaItem>(repResultado.Dados);
            }
            catch (Exception)
            {
                resultado.AdicionarErro("Erro inesperado.");
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
            catch (Exception)
            {
                resultado.AdicionarErro("Erro inesperado.");
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
            catch (Exception)
            {
                resultado.AdicionarErro("Erro inesperado.");
            }
            return resultado;
        }

        public async Task<ResultadoOperacao<DtoVendaItem>> Cadastrar(DtoVendaItem dtoVendaItem)
        {
            var resultado = new ResultadoOperacao<DtoVendaItem>();
            try
            {
                var entidade = mapeador.Map<EntidadeVendaItem>(dtoVendaItem);
                var repResultado = await repVendaItem.CadastrarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoVendaItem>(repResultado.Dados);
            }
            catch (Exception)
            {
                resultado.AdicionarErro("Erro interno.");
            }
            return resultado;
        }

        public async Task<ResultadoOperacao<DtoVendaItem>> Atualizar(DtoVendaItem dtoVendaItem)
        {
            var resultado = new ResultadoOperacao<DtoVendaItem>();
            try
            {
                var entidade = mapeador.Map<EntidadeVendaItem>(dtoVendaItem);
                var repResultado = await repVendaItem.AtualizarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoVendaItem>(repResultado.Dados);
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
                var repResultado = await repVendaItem.DeletarAsync(codigo);
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
