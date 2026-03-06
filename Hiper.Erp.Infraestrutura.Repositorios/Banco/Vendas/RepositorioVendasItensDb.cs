using Hiper.Erp.Aplicacao.Dtos.Filtros;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Vendas;
using Hiper.Erp.Dominio.Entidades.Vendas;
using Hiper.Erp.Infraestrutura.Bancos;
using Hiper.Erp.Infraestrutura.Repositorios.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Hiper.Erp.Infraestrutura.Repositorios.Vendas
{
    public class RepositorioVendasItensDb : RespositorioBase<EntidadeVendaItem>, IRepositorioVendasItens
    {
        public RepositorioVendasItensDb(RetaguardaDbContext contexto) : base(contexto)
        {
        }

        public async Task<ResultadoOperacao<EntidadeVendaItem>> ObtenhaPorCodigoAsync(int codigo)
        {
            var entidade = await contexto.VendasItens.FirstOrDefaultAsync(p => p.Codigo == codigo);
            return ResultadoOperacao<EntidadeVendaItem>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<List<EntidadeVendaItem>>> ObtenhaListaAsync()
        {
            var resultado = await contexto.VendasItens.ToListAsync();
            return ResultadoOperacao<List<EntidadeVendaItem>>.Ok(resultado);
        }

        public async Task<ResultadoOperacao<List<EntidadeVendaItem>>> ObtenhaListaComFiltrosAsync(DtoFiltro filtro)
        {
            var query = contexto.VendasItens.AsQueryable();
            query = query.AplicarFiltros(filtro);
            var total = await query.CountAsync();
            var dados = await query
                .Skip((filtro.Paginacao.PaginaAtual - 1) * filtro.Paginacao.QuantidadeItensPorPagina)
                .Take(filtro.Paginacao.QuantidadeItensPorPagina)
                .ToListAsync();
            return ResultadoOperacao<List<EntidadeVendaItem>>.Ok(dados);
        }

        public async Task<ResultadoOperacao<EntidadeVendaItem>> CadastrarAsync(EntidadeVendaItem entidade)
        {
            await contexto.VendasItens.AddAsync(entidade);
            await contexto.SaveChangesAsync();
            return ResultadoOperacao<EntidadeVendaItem>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<EntidadeVendaItem>> AtualizarAsync(EntidadeVendaItem entidade)
        {
            contexto.VendasItens.Update(entidade);
            await contexto.SaveChangesAsync();
            return ResultadoOperacao<EntidadeVendaItem>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<bool>> DeletarAsync(int codigo)
        {
            var entidade = await contexto.VendasItens.FirstOrDefaultAsync(p => p.Codigo == codigo);
            if (entidade != null)
            {
                contexto.VendasItens.Remove(entidade);
                await contexto.SaveChangesAsync();
            }
            return ResultadoOperacao<bool>.Ok(true);
        }
    }
}
