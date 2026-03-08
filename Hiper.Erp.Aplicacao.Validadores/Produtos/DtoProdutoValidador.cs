using Hiper.Erp.Aplicacao.Dtos.FormasPagamentos;
using Hiper.Erp.Aplicacao.Dtos.Produtos;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using Hiper.Erp.Utilitarios.ValidadorHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiper.Erp.Aplicacao.Validadores.Produtos
{
    public class DtoProdutoValidador
    {
        public static ResultadoOperacao<bool> Cadastrar(DtoProduto dto)
        {
            var resultado = new ResultadoOperacao<bool>();
            var validador = new ResultadoValidacao<bool>();

            validador = ValidadorString.CampoInformado(dto.Nome);
            if (!validador.Sucesso)
            {
                resultado.AdicionarErro("O campo Nome deve ser informado.");
                return resultado;
            }

            return resultado;
        }

        public static ResultadoOperacao<bool> Atualizar(DtoProduto dto)
        {
            var resultado = new ResultadoOperacao<bool>();
            var validador = new ResultadoValidacao<bool>();

            validador = ValidadorString.CampoInformado(dto.Nome);
            if (!validador.Sucesso)
            {
                resultado.AdicionarErro("O campo Nome deve ser informado.");
                return resultado;
            }

            return resultado;
        }
    }
}
