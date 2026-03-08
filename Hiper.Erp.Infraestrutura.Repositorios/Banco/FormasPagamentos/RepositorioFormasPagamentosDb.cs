using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.FormasPagamentos;
using Hiper.Erp.Dominio.Entidades.FormasPagamentos;
using Hiper.Erp.Infraestrutura.Bancos;
using Hiper.Erp.Infraestrutura.Repositorios.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Hiper.Erp.Infraestrutura.Repositorios.FormasPagamentos
{
    public class RepositorioFormasPagamentosDb : RespositorioBase<EntidadeFormaPagamento>, IRepositorioFormasPagamentos
    {
        public RepositorioFormasPagamentosDb(RetaguardaDbContext contexto) : base(contexto)
        {
        }

        public async Task<ResultadoOperacao<EntidadeFormaPagamento>> ObtenhaPorCodigoAsync(int codigo)
        {
            var entidade = await contexto.FormasPagamentos.FirstOrDefaultAsync(p => p.Codigo == codigo);
            return ResultadoOperacao<EntidadeFormaPagamento>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<List<EntidadeFormaPagamento>>> ObtenhaListaAsync()
        {
            var resultado = await contexto.FormasPagamentos.ToListAsync();
            return ResultadoOperacao<List<EntidadeFormaPagamento>>.Ok(resultado);
        }

        public async Task<ResultadoOperacao<List<EntidadeFormaPagamento>>> ObtenhaListaComFiltrosAsync(DtoFiltro filtro)
        {
            var query = contexto.FormasPagamentos.AsQueryable();
            query = query.AplicarFiltros(filtro);
            var total = await query.CountAsync();
            var dados = await query
                .Skip((filtro.Paginacao.PaginaAtual - 1) * filtro.Paginacao.QuantidadeItensPorPagina)
                .Take(filtro.Paginacao.QuantidadeItensPorPagina)
                .ToListAsync();
            return ResultadoOperacao<List<EntidadeFormaPagamento>>.Ok(dados);
        }

        public async Task<ResultadoOperacao<EntidadeFormaPagamento>> CadastrarAsync(EntidadeFormaPagamento entidade)
        {
            await contexto.FormasPagamentos.AddAsync(entidade);
            await contexto.SaveChangesAsync();
            return ResultadoOperacao<EntidadeFormaPagamento>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<EntidadeFormaPagamento>> AtualizarAsync(EntidadeFormaPagamento entidade)
        {
            contexto.FormasPagamentos.Update(entidade);
            await contexto.SaveChangesAsync();
            return ResultadoOperacao<EntidadeFormaPagamento>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<bool>> DeletarAsync(int codigo)
        {
            var entidade = await contexto.FormasPagamentos.FirstOrDefaultAsync(p => p.Codigo == codigo);
            if (entidade != null)
            {
                contexto.FormasPagamentos.Remove(entidade);
                await contexto.SaveChangesAsync();
            }
            return ResultadoOperacao<bool>.Ok(true);
        }
    }
}
