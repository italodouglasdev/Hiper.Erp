using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.Agentes;
using Hiper.Erp.Aplicacao.Dtos.Filtros;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Agentes;
using Hiper.Erp.Dominio.Entidades.Agentes;

namespace Hiper.Erp.Infraestrutura.Repositorios.API.Agentes
{
    public class RepositorioAgentesApi : RespositorioBase<EntidadeAgente>, IRepositorioAgentes
    {

        private readonly string endpoint = "/api/Agentes";
        private readonly RepositorioApi repositorioApi;


        public RepositorioAgentesApi(HttpClient httpCliente, IMapper mapeador) : base(httpCliente, mapeador)
        {
            repositorioApi = new RepositorioApi(httpCliente, mapeador);
        }


        public async Task<ResultadoOperacao<EntidadeAgente>> ObtenhaPorCodigoAsync(int codigo)
        {
            return await repositorioApi.GetAsync<EntidadeAgente, DtoAgente>($"{endpoint}/ObtenhaPorCodigo/{codigo}");
        }

        public async Task<ResultadoOperacao<EntidadeAgente>> ObtenhaPorCnpjCpfAsync(string CnpjCpf)
        {
            return await repositorioApi.GetAsync<EntidadeAgente, DtoAgente>($"{endpoint}/ObtenhaPorCnpjCpf/{CnpjCpf}");
        }

        public async Task<ResultadoOperacao<List<EntidadeAgente>>> ObtenhaListaAsync()
        {
            return await repositorioApi.GetAsync<List<EntidadeAgente>, List<DtoAgente>>($"{endpoint}/ObtenhaLista");
        }

        public async Task<ResultadoOperacao<EntidadeAgente>> CadastrarAsync(EntidadeAgente entidade)
        {
            return await repositorioApi.PostAsync<EntidadeAgente, DtoAgente>($"{endpoint}/Cadastrar", entidade);
        }

        public async Task<ResultadoOperacao<EntidadeAgente>> AtualizarAsync(EntidadeAgente entidade)
        {
            return await repositorioApi.PutAsync<EntidadeAgente, DtoAgente>($"{endpoint}/Atualizar", entidade);
        }

        public async Task<ResultadoOperacao<bool>> DeletarAsync(int codigo)
        {
            return await repositorioApi.DeleteAsync($"{endpoint}/Deletar/{codigo}");
        }

        public async Task<ResultadoOperacao<List<EntidadeAgente>>> ObtenhaListaComFiltrosAsync(DtoFiltro filtro)
        {
            return await repositorioApi.PostAsync<List<EntidadeAgente>, List<DtoAgente>>($"{endpoint}/ObtenhaListaComFiltros", filtro);
        }
    }
}
