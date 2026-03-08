using Hiper.Erp.Aplicacao.Dtos.Agentes;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;

namespace Hiper.Erp.Aplicacao.Interfaces.Servicos.Agentes
{
    public interface IServicoAgentes
    {
        Task<ResultadoOperacao<DtoAgente>> ObtenhaPorCodigo(int codigo);

        Task<ResultadoOperacao<DtoAgente>> ObtenhaPorCnpjCpf(string cnpjCpf);

        Task<ResultadoOperacao<List<DtoAgente>>> ObtenhaLista();

        Task<ResultadoOperacao<List<DtoAgente>>> ObtenhaListaComFiltros(DtoFiltro filtro);

        Task<ResultadoOperacao<DtoAgente>> Cadastrar(DtoAgente dto);

        Task<ResultadoOperacao<DtoAgente>> Atualizar(DtoAgente dto);

        Task<ResultadoOperacao<bool>> Deletar(int codigo);
    }
}
