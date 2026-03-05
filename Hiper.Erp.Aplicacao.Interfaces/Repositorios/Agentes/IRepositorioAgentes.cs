using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using Hiper.Erp.Dominio.Entidades.Agentes;

namespace Hiper.Erp.Aplicacao.Interfaces.Repositorios.Agentes
{
    public interface IRepositorioAgentes : IRepositorioGenerico<EntidadeAgente>
    {
        Task<ResultadoOperacao<EntidadeAgente>> ObtenhaPorCnpjCpfAsync(string documento);
    }
}
