using Hiper.Erp.Aplicacao.Dtos.Agentes;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using Hiper.Erp.Utilitarios.ValidadorHelper;

namespace Hiper.Erp.Aplicacao.Validadores.Agentes
{
    public class DtoAgenteValidador
    {
        public static ResultadoOperacao<bool> Cadastrar(DtoAgente dto)
        {
            var resultado = new ResultadoOperacao<bool>();
            var validador = new ResultadoValidacao<bool>();

            validador = ValidadorString.CampoInformado(dto.RazaoNome);
            if (!validador.Sucesso)
            {
                resultado.AdicionarErro("O campo Razão/Nome deve ser informado.");
                return resultado;
            }

            validador = ValidadorString.NomeComposto(dto.RazaoNome);
            if (!validador.Sucesso)
            {
                resultado.AdicionarErro("O campo Razão/Nome deve conter nome e sobrenome.");
                return resultado;
            }

            validador = ValidadorString.CNPJouCPF(dto.CnpjCpf);
            if (!validador.Sucesso)
            {
                resultado.AdicionarErro("O valor informado no campo CNPJ/CPF é inválido.");
                return resultado;
            }

            return resultado;
        }

        public static ResultadoOperacao<bool> Atualizar(DtoAgente dto)
        {
            var resultado = new ResultadoOperacao<bool>();
            var validador = new ResultadoValidacao<bool>();

            validador = ValidadorString.CampoInformado(dto.RazaoNome);
            if (!validador.Sucesso)
            {
                resultado.AdicionarErro("O campo Razão/Nome deve ser informado.");
                return resultado;
            }

            validador = ValidadorString.NomeComposto(dto.RazaoNome);
            if (!validador.Sucesso)
            {
                resultado.AdicionarErro("O campo Razão/Nome deve conter nome e sobrenome.");
                return resultado;
            }

            validador = ValidadorString.CNPJouCPF(dto.CnpjCpf);
            if (!validador.Sucesso)
            {
                resultado.AdicionarErro("O valor informado no campo CNPJ/CPF é inválido.");
                return resultado;
            }

            return resultado;
        }
    }
}
