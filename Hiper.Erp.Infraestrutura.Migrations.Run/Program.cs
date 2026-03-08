using Hiper.Erp.Dominio.Enumeradores;
using Hiper.Erp.Infraestrutura.Migrations.Run.Scripts;

namespace Hiper.Erp.Infraestrutura.Migrations.Run
{
    public class Program
    {
        static void Main(string[] args)
        {
            GerarScriptRetaguarda();
        }

        private static void GerarScriptRetaguarda()
        {
            // Configurar
            var tipoSgdb = EnumTipoSgdb.SQLServer;           
            var diretorioSaida = @"C:\Hiper.Erp\Scripts";

            // Criar serviço
            var servico = new ServicoGeradorScripts(tipoSgdb, "");

            // Gerar scripts
            var arquivos = servico.GerarTodosScripts(diretorioSaida);

            Console.WriteLine($"Scripts gerados: {arquivos.Count}");
        }
    }
}
