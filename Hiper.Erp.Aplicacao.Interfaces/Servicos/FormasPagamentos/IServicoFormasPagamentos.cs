using Hiper.Erp.Aplicacao.Dtos.FormasPagamentos;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;

namespace Hiper.Erp.Aplicacao.Interfaces.Servicos.FormasPagamentos
{
    public interface IServicoFormasPagamentos
    {
        Task<ResultadoOperacao<DtoFormaPagamento>> ObtenhaPorCodigo(int codigo);
        Task<ResultadoOperacao<List<DtoFormaPagamento>>> ObtenhaLista();
        Task<ResultadoOperacao<List<DtoFormaPagamento>>> ObtenhaListaComFiltros(DtoFiltro filtro);
        Task<ResultadoOperacao<DtoFormaPagamento>> Cadastrar(DtoFormaPagamento dtoFormaPagamento);
        Task<ResultadoOperacao<DtoFormaPagamento>> Atualizar(DtoFormaPagamento dtoFormaPagamento);
        Task<ResultadoOperacao<bool>> Deletar(int codigo);
    }
}
