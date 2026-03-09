using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.FormasPagamentos;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.FormasPagamentos;
using Hiper.Erp.Aplicacao.Mapeadores;
using Hiper.Erp.Aplicacao.Servicos.FormasPagamentos;
using Hiper.Erp.Dominio.Entidades.FormasPagamentos;
using Moq;
using Xunit;

namespace Hiper.Erp.Testes.XUnitTestes.Servicos.FormasPagamentos
{
    public class ServicoFormasPagamentosTestes
    {
        private readonly Mock<IRepositorioFormasPagamentos> _mockRepFormasPagamentos;
        private readonly IMapper _mapper;
        private readonly ServicoFormasPagamentos _servico;

        public ServicoFormasPagamentosTestes()
        {
            _mockRepFormasPagamentos = new Mock<IRepositorioFormasPagamentos>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapeadorRetaguarda>());
            _mapper = config.CreateMapper();

            _servico = new ServicoFormasPagamentos(_mapper, _mockRepFormasPagamentos.Object);
        }

        #region ObtenhaPorCodigo

        [Fact]
        public async Task ObtenhaPorCodigo_ComCodigoExistente_DeveRetornarFormaPagamento()
        {
            var entidade = new EntidadeFormaPagamento { Codigo = 1, Nome = "Dinheiro" };
            _mockRepFormasPagamentos
                .Setup(r => r.ObtenhaPorCodigoAsync(1))
                .ReturnsAsync(ResultadoOperacao<EntidadeFormaPagamento>.Ok(entidade));

            var resultado = await _servico.ObtenhaPorCodigo(1);

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
            Assert.Equal("Dinheiro", resultado.Dados.Nome);
        }

        [Fact]
        public async Task ObtenhaPorCodigo_QuandoRepositorioLancaExcecao_DeveRetornarErro()
        {
            _mockRepFormasPagamentos
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
            var entidades = new List<EntidadeFormaPagamento>
            {
                new EntidadeFormaPagamento { Codigo = 1, Nome = "Dinheiro" },
                new EntidadeFormaPagamento { Codigo = 2, Nome = "Cartão de Crédito" }
            };
            _mockRepFormasPagamentos
                .Setup(r => r.ObtenhaListaAsync())
                .ReturnsAsync(ResultadoOperacao<List<EntidadeFormaPagamento>>.Ok(entidades));

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
            var dto = new DtoFormaPagamento { Nome = "PIX" };
            var entidadeCadastrada = new EntidadeFormaPagamento { Codigo = 1, Nome = "PIX" };

            _mockRepFormasPagamentos
                .Setup(r => r.CadastrarAsync(It.IsAny<EntidadeFormaPagamento>()))
                .ReturnsAsync(ResultadoOperacao<EntidadeFormaPagamento>.Ok(entidadeCadastrada));

            var resultado = await _servico.Cadastrar(dto);

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
            Assert.Equal("PIX", resultado.Dados.Nome);
        }

        [Fact]
        public async Task Cadastrar_SemNome_DeveRetornarFalhaDeValidacao()
        {
            var dto = new DtoFormaPagamento { Nome = null };

            var resultado = await _servico.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Nome deve ser informado.", resultado.Erros);
        }

        [Fact]
        public async Task Cadastrar_ComNomeVazio_DeveRetornarFalhaDeValidacao()
        {
            var dto = new DtoFormaPagamento { Nome = "" };

            var resultado = await _servico.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Nome deve ser informado.", resultado.Erros);
        }

        #endregion

        #region Atualizar

        [Fact]
        public async Task Atualizar_ComDadosValidos_DeveRetornarSucesso()
        {
            var dto = new DtoFormaPagamento { Codigo = 1, Nome = "Cartão Atualizado" };
            var entidadeAtualizada = new EntidadeFormaPagamento { Codigo = 1, Nome = "Cartão Atualizado" };

            _mockRepFormasPagamentos
                .Setup(r => r.AtualizarAsync(It.IsAny<EntidadeFormaPagamento>()))
                .ReturnsAsync(ResultadoOperacao<EntidadeFormaPagamento>.Ok(entidadeAtualizada));

            var resultado = await _servico.Atualizar(dto);

            Assert.True(resultado.Sucesso);
            Assert.Equal("Cartão Atualizado", resultado.Dados.Nome);
        }

        [Fact]
        public async Task Atualizar_SemNome_DeveRetornarFalhaDeValidacao()
        {
            var dto = new DtoFormaPagamento { Codigo = 1, Nome = null };

            var resultado = await _servico.Atualizar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Nome deve ser informado.", resultado.Erros);
        }

        #endregion

        #region Deletar

        [Fact]
        public async Task Deletar_ComCodigoExistente_DeveRetornarSucesso()
        {
            _mockRepFormasPagamentos
                .Setup(r => r.DeletarAsync(1))
                .ReturnsAsync(ResultadoOperacao<bool>.Ok(true));

            var resultado = await _servico.Deletar(1);

            Assert.True(resultado.Sucesso);
            Assert.True(resultado.Dados);
        }

        [Fact]
        public async Task Deletar_QuandoRepositorioLancaExcecao_DeveRetornarErro()
        {
            _mockRepFormasPagamentos
                .Setup(r => r.DeletarAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro ao deletar"));

            var resultado = await _servico.Deletar(999);

            Assert.False(resultado.Sucesso);
            Assert.Contains(resultado.Erros, e => e.Contains("Erro fatal"));
        }

        #endregion
    }
}
