using Hiper.Erp.Dominio.Entidades.Agentes;
using Hiper.Erp.Dominio.Entidades.FormasPagamentos;
using Hiper.Erp.Dominio.Entidades.Produtos;
using Hiper.Erp.Dominio.Entidades.Vendas;
using Microsoft.EntityFrameworkCore;

namespace Hiper.Erp.Infraestrutura.Bancos
{
    public class RetaguardaDbContext : DbContext
    {
        public DbSet<EntidadeAgente> Agentes { get; set; }
        public DbSet<EntidadeProduto> Produtos { get; set; }
        public DbSet<EntidadeFormaPagamento> FormasPagamentos { get; set; }
        public DbSet<EntidadeVenda> Vendas { get; set; }
        public DbSet<EntidadeVendaItem> VendasItens { get; set; }


        public RetaguardaDbContext(DbContextOptions<RetaguardaDbContext> options) : base(options)
        {
        }

    }
}
