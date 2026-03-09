using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.Agentes;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Agentes;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Vendas;
using Hiper.Erp.Aplicacao.Mapeadores;
using Hiper.Erp.Aplicacao.Servicos.Agentes;
using Hiper.Erp.Dominio.Entidades.Agentes;
using Moq;
using Xunit;

namespace Hiper.Erp.Testes.XUnitTestes.Servicos.Agentes
{
    public class ServicoAgentesTestes
    {
        private readonly Mock<IRepositorioAgentes> _mockRepAgentes;
        private readonly Mock<IRepositorioVendas> _mockRepVendas;
        private readonly IMapper _mapper;
        private readonly ServicoAgentes _servico;

        public ServicoAgentesTestes()
        {
            _mockRepAgentes = new Mock<IRepositorioAgentes>();
            _mockRepVendas = new Mock<IRepositorioVendas>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapeadorRetaguarda>());
            _mapper = config.CreateMapper();

            _servico = new ServicoAgentes(_mapper, _mockRepAgentes.Object, _mockRepVendas.Object);
        }

        #region ObtenhaPorCodigo

        [Fact]
        public async Task ObtenhaPorCodigo_ComCodigoExistente_DeveRetornarAgente()
        {
            var entidade = new EntidadeAgente { Codigo = 1, RazaoNome = "João Silva", CnpjCpf = "52998224725" };
            _mockRepAgentes
                .Setup(r => r.ObtenhaPorCodigoAsync(1))
                .ReturnsAsync(ResultadoOperacao<EntidadeAgente>.Ok(entidade));

            var resultado = await _servico.ObtenhaPorCodigo(1);

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
            Assert.Equal("João Silva", resultado.Dados.RazaoNome);
        }

