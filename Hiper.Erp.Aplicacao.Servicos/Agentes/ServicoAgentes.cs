using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.Agentes;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Agentes;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Vendas;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Agentes;
using Hiper.Erp.Aplicacao.Validadores.Agentes;
using Hiper.Erp.Dominio.Entidades.Agentes;

namespace Hiper.Erp.Aplicacao.Servicos.Agentes
{
    public class ServicoAgentes : ServicoBase, IServicoAgentes
    {
        #region Propriedades

        private IRepositorioAgentes repAgente;
        private IRepositorioVendas repVendas;

        #endregion

        #region Construtores

        public ServicoAgentes(
            IMapper mapper,
            IRepositorioAgentes repAgente,
            IRepositorioVendas repVendas) : base(mapper)
        {
            this.repAgente = repAgente;
            this.repVendas = repVendas;
        }

        #endregion

        #region Métodos Públicos

        public async Task<ResultadoOperacao<DtoAgente>> ObtenhaPorCodigo(int codigo)
        {
            var resultado = new ResultadoOperacao<DtoAgente>();

            try
            {
                var repAgenteResultado = await repAgente.ObtenhaPorCodigoAsync(codigo);

                resultado.Dados = mapeador.Map<DtoAgente>(repAgenteResultado.Dados);

            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<DtoAgente>> ObtenhaPorCnpjCpf(string cnpjCpf)
        {
            var resultado = new ResultadoOperacao<DtoAgente>();

            try
            {
                var repAgenteResultado = await repAgente.ObtenhaPorCnpjCpfAsync(cnpjCpf);

                resultado.Dados = mapeador.Map<DtoAgente>(repAgenteResultado.Dados);

            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<List<DtoAgente>>> ObtenhaLista()
        {

            var resultado = new ResultadoOperacao<List<DtoAgente>>();

            try
            {
                var repAgenteResultado = await repAgente.ObtenhaListaAsync();

                resultado.Dados = mapeador.Map<List<DtoAgente>>(repAgenteResultado.Dados);

                resultado.AdicionarPaginacao(1, repAgenteResultado?.Dados?.Count ?? 0, 100);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;

        }

        public async Task<ResultadoOperacao<List<DtoAgente>>> ObtenhaListaComFiltros(DtoFiltro filtro)
        {
            var resultado = new ResultadoOperacao<List<DtoAgente>>();

            try
            {
                var repAgenteResultado = await repAgente.ObtenhaListaComFiltrosAsync(filtro);

                resultado.Dados = mapeador.Map<List<DtoAgente>>(repAgenteResultado.Dados);


                var quantidadeItensEmExibicao = 1;

                if (repAgenteResultado?.Dados?.Count != null)
                    quantidadeItensEmExibicao = repAgenteResultado.Dados.Count;


                resultado.AdicionarPaginacao(1, quantidadeItensEmExibicao, 100);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<DtoAgente>> Cadastrar(DtoAgente dtoAgente)
        {
            var resultado = new ResultadoOperacao<DtoAgente>();

            try
            {
                var validacaoDto = DtoAgenteValidador.Cadastrar(dtoAgente);
                if (!validacaoDto.Sucesso)
                {
                    resultado.AdicionarErros(validacaoDto.Erros);
                    return resultado;
                }

                var validarCpfExistente = await repAgente.ObtenhaPorCnpjCpfAsync(dtoAgente.CnpjCpf);
                if (validarCpfExistente.Dados?.Codigo > 0)
                {
                    resultado.AdicionarErro("Já existe um agente com o CNPJ/CPF informado.");
                    return resultado;
                }

                var entidade = mapeador.Map<EntidadeAgente>(dtoAgente);
                var repResultado = await repAgente.CadastrarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoAgente>(repResultado.Dados);

            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<DtoAgente>> Atualizar(DtoAgente dtoAgente)
        {
            var resultado = new ResultadoOperacao<DtoAgente>();

            try
            {
                var validacaoDto = DtoAgenteValidador.Atualizar(dtoAgente);
                if (!validacaoDto.Sucesso)
                {
                    resultado.AdicionarErros(validacaoDto.Erros);
                    return resultado;
                }

                var entidade = mapeador.Map<EntidadeAgente>(dtoAgente);
                var repResultado = await repAgente.AtualizarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoAgente>(repResultado.Dados);

            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<bool>> Deletar(int codigo)
        {
            var resultado = new ResultadoOperacao<bool>();

            try
            {

                var validarSeClientePossuiVendas = await repVendas.ObtenhaListaAsync();
                if (validarSeClientePossuiVendas.Dados?.Count > 0)
                {
                    resultado.AdicionarErro("Não é possível deletar o agente, existem vendas vinculadas.");
                    return resultado;
                }

                var repAgenteResultado = await repAgente.DeletarAsync(codigo);
                resultado.Dados = repAgenteResultado.Dados;
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        #endregion
      
    }
}
