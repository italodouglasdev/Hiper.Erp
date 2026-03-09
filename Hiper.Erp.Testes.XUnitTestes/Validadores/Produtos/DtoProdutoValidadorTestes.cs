using Hiper.Erp.Aplicacao.Dtos.Produtos;
using Hiper.Erp.Aplicacao.Validadores.Produtos;
using Xunit;

namespace Hiper.Erp.Testes.XUnitTestes.Validadores.Produtos
{
    public class DtoProdutoValidadorTestes
    {
        #region Cadastrar

        [Fact]
        public void Cadastrar_ComNomePreenchido_DeveRetornarSucesso()
        {
            var dto = new DtoProduto
            {
                Nome = "Produto Teste",
                PrecoVenda = 10.50m
            };

            var resultado = DtoProdutoValidador.Cadastrar(dto);

            Assert.True(resultado.Sucesso);
        }

        [Fact]
        public void Cadastrar_SemNome_DeveRetornarFalha()
        {
            var dto = new DtoProduto
            {
                Nome = null,
                PrecoVenda = 10.50m
            };

            var resultado = DtoProdutoValidador.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Nome deve ser informado.", resultado.Erros);
        }

        [Fact]
        public void Cadastrar_ComNomeVazio_DeveRetornarFalha()
        {
            var dto = new DtoProduto
            {
                Nome = "",
                PrecoVenda = 10.50m
            };

            var resultado = DtoProdutoValidador.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Nome deve ser informado.", resultado.Erros);
        }

        [Fact]
        public void Cadastrar_ComNomeApenasEspacos_DeveRetornarFalha()
        {
            var dto = new DtoProduto
            {
                Nome = "   ",
                PrecoVenda = 10.50m
            };

            var resultado = DtoProdutoValidador.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Nome deve ser informado.", resultado.Erros);
        }

        #endregion

        #region Atualizar

        [Fact]
        public void Atualizar_ComNomePreenchido_DeveRetornarSucesso()
        {
            var dto = new DtoProduto
            {
                Codigo = 1,
                Nome = "Produto Atualizado"
            };

            var resultado = DtoProdutoValidador.Atualizar(dto);

            Assert.True(resultado.Sucesso);
        }

        [Fact]
        public void Atualizar_SemNome_DeveRetornarFalha()
        {
            var dto = new DtoProduto
            {
                Codigo = 1,
                Nome = null
            };

            var resultado = DtoProdutoValidador.Atualizar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Nome deve ser informado.", resultado.Erros);
        }

        #endregion
    }
}