        [Fact]
        public async Task ObtenhaPorCodigo_QuandoRepositorioLancaExcecao_DeveRetornarErro()
        {
            _mockRepAgentes
                .Setup(r => r.ObtenhaPorCodigoAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro de conexão"));

            var resultado = await _servico.ObtenhaPorCodigo(999);

            Assert.False(resultado.Sucesso);
            Assert.Contains(resultado.Erros, e => e.Contains("Erro fatal"));
        }

        #endregion

        #region ObtenhaPorCnpjCpf

        [Fact]
        public async Task ObtenhaPorCnpjCpf_ComDocumentoExistente_DeveRetornarAgente()
        {
            var entidade = new EntidadeAgente { Codigo = 1, RazaoNome = "João Silva", CnpjCpf = "52998224725" };
            _mockRepAgentes
                .Setup(r => r.ObtenhaPorCnpjCpfAsync("52998224725"))
                .ReturnsAsync(ResultadoOperacao<EntidadeAgente>.Ok(entidade));

            var resultado = await _servico.ObtenhaPorCnpjCpf("52998224725");

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
            Assert.Equal("52998224725", resultado.Dados.CnpjCpf);
        }

        #endregion

        #region ObtenhaLista

        [Fact]
        public async Task ObtenhaLista_ComRegistros_DeveRetornarListaPreenchida()
        {
            var entidades = new List<EntidadeAgente>
            {
                new EntidadeAgente { Codigo = 1, RazaoNome = "João Silva", CnpjCpf = "52998224725" },
                new EntidadeAgente { Codigo = 2, RazaoNome = "Maria Santos", CnpjCpf = "11222333000181" }
            };
            _mockRepAgentes
                .Setup(r => r.ObtenhaListaAsync())
                .ReturnsAsync(ResultadoOperacao<List<EntidadeAgente>>.Ok(entidades));

            var resultado = await _servico.ObtenhaLista();

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
            Assert.Equal(2, resultado.Dados.Count);
        }

        [Fact]
        public async Task ObtenhaLista_QuandoRepositorioLancaExcecao_DeveRetornarErro()
        {
            _mockRepAgentes
                .Setup(r => r.ObtenhaListaAsync())
                .ThrowsAsync(new Exception("Erro de conexão"));

            var resultado = await _servico.ObtenhaLista();

            Assert.False(resultado.Sucesso);
            Assert.Contains(resultado.Erros, e => e.Contains("Erro fatal"));
        }

        #endregion

        #region Cadastrar

        [Fact]
        public async Task Cadastrar_ComDadosValidos_DeveRetornarSucesso()
        {
            var dto = new DtoAgente { RazaoNome = "João Silva", CnpjCpf = "52998224725" };
            var entidadeCadastrada = new EntidadeAgente { Codigo = 1, RazaoNome = "João Silva", CnpjCpf = "52998224725" };

            // Simula que não existe agente com o mesmo CPF
            _mockRepAgentes
                .Setup(r => r.ObtenhaPorCnpjCpfAsync("52998224725"))
                .ReturnsAsync(ResultadoOperacao<EntidadeAgente>.Ok(new EntidadeAgente { Codigo = 0 }));

            _mockRepAgentes
                .Setup(r => r.CadastrarAsync(It.IsAny<EntidadeAgente>()))
                .ReturnsAsync(ResultadoOperacao<EntidadeAgente>.Ok(entidadeCadastrada));

            var resultado = await _servico.Cadastrar(dto);

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
            Assert.Equal(1, resultado.Dados.Codigo);
            Assert.Equal("João Silva", resultado.Dados.RazaoNome);
        }

        [Fact]
        public async Task Cadastrar_ComCpfJaExistente_DeveRetornarFalha()
        {
            var dto = new DtoAgente { RazaoNome = "João Silva", CnpjCpf = "52998224725" };
            var entidadeExistente = new EntidadeAgente { Codigo = 5, RazaoNome = "Outro João", CnpjCpf = "52998224725" };

            _mockRepAgentes
                .Setup(r => r.ObtenhaPorCnpjCpfAsync("52998224725"))
                .ReturnsAsync(ResultadoOperacao<EntidadeAgente>.Ok(entidadeExistente));

            var resultado = await _servico.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("Já existe um agente com o CNPJ/CPF informado.", resultado.Erros);
        }

        [Fact]
        public async Task Cadastrar_SemRazaoNome_DeveRetornarFalhaDeValidacao()
        {
            var dto = new DtoAgente { RazaoNome = null, CnpjCpf = "52998224725" };

            var resultado = await _servico.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Razão/Nome deve ser informado.", resultado.Erros);
        }

        [Fact]
        public async Task Cadastrar_ComRazaoNomeSemSobrenome_DeveRetornarFalhaDeValidacao()
        {
            var dto = new DtoAgente { RazaoNome = "João", CnpjCpf = "52998224725" };

            var resultado = await _servico.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Razão/Nome deve conter nome e sobrenome.", resultado.Erros);
        }

        [Fact]
        public async Task Cadastrar_ComCnpjCpfInvalido_DeveRetornarFalhaDeValidacao()
        {
            var dto = new DtoAgente { RazaoNome = "João Silva", CnpjCpf = "12345" };

            var resultado = await _servico.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O valor informado no campo CNPJ/CPF é inválido.", resultado.Erros);
        }

        [Fact]
        public async Task Cadastrar_QuandoRepositorioLancaExcecao_DeveRetornarErro()
        {
            var dto = new DtoAgente { RazaoNome = "João Silva", CnpjCpf = "52998224725" };

            _mockRepAgentes
                .Setup(r => r.ObtenhaPorCnpjCpfAsync("52998224725"))
                .ThrowsAsync(new Exception("Erro de conexão"));

            var resultado = await _servico.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains(resultado.Erros, e => e.Contains("Erro fatal"));
        }

        #endregion

        #region Atualizar

        [Fact]
        public async Task Atualizar_ComDadosValidos_DeveRetornarSucesso()
        {
            var dto = new DtoAgente { Codigo = 1, RazaoNome = "João Silva Atualizado", CnpjCpf = "52998224725" };
            var entidadeAtualizada = new EntidadeAgente { Codigo = 1, RazaoNome = "João Silva Atualizado", CnpjCpf = "52998224725" };

            _mockRepAgentes
                .Setup(r => r.AtualizarAsync(It.IsAny<EntidadeAgente>()))
                .ReturnsAsync(ResultadoOperacao<EntidadeAgente>.Ok(entidadeAtualizada));

            var resultado = await _servico.Atualizar(dto);

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
            Assert.Equal("João Silva Atualizado", resultado.Dados.RazaoNome);
        }

        [Fact]
        public async Task Atualizar_SemRazaoNome_DeveRetornarFalhaDeValidacao()
        {
            var dto = new DtoAgente { Codigo = 1, RazaoNome = null, CnpjCpf = "52998224725" };

            var resultado = await _servico.Atualizar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Razão/Nome deve ser informado.", resultado.Erros);
        }

        #endregion

        #region Deletar

        [Fact]
        public async Task Deletar_SemVendasVinculadas_DeveRetornarSucesso()
        {
            _mockRepVendas
                .Setup(r => r.ObtenhaListaAsync())
                .ReturnsAsync(ResultadoOperacao<List<Hiper.Erp.Dominio.Entidades.Vendas.EntidadeVenda>>.Ok(
                    new List<Hiper.Erp.Dominio.Entidades.Vendas.EntidadeVenda>()));

            _mockRepAgentes
                .Setup(r => r.DeletarAsync(1))
                .ReturnsAsync(ResultadoOperacao<bool>.Ok(true));

            var resultado = await _servico.Deletar(1);

            Assert.True(resultado.Sucesso);
            Assert.True(resultado.Dados);
        }

        [Fact]
        public async Task Deletar_ComVendasVinculadas_DeveRetornarFalha()
        {
            var vendas = new List<Hiper.Erp.Dominio.Entidades.Vendas.EntidadeVenda>
            {
                new Hiper.Erp.Dominio.Entidades.Vendas.EntidadeVenda { Codigo = 1, CodigoCliente = 1 }
            };

            _mockRepVendas
                .Setup(r => r.ObtenhaListaAsync())
                .ReturnsAsync(ResultadoOperacao<List<Hiper.Erp.Dominio.Entidades.Vendas.EntidadeVenda>>.Ok(vendas));

            var resultado = await _servico.Deletar(1);

            Assert.False(resultado.Sucesso);
            Assert.Contains("Não é possível deletar o agente, existem vendas vinculadas.", resultado.Erros);
        }

        #endregion
    }
}
