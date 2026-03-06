using Hiper.Erp.Aplicacao.Dtos.Filtros;
using Hiper.Erp.Aplicacao.Dtos.Produtos;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;

namespace Hiper.Erp.Aplicacao.Interfaces.Servicos.Produtos
{
    public interface IServicoProdutos
    {
        Task<ResultadoOperacao<DtoProduto>> ObtenhaPorCodigo(int codigo);
        Task<ResultadoOperacao<List<DtoProduto>>> ObtenhaLista();
        Task<ResultadoOperacao<List<DtoProduto>>> ObtenhaListaComFiltros(DtoFiltro filtro);
        Task<ResultadoOperacao<DtoProduto>> Cadastrar(DtoProduto dtoProduto);
        Task<ResultadoOperacao<DtoProduto>> Atualizar(DtoProduto dtoProduto);
        Task<ResultadoOperacao<bool>> Deletar(int codigo);
    }
}
