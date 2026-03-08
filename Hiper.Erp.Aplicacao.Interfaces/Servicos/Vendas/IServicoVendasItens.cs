using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Dtos.Vendas;

namespace Hiper.Erp.Aplicacao.Interfaces.Servicos.Vendas
{
    public interface IServicoVendasItens
    {
        Task<ResultadoOperacao<DtoVendaItem>> ObtenhaPorCodigo(int codigo);
        Task<ResultadoOperacao<List<DtoVendaItem>>> ObtenhaLista();
        Task<ResultadoOperacao<List<DtoVendaItem>>> ObtenhaListaComFiltros(DtoFiltro filtro);
        Task<ResultadoOperacao<DtoVendaItem>> Cadastrar(DtoVendaItem dtoVendaItem);
        Task<ResultadoOperacao<DtoVendaItem>> Atualizar(DtoVendaItem dtoVendaItem);
        Task<ResultadoOperacao<bool>> Deletar(int codigo);
    }
}
