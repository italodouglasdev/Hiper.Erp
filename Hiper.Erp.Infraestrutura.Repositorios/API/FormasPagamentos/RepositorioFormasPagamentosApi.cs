using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.FormasPagamentos;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.FormasPagamentos;
using Hiper.Erp.Dominio.Entidades.FormasPagamentos;

namespace Hiper.Erp.Infraestrutura.Repositorios.API.FormasPagamentos
{
    public class RepositorioFormasPagamentosApi : RespositorioBase<EntidadeFormaPagamento>, IRepositorioFormasPagamentos
    {
        private readonly string endpoint = "/api/FormasPagamentos";
        private readonly RepositorioApi repositorioApi;

        public RepositorioFormasPagamentosApi(HttpClient httpCliente, IMapper mapeador) : base(httpCliente, mapeador)
        {
            repositorioApi = new RepositorioApi(httpCliente, mapeador);
        }

        public async Task<ResultadoOperacao<EntidadeFormaPagamento>> ObtenhaPorCodigoAsync(int codigo)
        {
            return await repositorioApi.GetAsync<EntidadeFormaPagamento, DtoFormaPagamento>($"{endpoint}/ObtenhaPorCodigo/{codigo}");
        }

        public async Task<ResultadoOperacao<List<EntidadeFormaPagamento>>> ObtenhaListaAsync()
        {
            return await repositorioApi.GetAsync<List<EntidadeFormaPagamento>, List<DtoFormaPagamento>>($"{endpoint}/ObtenhaLista");
        }

        public async Task<ResultadoOperacao<List<EntidadeFormaPagamento>>> ObtenhaListaComFiltrosAsync(DtoFiltro filtro)
        {
            return await repositorioApi.PostAsync<List<EntidadeFormaPagamento>, List<DtoFormaPagamento>>($"{endpoint}/ObtenhaListaComFiltros", filtro);
        }

        public async Task<ResultadoOperacao<EntidadeFormaPagamento>> CadastrarAsync(EntidadeFormaPagamento entidade)
        {
            return await repositorioApi.PostAsync<EntidadeFormaPagamento, DtoFormaPagamento>($"{endpoint}/Cadastrar", entidade);
        }

        public async Task<ResultadoOperacao<EntidadeFormaPagamento>> AtualizarAsync(EntidadeFormaPagamento entidade)
        {
            return await repositorioApi.PutAsync<EntidadeFormaPagamento, DtoFormaPagamento>($"{endpoint}/Atualizar", entidade);
        }

        public async Task<ResultadoOperacao<bool>> DeletarAsync(int codigo)
        {
            return await repositorioApi.DeleteAsync($"{endpoint}/Deletar/{codigo}");
        }
    }
}
