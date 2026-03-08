using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Vendas;
using Hiper.Erp.Dominio.Entidades.Vendas;
using Hiper.Erp.Infraestrutura.Bancos;
using Hiper.Erp.Infraestrutura.Repositorios.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Hiper.Erp.Infraestrutura.Repositorios.Vendas
{
    public class RepositorioVendasDb : RespositorioBase<EntidadeVenda>, IRepositorioVendas
    {
        public RepositorioVendasDb(RetaguardaDbContext contexto) : base(contexto)
        {
        }

        public async Task<ResultadoOperacao<EntidadeVenda>> ObtenhaPorCodigoAsync(int codigo)
        {
            var entidade = await contexto.Vendas.FirstOrDefaultAsync(p => p.Codigo == codigo);
            return ResultadoOperacao<EntidadeVenda>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<List<EntidadeVenda>>> ObtenhaListaAsync()
        {
            var resultado = await contexto.Vendas.ToListAsync();
            return ResultadoOperacao<List<EntidadeVenda>>.Ok(resultado);
        }

        public async Task<ResultadoOperacao<List<EntidadeVenda>>> ObtenhaListaComFiltrosAsync(DtoFiltro filtro)
        {
            var query = contexto.Vendas.AsQueryable();
            query = query.AplicarFiltros(filtro);
            var total = await query.CountAsync();
            var dados = await query
                .Skip((filtro.Paginacao.PaginaAtual - 1) * filtro.Paginacao.QuantidadeItensPorPagina)
                .Take(filtro.Paginacao.QuantidadeItensPorPagina)
                .ToListAsync();
            return ResultadoOperacao<List<EntidadeVenda>>.Ok(dados);
        }

        public async Task<ResultadoOperacao<EntidadeVenda>> CadastrarAsync(EntidadeVenda entidade)
        {
            await contexto.Vendas.AddAsync(entidade);
            await contexto.SaveChangesAsync();
            return ResultadoOperacao<EntidadeVenda>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<EntidadeVenda>> AtualizarAsync(EntidadeVenda entidade)
        {
            contexto.Vendas.Update(entidade);
            await contexto.SaveChangesAsync();
            return ResultadoOperacao<EntidadeVenda>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<bool>> DeletarAsync(int codigo)
        {
            var entidade = await contexto.Vendas.FirstOrDefaultAsync(p => p.Codigo == codigo);
            if (entidade != null)
            {
                contexto.Vendas.Remove(entidade);
                await contexto.SaveChangesAsync();
            }
            return ResultadoOperacao<bool>.Ok(true);
        }
      
    }
}
