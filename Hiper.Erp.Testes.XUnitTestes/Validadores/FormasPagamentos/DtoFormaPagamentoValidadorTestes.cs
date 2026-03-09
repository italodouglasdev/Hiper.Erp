using Hiper.Erp.Aplicacao.Dtos.FormasPagamentos;
using Hiper.Erp.Aplicacao.Validadores.FormasPagamentos;
using Xunit;

namespace Hiper.Erp.Testes.XUnitTestes.Validadores.FormasPagamentos
{
    public class DtoFormaPagamentoValidadorTestes
    {
        #region Cadastrar

        [Fact]
        public void Cadastrar_ComNomePreenchido_DeveRetornarSucesso()
        {
            var dto = new DtoFormaPagamento
            {
                Nome = "Cartão de Crédito"
            };

            var resultado = DtoFormaPagamentoValidador.Cadastrar(dto);

            Assert.True(resultado.Sucesso);
        }

        [Fact]
        public void Cadastrar_SemNome_DeveRetornarFalha()
        {
            var dto = new DtoFormaPagamento
            {
                Nome = null
            };

            var resultado = DtoFormaPagamentoValidador.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Nome deve ser informado.", resultado.Erros);
        }

        [Fact]
        public void Cadastrar_ComNomeVazio_DeveRetornarFalha()
        {
            var dto = new DtoFormaPagamento
            {
                Nome = ""
            };

            var resultado = DtoFormaPagamentoValidador.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Nome deve ser informado.", resultado.Erros);
        }

        [Fact]
        public void Cadastrar_ComNomeApenasEspacos_DeveRetornarFalha()
        {
            var dto = new DtoFormaPagamento
            {
                Nome = "   "
            };

            var resultado = DtoFormaPagamentoValidador.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Nome deve ser informado.", resultado.Erros);
        }

        #endregion

        #region Atualizar

        [Fact]
        public void Atualizar_ComNomePreenchido_DeveRetornarSucesso()
        {
            var dto = new DtoFormaPagamento
            {
                Codigo = 1,
                Nome = "Dinheiro"
            };

            var resultado = DtoFormaPagamentoValidador.Atualizar(dto);

            Assert.True(resultado.Sucesso);
        }

        [Fact]
        public void Atualizar_SemNome_DeveRetornarFalha()
        {
            var dto = new DtoFormaPagamento
            {
                Codigo = 1,
                Nome = null
            };

            var resultado = DtoFormaPagamentoValidador.Atualizar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Nome deve ser informado.", resultado.Erros);
        }

        #endregion
    }
}
