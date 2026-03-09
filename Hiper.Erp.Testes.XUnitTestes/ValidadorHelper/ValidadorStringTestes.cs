using Hiper.Erp.Utilitarios.ValidadorHelper;
using Xunit;

namespace Hiper.Erp.Testes.XUnitTestes.ValidadorHelper
{
    public class ValidadorStringTestes
    {
        #region CampoInformado

        [Fact]
        public void CampoInformado_ComValorPreenchido_DeveRetornarSucesso()
        {
            var resultado = ValidadorString.CampoInformado("Teste");

            Assert.True(resultado.Sucesso);
            Assert.True(resultado.Dados);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void CampoInformado_ComValorVazioOuNulo_DeveRetornarFalha(string? valor)
        {
            var resultado = ValidadorString.CampoInformado(valor);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O valor do campo não foi informado.", resultado.Erros);
        }

        #endregion

        #region NomeComposto

        [Theory]
        [InlineData("João Silva")]
        [InlineData("Maria Aparecida Santos")]
        [InlineData("Carlos Eduardo de Souza")]
        public void NomeComposto_ComNomeESobrenome_DeveRetornarSucesso(string nome)
        {
            var resultado = ValidadorString.NomeComposto(nome);

            Assert.True(resultado.Sucesso);
        }

        [Fact]
        public void NomeComposto_ComApenasUmNome_DeveRetornarFalha()
        {
            var resultado = ValidadorString.NomeComposto("João");

            Assert.False(resultado.Sucesso);
            Assert.Contains("Informe o nome completo (nome e sobrenome).", resultado.Erros);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void NomeComposto_ComValorVazioOuNulo_DeveRetornarFalha(string? valor)
        {
            var resultado = ValidadorString.NomeComposto(valor);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O nome deve ser informado.", resultado.Erros);
        }

        [Fact]
        public void NomeComposto_ComParteDoNomeMenorQue2Caracteres_DeveRetornarFalha()
        {
            var resultado = ValidadorString.NomeComposto("João S");

            Assert.False(resultado.Sucesso);
            Assert.Contains("Cada parte do nome deve conter pelo menos 2 caracteres.", resultado.Erros);
        }

        #endregion

        #region Email

        [Theory]
        [InlineData("teste@email.com")]
        [InlineData("usuario@dominio.com.br")]
        [InlineData("nome.sobrenome@empresa.org")]
        public void Email_ComFormatoValido_DeveRetornarSucesso(string email)
        {
            var resultado = ValidadorString.Email(email);

            Assert.True(resultado.Sucesso);
        }

        [Theory]
        [InlineData("emailinvalido")]
        [InlineData("email@")]
        [InlineData("@dominio.com")]
        [InlineData("email sem arroba")]
        public void Email_ComFormatoInvalido_DeveRetornarFalha(string email)
        {
            var resultado = ValidadorString.Email(email);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O e-mail informado é inválido.", resultado.Erros);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Email_ComValorVazioOuNulo_DeveRetornarFalha(string? valor)
        {
            var resultado = ValidadorString.Email(valor);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O e-mail deve ser informado.", resultado.Erros);
        }

        #endregion

        #region Telefone

        [Theory]
        [InlineData("11987654321")]
        [InlineData("1134567890")]
        [InlineData("(11) 98765-4321")]
        [InlineData("(11) 3456-7890")]
        public void Telefone_ComFormatoValido_DeveRetornarSucesso(string telefone)
        {
            var resultado = ValidadorString.Telefone(telefone);

            Assert.True(resultado.Sucesso);
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("123456789012")]
        public void Telefone_ComQuantidadeDeDigitosInvalida_DeveRetornarFalha(string telefone)
        {
            var resultado = ValidadorString.Telefone(telefone);

            Assert.False(resultado.Sucesso);
            Assert.Contains("Telefone inválido.", resultado.Erros);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Telefone_ComValorVazioOuNulo_DeveRetornarFalha(string? valor)
        {
            var resultado = ValidadorString.Telefone(valor);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O telefone deve ser informado.", resultado.Erros);
        }

        #endregion

        #region WhatsApp

        [Fact]
        public void WhatsApp_ComFormatoValido_DeveRetornarSucesso()
        {
            var resultado = ValidadorString.WhatsApp("11987654321");

            Assert.True(resultado.Sucesso);
        }

        [Fact]
        public void WhatsApp_ComQuantidadeDeDigitosDiferenteDe11_DeveRetornarFalha()
        {
            var resultado = ValidadorString.WhatsApp("1198765432");

            Assert.False(resultado.Sucesso);
            Assert.Contains("WhatsApp inválido. Informe DDD + número.", resultado.Erros);
        }

        [Fact]
        public void WhatsApp_SemDigito9AposODDD_DeveRetornarFalha()
        {
            var resultado = ValidadorString.WhatsApp("11887654321");

            Assert.False(resultado.Sucesso);
            Assert.Contains("WhatsApp deve conter o dígito 9 após o DDD.", resultado.Erros);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WhatsApp_ComValorVazioOuNulo_DeveRetornarFalha(string? valor)
        {
            var resultado = ValidadorString.WhatsApp(valor);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O WhatsApp deve ser informado.", resultado.Erros);
        }

        #endregion

        #region CPF

        [Theory]
        [InlineData("52998224725")]
        [InlineData("529.982.247-25")]
        public void CPF_ComValorValido_DeveRetornarSucesso(string cpf)
        {
            var resultado = ValidadorString.CPF(cpf);

            Assert.True(resultado.Sucesso);
        }

        [Theory]
        [InlineData("00000000000")]
        [InlineData("11111111111")]
        [InlineData("99999999999")]
        public void CPF_ComTodosOsDigitosIguais_DeveRetornarFalha(string cpf)
        {
            var resultado = ValidadorString.CPF(cpf);

            Assert.False(resultado.Sucesso);
            Assert.Contains("CPF inválido.", resultado.Erros);
        }

        [Fact]
        public void CPF_ComQuantidadeDeDigitosInvalida_DeveRetornarFalha()
        {
            var resultado = ValidadorString.CPF("1234567");

            Assert.False(resultado.Sucesso);
            Assert.Contains("CPF inválido.", resultado.Erros);
        }

        [Fact]
        public void CPF_ComDigitoVerificadorInvalido_DeveRetornarFalha()
        {
            var resultado = ValidadorString.CPF("52998224700");

            Assert.False(resultado.Sucesso);
            Assert.Contains("CPF inválido.", resultado.Erros);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CPF_ComValorVazioOuNulo_DeveRetornarFalha(string? valor)
        {
            var resultado = ValidadorString.CPF(valor);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O CPF deve ser informado.", resultado.Erros);
        }

        #endregion

        #region CNPJ

        [Theory]
        [InlineData("11222333000181")]
        [InlineData("11.222.333/0001-81")]
        public void CNPJ_ComValorValido_DeveRetornarSucesso(string cnpj)
        {
            var resultado = ValidadorString.CNPJ(cnpj);

            Assert.True(resultado.Sucesso);
        }

        [Theory]
        [InlineData("00000000000000")]
        [InlineData("11111111111111")]
        public void CNPJ_ComTodosOsDigitosIguais_DeveRetornarFalha(string cnpj)
        {
            var resultado = ValidadorString.CNPJ(cnpj);

            Assert.False(resultado.Sucesso);
            Assert.Contains("CNPJ inválido.", resultado.Erros);
        }

        [Fact]
        public void CNPJ_ComQuantidadeDeDigitosInvalida_DeveRetornarFalha()
        {
            var resultado = ValidadorString.CNPJ("1234567");

            Assert.False(resultado.Sucesso);
            Assert.Contains("CNPJ inválido.", resultado.Erros);
        }

        [Fact]
        public void CNPJ_ComDigitoVerificadorInvalido_DeveRetornarFalha()
        {
            var resultado = ValidadorString.CNPJ("11222333000199");

            Assert.False(resultado.Sucesso);
            Assert.Contains("CNPJ inválido.", resultado.Erros);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CNPJ_ComValorVazioOuNulo_DeveRetornarFalha(string? valor)
        {
            var resultado = ValidadorString.CNPJ(valor);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O CNPJ deve ser informado.", resultado.Erros);
        }

        #endregion

        #region CNPJouCPF

        [Fact]
        public void CNPJouCPF_ComCPFValido_DeveRetornarSucesso()
        {
            var resultado = ValidadorString.CNPJouCPF("52998224725");

            Assert.True(resultado.Sucesso);
        }

        [Fact]
        public void CNPJouCPF_ComCNPJValido_DeveRetornarSucesso()
        {
            var resultado = ValidadorString.CNPJouCPF("11222333000181");

            Assert.True(resultado.Sucesso);
        }

        [Fact]
        public void CNPJouCPF_ComQuantidadeDeDigitosInvalida_DeveRetornarFalha()
        {
            var resultado = ValidadorString.CNPJouCPF("12345");

            Assert.False(resultado.Sucesso);
            Assert.Contains("CPF ou CNPJ inválido.", resultado.Erros);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CNPJouCPF_ComValorVazioOuNulo_DeveRetornarFalha(string? valor)
        {
            var resultado = ValidadorString.CNPJouCPF(valor);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O CPF ou CNPJ deve ser informado.", resultado.Erros);
        }

        #endregion
    }
}
