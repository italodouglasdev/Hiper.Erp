using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Dtos.Vendas;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Vendas;
using Hiper.Erp.Dominio.Entidades.Vendas;

namespace Hiper.Erp.Infraestrutura.Repositorios.API.Vendas
{
    public class RepositorioVendasItensApi : RespositorioBase<EntidadeVendaItem>, IRepositorioVendasItens
    {
        private readonly string endpoint = "/api/VendasItens";
        private readonly RepositorioApi repositorioApi;

        public RepositorioVendasItensApi(HttpClient httpCliente, IMapper mapeador) : base(httpCliente, mapeador)
        {
            repositorioApi = new RepositorioApi(httpCliente, mapeador);
        }

        public async Task<ResultadoOperacao<EntidadeVendaItem>> ObtenhaPorCodigoAsync(int codigo)
        {
            return await repositorioApi.GetAsync<EntidadeVendaItem, DtoVendaItem>($"{endpoint}/ObtenhaPorCodigo/{codigo}");
        }

        public async Task<ResultadoOperacao<List<EntidadeVendaItem>>> ObtenhaListaAsync()
        {
            return await repositorioApi.GetAsync<List<EntidadeVendaItem>, List<DtoVendaItem>>($"{endpoint}/ObtenhaLista");
        }

        public async Task<ResultadoOperacao<List<EntidadeVendaItem>>> ObtenhaListaComFiltrosAsync(DtoFiltro filtro)
        {
            return await repositorioApi.PostAsync<List<EntidadeVendaItem>, List<DtoVendaItem>>($"{endpoint}/ObtenhaListaComFiltros", filtro);
        }

        public async Task<ResultadoOperacao<EntidadeVendaItem>> CadastrarAsync(EntidadeVendaItem entidade)
        {
            return await repositorioApi.PostAsync<EntidadeVendaItem, DtoVendaItem>($"{endpoint}/Cadastrar", entidade);
        }

        public async Task<ResultadoOperacao<EntidadeVendaItem>> AtualizarAsync(EntidadeVendaItem entidade)
        {
            return await repositorioApi.PutAsync<EntidadeVendaItem, DtoVendaItem>($"{endpoint}/Atualizar", entidade);
        }

        public async Task<ResultadoOperacao<bool>> DeletarAsync(int codigo)
        {
            return await repositorioApi.DeleteAsync($"{endpoint}/Deletar/{codigo}");
        }
    }
}
