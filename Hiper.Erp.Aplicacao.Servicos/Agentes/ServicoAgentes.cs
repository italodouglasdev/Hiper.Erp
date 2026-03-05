using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.Agentes;
using Hiper.Erp.Aplicacao.Dtos.Filtros;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Agentes;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Agentes;
using Hiper.Erp.Dominio.Entidades.Agentes;

namespace Hiper.Erp.Aplicacao.Servicos.Agentes
{
    public class ServicoAgentes : ServicoBase, IServicoAgentes
    {
        #region Propriedades

        private IRepositorioAgentes repAgente;

        #endregion

        #region Construtores

        public ServicoAgentes(IMapper mapper, IRepositorioAgentes repAgente) : base(mapper)
        {
            this.repAgente = repAgente;
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
                resultado.AdicionarErro("Erro inesperado.");
            }
            finally
            {

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
                resultado.AdicionarErro("Erro inesperado.");
            }
            finally
            {

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

                resultado.AdicionarPaginacao(1, repAgenteResultado.Dados.Count, 100);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro("Erro inesperado.");
            }
            finally
            {
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
                resultado.AdicionarErro("Erro inesperado.");
            }
            finally
            {
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<DtoAgente>> Cadastrar(DtoAgente dtoAgente)
        {
            var resultado = new ResultadoOperacao<DtoAgente>();


            try
            {

                // 1. CHAMA SUA CAMADA DE VALIDAÇÃO PERSONALIZADA
                //var validacaoDto = DtoAgenteValidador.ValidarCadastrar(dtoAgente);
                //if (!validacaoDto.Sucesso)
                //{
                //    resultado.AdicionarErros(validacaoDto.Erros);
                //    return resultado;
                //}


                // 2. VALIDAÇÃO DE INFRA (Regras que precisam de Banco)
                //if (await repAgente.ExisteCpf(dtoAgente.Cpf))
                //{
                //    resultado.AdicionarErro("Este CPF já está cadastrado.");
                //    return resultado;
                //}


                // 3. MAPEAMENTO
                var entidade = mapeador.Map<EntidadeAgente>(dtoAgente);


                // 4. VALIDAÇÃO DE DOMÍNIO (A entidade se auto-valida)
                //var validacaoEntidade = entidade.ValidarRegrasDeNegocioCadastrar();
                //if (!validacaoEntidade.Sucesso)
                //{
                //    resultado.AdicionarErros(validacaoEntidade.Erros);
                //    return resultado;
                //}

                // 5. PERSISTÊNCIA
                var repResultado = await repAgente.CadastrarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoAgente>(repResultado.Dados);
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro("Erro interno.");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<DtoAgente>> Atualizar(DtoAgente dtoAgente)
        {
            var resultado = new ResultadoOperacao<DtoAgente>();

            try
            {
                // 1. CHAMA SUA CAMADA DE VALIDAÇÃO PERSONALIZADA
                //var validacaoDto = DtoAgenteValidador.ValidarAtualizar(dtoAgente);
                //if (!validacaoDto.Sucesso)
                //{
                //    resultado.AdicionarErros(validacaoDto.Erros);
                //    return resultado;
                //}


                // 2. VALIDAÇÃO DE INFRA (Regras que precisam de Banco)
                //if (await repAgente.ExisteCpf(dtoAgente.Cpf))
                //{
                //    resultado.AdicionarErro("Este CPF já está cadastrado.");
                //    return resultado;
                //}


                // 3. MAPEAMENTO
                var entidade = mapeador.Map<EntidadeAgente>(dtoAgente);


                // 4. VALIDAÇÃO DE DOMÍNIO (A entidade se auto-valida)
                //var validacaoEntidade = entidade.ValidarRegrasDeNegocioAtualizar();
                //if (!validacaoEntidade.Sucesso)
                //{
                //    resultado.AdicionarErros(validacaoEntidade.Erros);
                //    return resultado;
                //}

                // 5. PERSISTÊNCIA
                var repResultado = await repAgente.AtualizarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoAgente>(repResultado.Dados);

            }
            catch (Exception ex)
            {
                resultado.AdicionarErro("Erro inesperado ao atualizar.");
            }

            return resultado;
        }

        public async Task<ResultadoOperacao<bool>> Deletar(int codigo)
        {
            var resultado = new ResultadoOperacao<bool>();

            try
            {
                var repAgenteResultado = await repAgente.DeletarAsync(codigo);

                resultado.Dados = repAgenteResultado.Dados;
            }
            catch (Exception ex)
            {
                resultado.AdicionarErro("Erro inesperado ao deletar.");
            }

            return resultado;
        }

        #endregion

        #region Métodos Privados
        //Nada ainda
        #endregion
    }
}
