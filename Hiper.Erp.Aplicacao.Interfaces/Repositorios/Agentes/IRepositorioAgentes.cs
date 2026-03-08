using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Dominio.Entidades.Agentes;

namespace Hiper.Erp.Aplicacao.Interfaces.Repositorios.Agentes
{
    public interface IRepositorioAgentes : IRepositorioGenerico<EntidadeAgente>
    {
        Task<ResultadoOperacao<EntidadeAgente>> ObtenhaPorCnpjCpfAsync(string documento);
    }
}
