using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Produtos;
using Hiper.Erp.Dominio.Entidades.Produtos;
using Hiper.Erp.Infraestrutura.Bancos;
using Hiper.Erp.Infraestrutura.Repositorios.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Hiper.Erp.Infraestrutura.Repositorios.Produtos
{
    public class RepositorioProdutosDb : RespositorioBase<EntidadeProduto>, IRepositorioProdutos
    {
        public RepositorioProdutosDb(RetaguardaDbContext contexto) : base(contexto)
        {
        }

        public async Task<ResultadoOperacao<EntidadeProduto>> ObtenhaPorCodigoAsync(int codigo)
        {
            var entidade = await contexto.Produtos.FirstOrDefaultAsync(p => p.Codigo == codigo);
            return ResultadoOperacao<EntidadeProduto>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<List<EntidadeProduto>>> ObtenhaListaAsync()
        {
            var resultado = await contexto.Produtos.ToListAsync();
            return ResultadoOperacao<List<EntidadeProduto>>.Ok(resultado);
        }

        public async Task<ResultadoOperacao<List<EntidadeProduto>>> ObtenhaListaComFiltrosAsync(DtoFiltro filtro)
        {
            var query = contexto.Produtos.AsQueryable();
            query = query.AplicarFiltros(filtro);
            var total = await query.CountAsync();
            var dados = await query
                .Skip((filtro.Paginacao.PaginaAtual - 1) * filtro.Paginacao.QuantidadeItensPorPagina)
                .Take(filtro.Paginacao.QuantidadeItensPorPagina)
                .ToListAsync();
            return ResultadoOperacao<List<EntidadeProduto>>.Ok(dados);
        }

        public async Task<ResultadoOperacao<EntidadeProduto>> CadastrarAsync(EntidadeProduto entidade)
        {
            await contexto.Produtos.AddAsync(entidade);
            await contexto.SaveChangesAsync();
            return ResultadoOperacao<EntidadeProduto>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<EntidadeProduto>> AtualizarAsync(EntidadeProduto entidade)
        {
            contexto.Produtos.Update(entidade);
            await contexto.SaveChangesAsync();
            return ResultadoOperacao<EntidadeProduto>.Ok(entidade);
        }

        public async Task<ResultadoOperacao<bool>> DeletarAsync(int codigo)
        {
            var entidade = await contexto.Produtos.FirstOrDefaultAsync(p => p.Codigo == codigo);
            if (entidade != null)
            {
                contexto.Produtos.Remove(entidade);
                await contexto.SaveChangesAsync();
            }
            return ResultadoOperacao<bool>.Ok(true);
        }
    }
}
