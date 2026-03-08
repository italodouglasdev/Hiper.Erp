using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.FormasPagamentos;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.FormasPagamentos;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.FormasPagamentos;
using Hiper.Erp.Aplicacao.Validadores.FormasPagamentos;
using Hiper.Erp.Dominio.Entidades.FormasPagamentos;

namespace Hiper.Erp.Aplicacao.Servicos.FormasPagamentos
{
    public class ServicoFormasPagamentos : ServicoBase, IServicoFormasPagamentos
    {
        #region Propriedades

        private IRepositorioFormasPagamentos repFormaPagamento;

        #endregion

        #region Construtores

        public ServicoFormasPagamentos(IMapper mapper, IRepositorioFormasPagamentos repFormaPagamento) : base(mapper)
        {
            this.repFormaPagamento = repFormaPagamento;
        }

        #endregion

        #region Métodos Públicos

        public async Task<ResultadoOperacao<DtoFormaPagamento>> ObtenhaPorCodigo(int codigo)
        {
            var resultado = new ResultadoOperacao<DtoFormaPagamento>();

            try
            {
                var repResultado = await repFormaPagamento.ObtenhaPorCodigoAsync(codigo);
                resultado.Dados = mapeador.Map<DtoFormaPagamento>(repResultado.Dados);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<List<DtoFormaPagamento>>> ObtenhaLista()
        {
            var resultado = new ResultadoOperacao<List<DtoFormaPagamento>>();

            try
            {
                var repResultado = await repFormaPagamento.ObtenhaListaAsync();
                resultado.Dados = mapeador.Map<List<DtoFormaPagamento>>(repResultado.Dados);
                resultado.AdicionarPaginacao(1, repResultado.Dados.Count, 100);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<List<DtoFormaPagamento>>> ObtenhaListaComFiltros(DtoFiltro filtro)
        {
            var resultado = new ResultadoOperacao<List<DtoFormaPagamento>>();

            try
            {
                var repResultado = await repFormaPagamento.ObtenhaListaComFiltrosAsync(filtro);
                resultado.Dados = mapeador.Map<List<DtoFormaPagamento>>(repResultado.Dados);
                resultado.AdicionarPaginacao(1, repResultado.Dados?.Count ?? 0, 100);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<DtoFormaPagamento>> Cadastrar(DtoFormaPagamento dtoFormaPagamento)
        {
            var resultado = new ResultadoOperacao<DtoFormaPagamento>();

            try
            {
                var validacaoDto = DtoFormaPagamentoValidador.Cadastrar(dtoFormaPagamento);
                if (!validacaoDto.Sucesso)
                {
                    resultado.AdicionarErros(validacaoDto.Erros);
                    return resultado;
                }

                var entidade = mapeador.Map<EntidadeFormaPagamento>(dtoFormaPagamento);
                var repResultado = await repFormaPagamento.CadastrarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoFormaPagamento>(repResultado.Dados);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro($"Erro fatal. Detalhes: {ex.Message}");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<DtoFormaPagamento>> Atualizar(DtoFormaPagamento dtoFormaPagamento)
        {
            var resultado = new ResultadoOperacao<DtoFormaPagamento>();

            try
            {
                var validacaoDto = DtoFormaPagamentoValidador.Atualizar(dtoFormaPagamento);
                if (!validacaoDto.Sucesso)
                {
                    resultado.AdicionarErros(validacaoDto.Erros);
                    return resultado;
                }

                var entidade = mapeador.Map<EntidadeFormaPagamento>(dtoFormaPagamento);
                var repResultado = await repFormaPagamento.AtualizarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoFormaPagamento>(repResultado.Dados);
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
                var repResultado = await repFormaPagamento.DeletarAsync(codigo);
                resultado.Dados = repResultado.Dados;
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
