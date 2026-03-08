using System.Text.RegularExpressions;

namespace Hiper.Erp.Utilitarios.ValidadorHelper
{
    public class ValidadorString
    {
        public static ResultadoValidacao<bool> CampoInformado(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                return ResultadoValidacao<bool>.Ok(true);

            return ResultadoValidacao<bool>.Falha("O valor do campo não foi informado.");
        }

        public static ResultadoValidacao<bool> NomeComposto(string valor)
        {
            if (CampoInformado(valor).Sucesso == false)
                return ResultadoValidacao<bool>.Falha("O nome deve ser informado.");

            var partes = valor.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (partes.Length < 2)
                return ResultadoValidacao<bool>.Falha("Informe o nome completo (nome e sobrenome).");

            foreach (var parte in partes)
            {
                if (parte.Length < 2)
                    return ResultadoValidacao<bool>.Falha("Cada parte do nome deve conter pelo menos 2 caracteres.");
            }

            return ResultadoValidacao<bool>.Ok(true);
        }

        public static ResultadoValidacao<bool> Email(string valor)
        {
            if (CampoInformado(valor).Sucesso == false)
                return ResultadoValidacao<bool>.Falha("O e-mail deve ser informado.");

            var regex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (!Regex.IsMatch(valor, regex))
                return ResultadoValidacao<bool>.Falha("O e-mail informado é inválido.");

            return ResultadoValidacao<bool>.Ok(true);
        }

        public static ResultadoValidacao<bool> Telefone(string valor)
        {
            if (CampoInformado(valor).Sucesso == false)
                return ResultadoValidacao<bool>.Falha("O telefone deve ser informado.");

            var numeros = Regex.Replace(valor, @"\D", "");

            if (numeros.Length < 10 || numeros.Length > 11)
                return ResultadoValidacao<bool>.Falha("Telefone inválido.");

            return ResultadoValidacao<bool>.Ok(true);
        }

        public static ResultadoValidacao<bool> WhatsApp(string valor)
        {
            if (CampoInformado(valor).Sucesso == false)
                return ResultadoValidacao<bool>.Falha("O WhatsApp deve ser informado.");

            var numeros = Regex.Replace(valor, @"\D", "");

            if (numeros.Length != 11)
                return ResultadoValidacao<bool>.Falha("WhatsApp inválido. Informe DDD + número.");

            if (numeros[2] != '9')
                return ResultadoValidacao<bool>.Falha("WhatsApp deve conter o dígito 9 após o DDD.");

            return ResultadoValidacao<bool>.Ok(true);
        }

        public static ResultadoValidacao<bool> CPF(string valor)
        {
            if (CampoInformado(valor).Sucesso == false)
                return ResultadoValidacao<bool>.Falha("O CPF deve ser informado.");

            var cpf = Regex.Replace(valor, @"\D", "");

            if (cpf.Length != 11)
                return ResultadoValidacao<bool>.Falha("CPF inválido.");

            if (cpf.Distinct().Count() == 1)
                return ResultadoValidacao<bool>.Falha("CPF inválido.");

            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (cpf[i] - '0') * (10 - i);

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            if ((cpf[9] - '0') != digito1)
                return ResultadoValidacao<bool>.Falha("CPF inválido.");

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (cpf[i] - '0') * (11 - i);

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            if ((cpf[10] - '0') != digito2)
                return ResultadoValidacao<bool>.Falha("CPF inválido.");

            return ResultadoValidacao<bool>.Ok(true);
        }

        public static ResultadoValidacao<bool> CNPJ(string valor)
        {
            if (CampoInformado(valor).Sucesso == false)
                return ResultadoValidacao<bool>.Falha("O CNPJ deve ser informado.");

            var cnpj = Regex.Replace(valor, @"\D", "");

            if (cnpj.Length != 14)
                return ResultadoValidacao<bool>.Falha("CNPJ inválido.");

            if (cnpj.Distinct().Count() == 1)
                return ResultadoValidacao<bool>.Falha("CNPJ inválido.");

            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCnpj = cnpj.Substring(0, 12);

            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += (tempCnpj[i] - '0') * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();

            tempCnpj += digito;

            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += (tempCnpj[i] - '0') * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            if (!cnpj.EndsWith(digito))
                return ResultadoValidacao<bool>.Falha("CNPJ inválido.");

            return ResultadoValidacao<bool>.Ok(true);
        }

        public static ResultadoValidacao<bool> CNPJouCPF(string valor)
        {
            if (CampoInformado(valor).Sucesso == false)
                return ResultadoValidacao<bool>.Falha("O CPF ou CNPJ deve ser informado.");

            var numeros = Regex.Replace(valor, @"\D", "");

            if (numeros.Length == 11)
                return CPF(numeros);

            if (numeros.Length == 14)
                return CNPJ(numeros);

            return ResultadoValidacao<bool>.Falha("CPF ou CNPJ inválido.");
        }

    }
}