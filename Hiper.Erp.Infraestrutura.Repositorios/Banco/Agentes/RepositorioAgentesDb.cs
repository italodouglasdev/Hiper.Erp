using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Agentes;
using Hiper.Erp.Dominio.Entidades.Agentes;
using Hiper.Erp.Infraestrutura.Bancos;
using Hiper.Erp.Infraestrutura.Repositorios.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Hiper.Erp.Infraestrutura.Repositorios.Agentes
{
    public class RepositorioAgentesDb : RespositorioBase<EntidadeAgente>, IRepositorioAgentes
    {
        public RepositorioAgentesDb(RetaguardaDbContext contexto) : base(contexto)
        {
        }

        public async Task<ResultadoOperacao<EntidadeAgente>> ObtenhaPorCodigoAsync(int codigo)
        {
            var entidade = await contexto.Agentes.FirstOrDefaultAsync(p => p.Codigo == codigo);

            return ResultadoOperacao<EntidadeAgente>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<EntidadeAgente>> ObtenhaPorCnpjCpfAsync(string cnpjCpf)
        {
            var entidade = await contexto.Agentes.FirstOrDefaultAsync(p => p.CnpjCpf == cnpjCpf);

            return ResultadoOperacao<EntidadeAgente>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<List<EntidadeAgente>>> ObtenhaListaAsync()
        {
            var resultado = await contexto.Agentes.ToListAsync();
            return ResultadoOperacao<List<EntidadeAgente>>.Ok(resultado);
        }

        public async Task<ResultadoOperacao<List<EntidadeAgente>>> ObtenhaListaComFiltrosAsync(DtoFiltro filtro)
        {
            var query = contexto.Agentes.AsQueryable();

            query = query.AplicarFiltros(filtro);

            var total = await query.CountAsync();

            var dados = await query
                .Skip((filtro.Paginacao.PaginaAtual - 1) * filtro.Paginacao.QuantidadeItensPorPagina)
                .Take(filtro.Paginacao.QuantidadeItensPorPagina)
                .ToListAsync();

            return ResultadoOperacao<List<EntidadeAgente>>.Ok(dados);
        }


        public async Task<ResultadoOperacao<EntidadeAgente>> CadastrarAsync(EntidadeAgente entidade)
        {
            await contexto.Agentes.AddAsync(entidade);
            await contexto.SaveChangesAsync();
            return ResultadoOperacao<EntidadeAgente>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<EntidadeAgente>> AtualizarAsync(EntidadeAgente entidade)
        {
            contexto.Agentes.Update(entidade);
            await contexto.SaveChangesAsync();
            return ResultadoOperacao<EntidadeAgente>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<bool>> DeletarAsync(int codigo)
        {
            var entidade = await contexto.Agentes.FirstOrDefaultAsync(p => p.Codigo == codigo);
            if (entidade != null)
            {
                contexto.Agentes.Remove(entidade);
                await contexto.SaveChangesAsync();
            }
            return ResultadoOperacao<bool>.Ok(true);
        }


    }
}
