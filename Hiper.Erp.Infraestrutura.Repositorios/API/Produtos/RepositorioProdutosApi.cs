using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.Filtros;
using Hiper.Erp.Aplicacao.Dtos.Produtos;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Produtos;
using Hiper.Erp.Dominio.Entidades.Produtos;

namespace Hiper.Erp.Infraestrutura.Repositorios.API.Produtos
{
    public class RepositorioProdutosApi : RespositorioBase<EntidadeProduto>, IRepositorioProdutos
    {
        private readonly string endpoint = "/api/Produtos";
        private readonly RepositorioApi repositorioApi;

        public RepositorioProdutosApi(HttpClient httpCliente, IMapper mapeador) : base(httpCliente, mapeador)
        {
            repositorioApi = new RepositorioApi(httpCliente, mapeador);
        }

        public async Task<ResultadoOperacao<EntidadeProduto>> ObtenhaPorCodigoAsync(int codigo)
        {
            return await repositorioApi.GetAsync<EntidadeProduto, DtoProduto>($"{endpoint}/ObtenhaPorCodigo/{codigo}");
        }

        public async Task<ResultadoOperacao<List<EntidadeProduto>>> ObtenhaListaAsync()
        {
            return await repositorioApi.GetAsync<List<EntidadeProduto>, List<DtoProduto>>($"{endpoint}/ObtenhaLista");
        }

        public async Task<ResultadoOperacao<List<EntidadeProduto>>> ObtenhaListaComFiltrosAsync(DtoFiltro filtro)
        {
            return await repositorioApi.PostAsync<List<EntidadeProduto>, List<DtoProduto>>($"{endpoint}/ObtenhaListaComFiltros", filtro);
        }

        public async Task<ResultadoOperacao<EntidadeProduto>> CadastrarAsync(EntidadeProduto entidade)
        {
            return await repositorioApi.PostAsync<EntidadeProduto, DtoProduto>($"{endpoint}/Cadastrar", entidade);
        }

        public async Task<ResultadoOperacao<EntidadeProduto>> AtualizarAsync(EntidadeProduto entidade)
        {
            return await repositorioApi.PutAsync<EntidadeProduto, DtoProduto>($"{endpoint}/Atualizar", entidade);
        }

        public async Task<ResultadoOperacao<bool>> DeletarAsync(int codigo)
        {
            return await repositorioApi.DeleteAsync($"{endpoint}/Deletar/{codigo}");
        }
    }
}
