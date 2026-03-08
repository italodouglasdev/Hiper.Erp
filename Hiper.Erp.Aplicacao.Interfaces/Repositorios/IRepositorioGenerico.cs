using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;

namespace Hiper.Erp.Aplicacao.Interfaces.Repositorios
{
    public interface IRepositorioGenerico<TEntidade> where TEntidade : class
    {
        Task<ResultadoOperacao<TEntidade>> ObtenhaPorCodigoAsync(int codigo);
        Task<ResultadoOperacao<List<TEntidade>>> ObtenhaListaAsync();
        Task<ResultadoOperacao<List<TEntidade>>> ObtenhaListaComFiltrosAsync(DtoFiltro filtro);
        Task<ResultadoOperacao<TEntidade>> CadastrarAsync(TEntidade entidade);
        Task<ResultadoOperacao<TEntidade>> AtualizarAsync(TEntidade entidade);
        Task<ResultadoOperacao<bool>> DeletarAsync(int codigo);
    }
}
