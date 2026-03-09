using Hiper.Erp.Aplicacao.Dtos.Agentes;
using Hiper.Erp.Aplicacao.Validadores.Agentes;
using Xunit;

namespace Hiper.Erp.Testes.XUnitTestes.Validadores.Agentes
{
    public class DtoAgenteValidadorTestes
    {
        #region Cadastrar

        [Fact]
        public void Cadastrar_ComDadosValidos_CPF_DeveRetornarSucesso()
        {
            var dto = new DtoAgente
            {
                RazaoNome = "João Silva",
                CnpjCpf = "52998224725"
            };

            var resultado = DtoAgenteValidador.Cadastrar(dto);

            Assert.True(resultado.Sucesso);
        }

        [Fact]
        public void Cadastrar_ComDadosValidos_CNPJ_DeveRetornarSucesso()
        {
            var dto = new DtoAgente
            {
                RazaoNome = "Empresa Teste",
                CnpjCpf = "11222333000181"
            };

            var resultado = DtoAgenteValidador.Cadastrar(dto);

            Assert.True(resultado.Sucesso);
        }

        [Fact]
        public void Cadastrar_SemRazaoNome_DeveRetornarFalha()
        {
            var dto = new DtoAgente
            {
                RazaoNome = null,
                CnpjCpf = "52998224725"
            };

            var resultado = DtoAgenteValidador.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Razão/Nome deve ser informado.", resultado.Erros);
        }

        [Fact]
        public void Cadastrar_ComRazaoNomeVazia_DeveRetornarFalha()
        {
            var dto = new DtoAgente
            {
                RazaoNome = "",
                CnpjCpf = "52998224725"
            };

            var resultado = DtoAgenteValidador.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Razão/Nome deve ser informado.", resultado.Erros);
        }

        [Fact]
        public void Cadastrar_ComRazaoNomeSemSobrenome_DeveRetornarFalha()
        {
            var dto = new DtoAgente
            {
                RazaoNome = "João",
                CnpjCpf = "52998224725"
            };

            var resultado = DtoAgenteValidador.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Razão/Nome deve conter nome e sobrenome.", resultado.Erros);
        }

        [Fact]
        public void Cadastrar_ComCnpjCpfInvalido_DeveRetornarFalha()
        {
            var dto = new DtoAgente
            {
                RazaoNome = "João Silva",
                CnpjCpf = "12345"
            };

            var resultado = DtoAgenteValidador.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O valor informado no campo CNPJ/CPF é inválido.", resultado.Erros);
        }

        [Fact]
        public void Cadastrar_ComCnpjCpfNulo_DeveRetornarFalha()
        {
            var dto = new DtoAgente
            {
                RazaoNome = "João Silva",
                CnpjCpf = null
            };

            var resultado = DtoAgenteValidador.Cadastrar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O valor informado no campo CNPJ/CPF é inválido.", resultado.Erros);
        }

        #endregion

        #region Atualizar

        [Fact]
        public void Atualizar_ComDadosValidos_DeveRetornarSucesso()
        {
            var dto = new DtoAgente
            {
                Codigo = 1,
                RazaoNome = "João Silva",
                CnpjCpf = "52998224725"
            };

            var resultado = DtoAgenteValidador.Atualizar(dto);

            Assert.True(resultado.Sucesso);
        }

        [Fact]
        public void Atualizar_SemRazaoNome_DeveRetornarFalha()
        {
            var dto = new DtoAgente
            {
                Codigo = 1,
                RazaoNome = null,
                CnpjCpf = "52998224725"
            };

            var resultado = DtoAgenteValidador.Atualizar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Razão/Nome deve ser informado.", resultado.Erros);
        }

        [Fact]
        public void Atualizar_ComRazaoNomeSemSobrenome_DeveRetornarFalha()
        {
            var dto = new DtoAgente
            {
                Codigo = 1,
                RazaoNome = "João",
                CnpjCpf = "52998224725"
            };

            var resultado = DtoAgenteValidador.Atualizar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O campo Razão/Nome deve conter nome e sobrenome.", resultado.Erros);
        }

        [Fact]
        public void Atualizar_ComCnpjCpfInvalido_DeveRetornarFalha()
        {
            var dto = new DtoAgente
            {
                Codigo = 1,
                RazaoNome = "João Silva",
                CnpjCpf = "00000000000"
            };

            var resultado = DtoAgenteValidador.Atualizar(dto);

            Assert.False(resultado.Sucesso);
            Assert.Contains("O valor informado no campo CNPJ/CPF é inválido.", resultado.Erros);
        }

        #endregion
    }
}
