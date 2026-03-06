using Hiper.Erp.Dominio.Enumeradores;
using Microsoft.EntityFrameworkCore;

namespace Hiper.Erp.Infraestrutura.Bancos.SGDBs.Fabricas
{
    public class FabricaConexoes
    {
        public static RetaguardaDbContext CriarContexto(EnumTipoSgdb tipoSgdb, string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RetaguardaDbContext>();

            switch (tipoSgdb)
            {
                case EnumTipoSgdb.SQLServer:
                    optionsBuilder.UseSqlServer(connectionString);
                    break;

                case EnumTipoSgdb.PostgreSQL:
                    optionsBuilder.UseNpgsql(connectionString);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(tipoSgdb), tipoSgdb, "Tipo de SGBD não suportado");
            }

            return new RetaguardaDbContext(optionsBuilder.Options);
        }


    }
}
