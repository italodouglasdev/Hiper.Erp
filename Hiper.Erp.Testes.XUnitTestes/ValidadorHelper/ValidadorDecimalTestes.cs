using Hiper.Erp.Utilitarios.ValidadorHelper;
using Xunit;

namespace Hiper.Erp.Testes.XUnitTestes.ValidadorHelper
{
    public class ValidadorDecimalTestes
    {
        [Theory]
        [InlineData(0.01)]
        [InlineData(1)]
        [InlineData(100.50)]
        [InlineData(999999.99)]
        public void MaiorQueZero_ComValorPositivo_DeveRetornarSucesso(decimal valor)
        {
            var resultado = ValidadorDecimal.MaiorQueZero(valor);

            Assert.True(resultado.Sucesso);
            Assert.True(resultado.Dados);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100.50)]
        public void MaiorQueZero_ComValorZeroOuNegativo_DeveRetornarFalha(decimal valor)
        {
            var resultado = ValidadorDecimal.MaiorQueZero(valor);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O valor informado deve ser maior do que zero.", resultado.Erros);
        }
    }
}
