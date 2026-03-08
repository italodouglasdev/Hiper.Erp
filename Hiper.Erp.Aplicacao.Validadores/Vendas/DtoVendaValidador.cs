using Hiper.Erp.Aplicacao.Dtos.Vendas;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using Hiper.Erp.Utilitarios.ValidadorHelper;

namespace Hiper.Erp.Aplicacao.Validadores.Vendas
{
    public class DtoVendaValidador
    {
        public static ResultadoOperacao<bool> Cadastrar(DtoVenda dto)
        {
            var resultado = new ResultadoOperacao<bool>();
            var validador = new ResultadoValidacao<bool>();           

            return resultado;
        }
        public static ResultadoOperacao<bool> Atualizar(DtoVenda dto)
        {
            var resultado = new ResultadoOperacao<bool>();
            var validador = new ResultadoValidacao<bool>();

            return resultado;
        }
    }
}
