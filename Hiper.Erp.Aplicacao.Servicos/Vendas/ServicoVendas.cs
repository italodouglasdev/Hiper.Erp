using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.Filtros;
using Hiper.Erp.Aplicacao.Dtos.Vendas;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Vendas;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Vendas;
using Hiper.Erp.Dominio.Entidades.Vendas;

namespace Hiper.Erp.Aplicacao.Servicos.Vendas
{
    public class ServicoVendas : ServicoBase, IServicoVendas
    {
        private IRepositorioVendas repVenda;

        public ServicoVendas(IMapper mapper, IRepositorioVendas repVenda) : base(mapper)
        {
            this.repVenda = repVenda;
        }

        public async Task<ResultadoOperacao<DtoVenda>> ObtenhaPorCodigo(int codigo)
        {
            var resultado = new ResultadoOperacao<DtoVenda>();
            try
            {
                var repResultado = await repVenda.ObtenhaPorCodigoAsync(codigo);
                resultado.Dados = mapeador.Map<DtoVenda>(repResultado.Dados);
            }
            catch (Exception)
            {
                resultado.AdicionarErro("Erro inesperado.");
            }
            return resultado;
        }

        public async Task<ResultadoOperacao<List<DtoVenda>>> ObtenhaLista()
        {
            var resultado = new ResultadoOperacao<List<DtoVenda>>();
            try
            {
                var repResultado = await repVenda.ObtenhaListaAsync();
                resultado.Dados = mapeador.Map<List<DtoVenda>>(repResultado.Dados);
                resultado.AdicionarPaginacao(1, repResultado.Dados.Count, 100);
            }
            catch (Exception)
            {
                resultado.AdicionarErro("Erro inesperado.");
            }
            return resultado;
        }

        public async Task<ResultadoOperacao<List<DtoVenda>>> ObtenhaListaComFiltros(DtoFiltro filtro)
        {
            var resultado = new ResultadoOperacao<List<DtoVenda>>();
            try
            {
                var repResultado = await repVenda.ObtenhaListaComFiltrosAsync(filtro);
                resultado.Dados = mapeador.Map<List<DtoVenda>>(repResultado.Dados);
                resultado.AdicionarPaginacao(1, repResultado.Dados?.Count ?? 0, 100);
            }
            catch (Exception)
            {
                resultado.AdicionarErro("Erro inesperado.");
            }
            return resultado;
        }

        public async Task<ResultadoOperacao<DtoVenda>> Cadastrar(DtoVenda dtoVenda)
        {
            var resultado = new ResultadoOperacao<DtoVenda>();
            try
            {
                var entidade = mapeador.Map<EntidadeVenda>(dtoVenda);
                var repResultado = await repVenda.CadastrarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoVenda>(repResultado.Dados);
            }
            catch (Exception)
            {
                resultado.AdicionarErro("Erro interno.");
            }
            return resultado;
        }

        public async Task<ResultadoOperacao<DtoVenda>> Atualizar(DtoVenda dtoVenda)
        {
            var resultado = new ResultadoOperacao<DtoVenda>();
            try
            {
                var entidade = mapeador.Map<EntidadeVenda>(dtoVenda);
                var repResultado = await repVenda.AtualizarAsync(entidade);
                resultado.Dados = mapeador.Map<DtoVenda>(repResultado.Dados);
            }
            catch (Exception)
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
                var repResultado = await repVenda.DeletarAsync(codigo);
                resultado.Dados = repResultado.Dados;
            }
            catch (Exception)
            {
                resultado.AdicionarErro("Erro inesperado ao deletar.");
            }
            return resultado;
        }
    }
}
