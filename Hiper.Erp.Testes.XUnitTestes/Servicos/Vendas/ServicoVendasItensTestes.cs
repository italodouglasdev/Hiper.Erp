using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Dtos.Vendas;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Vendas;
using Hiper.Erp.Aplicacao.Mapeadores;
using Hiper.Erp.Aplicacao.Servicos.Vendas;
using Hiper.Erp.Dominio.Entidades.Vendas;
using Moq;
using Xunit;

namespace Hiper.Erp.Testes.XUnitTestes.Servicos.Vendas
{
    public class ServicoVendasItensTestes
    {
        private readonly Mock<IRepositorioVendasItens> _mockRepVendasItens;
        private readonly IMapper _mapper;
        private readonly ServicoVendasItens _servico;

        public ServicoVendasItensTestes()
        {
            _mockRepVendasItens = new Mock<IRepositorioVendasItens>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapeadorRetaguarda>());
            _mapper = config.CreateMapper();

            _servico = new ServicoVendasItens(_mapper, _mockRepVendasItens.Object);
        }

        #region ObtenhaPorCodigo

        [Fact]
        public async Task ObtenhaPorCodigo_ComCodigoExistente_DeveRetornarItem()
        {
            var entidade = new EntidadeVendaItem
            {
                Codigo = 1,
                CodigoVenda = 1,
                CodigoProduto = 10,
                NomeProduto = "Produto A",
                Quantidade = 2,
                ValorUnitario = 25.00m,
                ValorTotal = 50.00m
            };
            _mockRepVendasItens
                .Setup(r => r.ObtenhaPorCodigoAsync(1))
                .ReturnsAsync(ResultadoOperacao<EntidadeVendaItem>.Ok(entidade));

            var resultado = await _servico.ObtenhaPorCodigo(1);

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
            Assert.Equal("Produto A", resultado.Dados.NomeProduto);
        }

        [Fact]
        public async Task ObtenhaPorCodigo_QuandoRepositorioLancaExcecao_DeveRetornarErro()
        {
            _mockRepVendasItens
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
            var entidades = new List<EntidadeVendaItem>
            {
                new EntidadeVendaItem { Codigo = 1, NomeProduto = "Produto A", Quantidade = 2, ValorTotal = 50.00m },
                new EntidadeVendaItem { Codigo = 2, NomeProduto = "Produto B", Quantidade = 1, ValorTotal = 30.00m }
            };
            _mockRepVendasItens
                .Setup(r => r.ObtenhaListaAsync())
                .ReturnsAsync(ResultadoOperacao<List<EntidadeVendaItem>>.Ok(entidades));

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
            var dto = new DtoVendaItem
            {
                CodigoVenda = 1,
                CodigoProduto = 10,
                NomeProduto = "Produto A",
                Quantidade = 3,
                ValorUnitario = 10.00m,
                ValorTotal = 30.00m
            };
            var entidadeCadastrada = new EntidadeVendaItem
            {
                Codigo = 1,
                CodigoVenda = 1,
                CodigoProduto = 10,
                NomeProduto = "Produto A",
                Quantidade = 3,
                ValorUnitario = 10.00m,
                ValorTotal = 30.00m
            };

            _mockRepVendasItens
                .Setup(r => r.CadastrarAsync(It.IsAny<EntidadeVendaItem>()))
                .ReturnsAsync(ResultadoOperacao<EntidadeVendaItem>.Ok(entidadeCadastrada));

            var resultado = await _servico.Cadastrar(dto);

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
            Assert.Equal(1, resultado.Dados.Codigo);
            Assert.Equal("Produto A", resultado.Dados.NomeProduto);
        }

        [Fact]
        public async Task Cadastrar_QuandoRepositorioLancaExcecao_DeveRetornarErro()
        {
            var dto = new DtoVendaItem
            {
                CodigoVenda = 1,
                CodigoProduto = 10,
                NomeProduto = "Produto A"
            };

            _mockRepVendasItens
                .Setup(r => r.CadastrarAsync(It.IsAny<EntidadeVendaItem>()))
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
            var dto = new DtoVendaItem
            {
                Codigo = 1,
                CodigoVenda = 1,
                CodigoProduto = 10,
                NomeProduto = "Produto Atualizado",
                Quantidade = 5,
                ValorUnitario = 10.00m,
                ValorTotal = 50.00m
            };
            var entidadeAtualizada = new EntidadeVendaItem
            {
                Codigo = 1,
                CodigoVenda = 1,
                CodigoProduto = 10,
                NomeProduto = "Produto Atualizado",
                Quantidade = 5,
                ValorUnitario = 10.00m,
                ValorTotal = 50.00m
            };

            _mockRepVendasItens
                .Setup(r => r.AtualizarAsync(It.IsAny<EntidadeVendaItem>()))
                .ReturnsAsync(ResultadoOperacao<EntidadeVendaItem>.Ok(entidadeAtualizada));

            var resultado = await _servico.Atualizar(dto);

            Assert.True(resultado.Sucesso);
            Assert.Equal("Produto Atualizado", resultado.Dados.NomeProduto);
        }

        #endregion

        #region Deletar

        [Fact]
        public async Task Deletar_ComCodigoExistente_DeveRetornarSucesso()
        {
            _mockRepVendasItens
                .Setup(r => r.DeletarAsync(1))
                .ReturnsAsync(ResultadoOperacao<bool>.Ok(true));

            var resultado = await _servico.Deletar(1);

            Assert.True(resultado.Sucesso);
            Assert.True(resultado.Dados);
        }

        [Fact]
        public async Task Deletar_QuandoRepositorioLancaExcecao_DeveRetornarErro()
        {
            _mockRepVendasItens
                .Setup(r => r.DeletarAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro ao deletar"));

            var resultado = await _servico.Deletar(999);

            Assert.False(resultado.Sucesso);
            Assert.Contains(resultado.Erros, e => e.Contains("Erro fatal"));
        }

        #endregion
    }
}
