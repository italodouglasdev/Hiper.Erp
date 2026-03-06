using Hiper.Erp.Dominio.Enumeradores;
using Hiper.Erp.Infraestrutura.Bancos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiper.Erp.Infraestrutura.Migrations.Run.Scripts
{
    /// <summary>
    /// Serviço para gerar scripts de todos os repositórios
    /// </summary>
    public class ServicoGeradorScripts
    {
        private readonly EnumTipoSgdb _tipoSgdb;
        private readonly string _stringConexao;

        public ServicoGeradorScripts(EnumTipoSgdb tipoSgdb, string stringConexao)
        {
            _tipoSgdb = tipoSgdb;
            _stringConexao = stringConexao;
        }

        /// <summary>
        /// Gera scripts de todas as versões e salva em arquivos
        /// </summary>
        /// <param name="diretorioSaida">Diretório onde os scripts serão salvos</param>
        /// <returns>Lista de arquivos gerados</returns>
        public List<string> GerarTodosScripts(string diretorioSaida)
        {
            var arquivosGerados = new List<string>();

            // Cria diretório se não existir
            if (!Directory.Exists(diretorioSaida))
                Directory.CreateDirectory(diretorioSaida);

            // Cria contexto
            var context = CriarContexto();

            // Cria gerador
            var gerador = new GeradorScriptVersao(context);

            // Gera scripts por versão
            var scriptsPorVersao = gerador.GerarScriptsPorVersao();

            // Salva cada script em um arquivo
            foreach (var kvp in scriptsPorVersao.OrderBy(x => x.Key))
            {
                var versao = kvp.Key;
                var script = kvp.Value;

                var nomeArquivo = $"Script_Versao_{versao}.sql";
                var caminhoCompleto = Path.Combine(diretorioSaida, nomeArquivo);

                File.WriteAllText(caminhoCompleto, script);
                arquivosGerados.Add(caminhoCompleto);

                Console.WriteLine($"✓ Script gerado: {nomeArquivo}");
            }

            // Gera também o script completo
            var scriptCompleto = gerador.GerarScriptCompleto();
            var arquivoCompleto = Path.Combine(diretorioSaida, "Script_Completo.sql");
            File.WriteAllText(arquivoCompleto, scriptCompleto);
            arquivosGerados.Add(arquivoCompleto);
            Console.WriteLine($"✓ Script completo gerado: Script_Completo.sql");

            // Gera arquivo de índice
            GerarArquivoIndice(diretorioSaida, scriptsPorVersao.Keys.ToList());

            return arquivosGerados;
        }

        /// <summary>
        /// Gera relatório com informações sobre as versões
        /// </summary>
        public string GerarRelatorioVersoes()
        {
            var context = CriarContexto();
            var gerador = new GeradorScriptVersao(context);
            var scriptsPorVersao = gerador.GerarScriptsPorVersao();

            var relatorio = new System.Text.StringBuilder();
            relatorio.AppendLine("=".PadRight(80, '='));
            relatorio.AppendLine("RELATÓRIO DE VERSÕES DO BANCO DE DADOS");
            relatorio.AppendLine("=".PadRight(80, '='));
            relatorio.AppendLine();
            relatorio.AppendLine($"Data de Geração: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            relatorio.AppendLine($"Tipo de Banco: {_tipoSgdb}");
            relatorio.AppendLine();
            relatorio.AppendLine($"Total de Versões: {scriptsPorVersao.Count}");
            relatorio.AppendLine();

            foreach (var versao in scriptsPorVersao.Keys.OrderBy(v => v))
            {
                var script = scriptsPorVersao[versao];
                var linhas = script.Split('\n').Length;
                var tamanho = script.Length;

                relatorio.AppendLine($"Versão {versao}:");
                relatorio.AppendLine($"  - Linhas: {linhas}");
                relatorio.AppendLine($"  - Tamanho: {tamanho} bytes");
                relatorio.AppendLine();
            }

            return relatorio.ToString();
        }

        /// <summary>
        /// Gera arquivo de índice com informações sobre os scripts
        /// </summary>
        private void GerarArquivoIndice(string diretorioSaida, List<EnumVersao> versoes)
        {
            var indice = new System.Text.StringBuilder();

            indice.AppendLine("# Índice de Scripts de Banco de Dados");
            indice.AppendLine();
            indice.AppendLine($"**Data de Geração:** {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            indice.AppendLine($"**Tipo de Banco:** {_tipoSgdb}");
            indice.AppendLine();
            indice.AppendLine("## Scripts Disponíveis");
            indice.AppendLine();
            indice.AppendLine("### Scripts por Versão");
            indice.AppendLine();

            foreach (var versao in versoes.OrderBy(v => v))
            {
                indice.AppendLine($"- **Versão {versao}**: `Script_Versao_{versao}.sql`");
            }

            indice.AppendLine();
            indice.AppendLine("### Script Completo");
            indice.AppendLine();
            indice.AppendLine("- **Todas as Versões**: `Script_Completo.sql`");
            indice.AppendLine();
            indice.AppendLine("## Como Usar");
            indice.AppendLine();
            indice.AppendLine("1. Execute os scripts na ordem das versões (V1_0, V1_1, V1_2, etc.)");
            indice.AppendLine("2. Ou execute o `Script_Completo.sql` para criar tudo de uma vez");
            indice.AppendLine();
            indice.AppendLine("## Observações");
            indice.AppendLine();
            indice.AppendLine("- Scripts de versões incrementais contêm apenas as alterações daquela versão");
            indice.AppendLine("- O script completo contém todas as tabelas e campos de todas as versões");
            indice.AppendLine("- Sempre faça backup do banco antes de executar scripts de atualização");

            var caminhoIndice = Path.Combine(diretorioSaida, "README.md");
            File.WriteAllText(caminhoIndice, indice.ToString());
            Console.WriteLine($"✓ Índice gerado: README.md");
        }

        /// <summary>
        /// Cria contexto do banco de dados
        /// </summary>
        private RetaguardaDbContext CriarContexto()
        {
            var optionsBuilder = new DbContextOptionsBuilder<RetaguardaDbContext>();

            switch (_tipoSgdb)
            {
                case EnumTipoSgdb.SQLServer:
                    optionsBuilder.UseSqlServer(_stringConexao);
                    break;

                case EnumTipoSgdb.PostgreSQL:
                    optionsBuilder.UseNpgsql(_stringConexao);
                    break;

                default:
                    throw new NotSupportedException($"Tipo de banco {_tipoSgdb} não suportado");
            }

            return new RetaguardaDbContext(optionsBuilder.Options);
        }

        /// <summary>
        /// Gera scripts e retorna como dicionário (sem salvar em arquivo)
        /// </summary>
        public Dictionary<EnumVersao, string> GerarScriptsPorVersao()
        {
            var context = CriarContexto();
            var gerador = new GeradorScriptVersao(context);
            return gerador.GerarScriptsPorVersao();
        }

        /// <summary>
        /// Gera script completo (sem salvar em arquivo)
        /// </summary>
        public string GerarScriptCompleto()
        {
            var context = CriarContexto();
            var gerador = new GeradorScriptVersao(context);
            return gerador.GerarScriptCompleto();
        }
    }
}
