using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.Filtros;
using Hiper.Erp.Aplicacao.Dtos.Vendas;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Vendas;
using Hiper.Erp.Dominio.Entidades.Vendas;

namespace Hiper.Erp.Infraestrutura.Repositorios.API.Vendas
{
    public class RepositorioVendasApi : RespositorioBase<EntidadeVenda>, IRepositorioVendas
    {
        private readonly string endpoint = "/api/Vendas";
        private readonly RepositorioApi repositorioApi;

        public RepositorioVendasApi(HttpClient httpCliente, IMapper mapeador) : base(httpCliente, mapeador)
        {
            repositorioApi = new RepositorioApi(httpCliente, mapeador);
        }

        public async Task<ResultadoOperacao<EntidadeVenda>> ObtenhaPorCodigoAsync(int codigo)
        {
            return await repositorioApi.GetAsync<EntidadeVenda, DtoVenda>($"{endpoint}/ObtenhaPorCodigo/{codigo}");
        }

        public async Task<ResultadoOperacao<List<EntidadeVenda>>> ObtenhaListaAsync()
        {
            return await repositorioApi.GetAsync<List<EntidadeVenda>, List<DtoVenda>>($"{endpoint}/ObtenhaLista");
        }

        public async Task<ResultadoOperacao<List<EntidadeVenda>>> ObtenhaListaComFiltrosAsync(DtoFiltro filtro)
        {
            return await repositorioApi.PostAsync<List<EntidadeVenda>, List<DtoVenda>>($"{endpoint}/ObtenhaListaComFiltros", filtro);
        }

        public async Task<ResultadoOperacao<EntidadeVenda>> CadastrarAsync(EntidadeVenda entidade)
        {
            return await repositorioApi.PostAsync<EntidadeVenda, DtoVenda>($"{endpoint}/Cadastrar", entidade);
        }

        public async Task<ResultadoOperacao<EntidadeVenda>> AtualizarAsync(EntidadeVenda entidade)
        {
            return await repositorioApi.PutAsync<EntidadeVenda, DtoVenda>($"{endpoint}/Atualizar", entidade);
        }

        public async Task<ResultadoOperacao<bool>> DeletarAsync(int codigo)
        {
            return await repositorioApi.DeleteAsync($"{endpoint}/Deletar/{codigo}");
        }

    }
}
