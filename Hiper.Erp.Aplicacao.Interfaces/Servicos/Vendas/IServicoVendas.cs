using Hiper.Erp.Aplicacao.Dtos.Filtros;
using Hiper.Erp.Aplicacao.Dtos.Vendas;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;

namespace Hiper.Erp.Aplicacao.Interfaces.Servicos.Vendas
{
    public interface IServicoVendas
    {
        Task<ResultadoOperacao<DtoVenda>> ObtenhaPorCodigo(int codigo);
        Task<ResultadoOperacao<List<DtoVenda>>> ObtenhaLista();
        Task<ResultadoOperacao<List<DtoVenda>>> ObtenhaListaComFiltros(DtoFiltro filtro);
        Task<ResultadoOperacao<DtoVenda>> Cadastrar(DtoVenda dtoVenda);
        Task<ResultadoOperacao<DtoVenda>> Atualizar(DtoVenda dtoVenda);
        Task<ResultadoOperacao<bool>> Deletar(int codigo);
    }
}
