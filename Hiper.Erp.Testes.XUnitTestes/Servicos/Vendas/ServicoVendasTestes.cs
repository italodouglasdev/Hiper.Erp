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
    public class ServicoVendasTestes
    {
        private readonly Mock<IRepositorioVendas> _mockRepVendas;
        private readonly Mock<IRepositorioVendasItens> _mockRepVendasItens;
        private readonly IMapper _mapper;
        private readonly ServicoVendas _servico;

        public ServicoVendasTestes()
        {
            _mockRepVendas = new Mock<IRepositorioVendas>();
            _mockRepVendasItens = new Mock<IRepositorioVendasItens>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapeadorRetaguarda>());
            _mapper = config.CreateMapper();

            _servico = new ServicoVendas(_mapper, _mockRepVendas.Object, _mockRepVendasItens.Object);
        }

        #region ObtenhaPorCodigo

        [Fact]
        public async Task ObtenhaPorCodigo_ComCodigoExistente_DeveRetornarVenda()
        {
            var entidade = new EntidadeVenda
            {
                Codigo = 1,
                CodigoCliente = 1,
                NomeCliente = "João Silva",
                CodigoFormaPagamento = 1,
                FormaPagamento = "Dinheiro",
                ValorTotal = 100.00m,
                DataHora = DateTime.Now
            };
            _mockRepVendas
                .Setup(r => r.ObtenhaPorCodigoAsync(1))
                .ReturnsAsync(ResultadoOperacao<EntidadeVenda>.Ok(entidade));

            var resultado = await _servico.ObtenhaPorCodigo(1);

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
            Assert.Equal("João Silva", resultado.Dados.NomeCliente);
        }

        [Fact]
        public async Task ObtenhaPorCodigo_QuandoRepositorioLancaExcecao_DeveRetornarErro()
        {
            _mockRepVendas
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
            var entidades = new List<EntidadeVenda>
            {
                new EntidadeVenda { Codigo = 1, NomeCliente = "Cliente A", ValorTotal = 50.00m },
                new EntidadeVenda { Codigo = 2, NomeCliente = "Cliente B", ValorTotal = 75.00m }
            };
            _mockRepVendas
                .Setup(r => r.ObtenhaListaAsync())
                .ReturnsAsync(ResultadoOperacao<List<EntidadeVenda>>.Ok(entidades));

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
            var dto = new DtoVenda
            {
                CodigoCliente = 1,
                NomeCliente = "João Silva",
                CodigoFormaPagamento = 1,
                FormaPagamento = "Dinheiro",
                ValorTotal = 100.00m,
                DataHora = DateTime.Now
            };
            var entidadeCadastrada = new EntidadeVenda
            {
                Codigo = 1,
                CodigoCliente = 1,
                NomeCliente = "João Silva",
                CodigoFormaPagamento = 1,
                FormaPagamento = "Dinheiro",
                ValorTotal = 100.00m,
                DataHora = DateTime.Now
            };

            _mockRepVendas
                .Setup(r => r.CadastrarAsync(It.IsAny<EntidadeVenda>()))
                .ReturnsAsync(ResultadoOperacao<EntidadeVenda>.Ok(entidadeCadastrada));

            var resultado = await _servico.Cadastrar(dto);

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
            Assert.Equal(1, resultado.Dados.Codigo);
        }

        [Fact]
        public async Task Cadastrar_QuandoRepositorioLancaExcecao_DeveRetornarErro()
        {
            var dto = new DtoVenda
            {
                CodigoCliente = 1,
                NomeCliente = "João Silva",
                CodigoFormaPagamento = 1,
                FormaPagamento = "Dinheiro"
            };

            _mockRepVendas
                .Setup(r => r.CadastrarAsync(It.IsAny<EntidadeVenda>()))
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
            var dto = new DtoVenda
            {
                Codigo = 1,
                CodigoCliente = 1,
                NomeCliente = "João Silva",
                CodigoFormaPagamento = 2,
                FormaPagamento = "Cartão",
                ValorTotal = 150.00m
            };
            var entidadeAtualizada = new EntidadeVenda
            {
                Codigo = 1,
                CodigoCliente = 1,
                NomeCliente = "João Silva",
                CodigoFormaPagamento = 2,
                FormaPagamento = "Cartão",
                ValorTotal = 150.00m
            };

            _mockRepVendas
                .Setup(r => r.AtualizarAsync(It.IsAny<EntidadeVenda>()))
                .ReturnsAsync(ResultadoOperacao<EntidadeVenda>.Ok(entidadeAtualizada));

            var resultado = await _servico.Atualizar(dto);

            Assert.True(resultado.Sucesso);
            Assert.Equal("Cartão", resultado.Dados.FormaPagamento);
        }

        #endregion

        #region Deletar

        [Fact]
        public async Task Deletar_SemItensVinculados_DeveRetornarSucesso()
        {
            _mockRepVendasItens
                .Setup(r => r.ObtenhaListaAsync())
                .ReturnsAsync(ResultadoOperacao<List<EntidadeVendaItem>>.Ok(new List<EntidadeVendaItem>()));

            _mockRepVendas
                .Setup(r => r.DeletarAsync(1))
                .ReturnsAsync(ResultadoOperacao<bool>.Ok(true));

            var resultado = await _servico.Deletar(1);

            Assert.True(resultado.Sucesso);
            Assert.True(resultado.Dados);
        }

        [Fact]
        public async Task Deletar_ComItensVinculados_DeveRetornarFalha()
        {
            var itens = new List<EntidadeVendaItem>
            {
                new EntidadeVendaItem { Codigo = 1, CodigoVenda = 1, CodigoProduto = 1 }
            };

            _mockRepVendasItens
                .Setup(r => r.ObtenhaListaAsync())
                .ReturnsAsync(ResultadoOperacao<List<EntidadeVendaItem>>.Ok(itens));

            var resultado = await _servico.Deletar(1);

            Assert.False(resultado.Sucesso);
            Assert.Contains("Não é possível deletar a venda, existem itens vinculadas.", resultado.Erros);
        }

        [Fact]
        public async Task Deletar_QuandoRepositorioLancaExcecao_DeveRetornarErro()
        {
            _mockRepVendasItens
                .Setup(r => r.ObtenhaListaAsync())
                .ThrowsAsync(new Exception("Erro de conexão"));

            var resultado = await _servico.Deletar(1);

            Assert.False(resultado.Sucesso);
            Assert.Contains(resultado.Erros, e => e.Contains("Erro fatal"));
        }

        #endregion
    }
}
