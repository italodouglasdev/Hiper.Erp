using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Dtos.Produtos;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Produtos;
using Hiper.Erp.Aplicacao.Mapeadores;
using Hiper.Erp.Aplicacao.Servicos.Produtos;
using Hiper.Erp.Dominio.Entidades.Produtos;
using Moq;
using Xunit;

namespace Hiper.Erp.Testes.XUnitTestes.Servicos.Produtos
{
    public class ServicoProdutosTestes
    {
        private readonly Mock<IRepositorioProdutos> _mockRepProdutos;
        private readonly IMapper _mapper;
        private readonly ServicoProdutos _servico;

        public ServicoProdutosTestes()
        {
            _mockRepProdutos = new Mock<IRepositorioProdutos>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapeadorRetaguarda>());
            _mapper = config.CreateMapper();

            _servico = new ServicoProdutos(_mapper, _mockRepProdutos.Object);
        }

        #region ObtenhaPorCodigo

        [Fact]
        public async Task ObtenhaPorCodigo_ComCodigoExistente_DeveRetornarProduto()
        {
            var entidade = new EntidadeProduto { Codigo = 1, Nome = "Produto A", PrecoVenda = 10.50m };
            _mockRepProdutos
                .Setup(r => r.ObtenhaPorCodigoAsync(1))
                .ReturnsAsync(ResultadoOperacao<EntidadeProduto>.Ok(entidade));

            var resultado = await _servico.ObtenhaPorCodigo(1);

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
            Assert.Equal("Produto A", resultado.Dados.Nome);
        }

        [Fact]
        public async Task ObtenhaPorCodigo_QuandoRepositorioLancaExcecao_DeveRetornarErro()
        {
            _mockRepProdutos
                .Setup(r => r.ObtenhaPorCodigoAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro de conexão"));

            var resultado = await _servico.ObtenhaPorCodigo(999);

            Assert.False(resultado.Sucesso);
            Assert.Contains(resultado.Erros, e => e.Contains("Erro fatal"));
        }

        #endregion

        #region ObtenhaLista

        [Fact]
        public async Task ObtenhaLista_ComRegistros_DeveRetornarListaPreenchida()
        {
            var entidades = new List<EntidadeProduto>
            {
                new EntidadeProduto { Codigo = 1, Nome = "Produto A", PrecoVenda = 10.50m },
                new EntidadeProduto { Codigo = 2, Nome = "Produto B", PrecoVenda = 20.00m }
            };
            _mockRepProdutos
                .Setup(r => r.ObtenhaListaAsync())
                .ReturnsAsync(ResultadoOperacao<List<EntidadeProduto>>.Ok(entidades));

            var resultado = await _servico.ObtenhaLista();

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
            Assert.Equal(2, resultado.Dados.Count);
        }

        #endregion

        #region Cadastrar

        [Fact]
        public async Task Cadastrar_ComDadosValidos_DeveRetornarSucesso()
        {
            var dto = new DtoProduto { Nome = "Produto Novo", PrecoVenda = 15.00m };
            var entidadeCadastrada = new EntidadeProduto { Codigo = 1, Nome = "Produto Novo", PrecoVenda = 15.00m };

            _mockRepProdutos
                .Setup(r => r.CadastrarAsync(It.IsAny<EntidadeProduto>()))
                .ReturnsAsync(ResultadoOperacao<EntidadeProduto>.Ok(entidadeCadastrada));

            var resultado = await _servico.Cadastrar(dto);

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
            Assert.Equal("Produto Novo", resultado.Dados.Nome);
        }

        [Fact]
        public async Task Cadastrar_SemNome_DeveRetornarFalhaDeValidacao()
        {
            var dto = new DtoProduto { Nome = null, PrecoVenda = 15.00m };

            var resultado = await _servico.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Nome deve ser informado.", resultado.Erros);
        }

        [Fact]
        public async Task Cadastrar_ComNomeVazio_DeveRetornarFalhaDeValidacao()
        {
            var dto = new DtoProduto { Nome = "", PrecoVenda = 15.00m };

            var resultado = await _servico.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Nome deve ser informado.", resultado.Erros);
        }

        [Fact]
        public async Task Cadastrar_QuandoRepositorioLancaExcecao_DeveRetornarErro()
        {
            var dto = new DtoProduto { Nome = "Produto Novo", PrecoVenda = 15.00m };

            _mockRepProdutos
                .Setup(r => r.CadastrarAsync(It.IsAny<EntidadeProduto>()))
                .ThrowsAsync(new Exception("Erro ao salvar"));

            var resultado = await _servico.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains(resultado.Erros, e => e.Contains("Erro fatal"));
        }

        #endregion

        #region Atualizar

        [Fact]
        public async Task Atualizar_ComDadosValidos_DeveRetornarSucesso()
        {
            var dto = new DtoProduto { Codigo = 1, Nome = "Produto Atualizado", PrecoVenda = 20.00m };
            var entidadeAtualizada = new EntidadeProduto { Codigo = 1, Nome = "Produto Atualizado", PrecoVenda = 20.00m };

            _mockRepProdutos
                .Setup(r => r.AtualizarAsync(It.IsAny<EntidadeProduto>()))
                .ReturnsAsync(ResultadoOperacao<EntidadeProduto>.Ok(entidadeAtualizada));

            var resultado = await _servico.Atualizar(dto);

            Assert.True(resultado.Sucesso);
            Assert.Equal("Produto Atualizado", resultado.Dados.Nome);
        }

        [Fact]
        public async Task Atualizar_SemNome_DeveRetornarFalhaDeValidacao()
        {
            var dto = new DtoProduto { Codigo = 1, Nome = null };

            var resultado = await _servico.Atualizar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Nome deve ser informado.", resultado.Erros);
        }

        #endregion

        #region Deletar

        [Fact]
        public async Task Deletar_ComCodigoExistente_DeveRetornarSucesso()
        {
            _mockRepProdutos
                .Setup(r => r.DeletarAsync(1))
                .ReturnsAsync(ResultadoOperacao<bool>.Ok(true));

            var resultado = await _servico.Deletar(1);

            Assert.True(resultado.Sucesso);
            Assert.True(resultado.Dados);
        }

        [Fact]
        public async Task Deletar_QuandoRepositorioLancaExcecao_DeveRetornarErro()
        {
            _mockRepProdutos
                .Setup(r => r.DeletarAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro ao deletar"));

            var resultado = await _servico.Deletar(999);

            Assert.False(resultado.Sucesso);
            Assert.Contains(resultado.Erros, e => e.Contains("Erro fatal"));
        }

        #endregion
    }
}
