using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Dtos.Vendas;
using Hiper.Erp.Utilitarios.ValidadorHelper;

namespace Hiper.Erp.Aplicacao.Validadores.Vendas
{
    public class DtoVendaItemValidador
    {
        public static ResultadoOperacao<bool> Cadastrar(DtoVendaItem dto)
        {
            var resultado = new ResultadoOperacao<bool>();
            var validador = new ResultadoValidacao<bool>();

            return resultado;
        }
        public static ResultadoOperacao<bool> Atualizar(DtoVendaItem dto)
        {
            var resultado = new ResultadoOperacao<bool>();
            var validador = new ResultadoValidacao<bool>();

            return resultado;
        }
    }
}
