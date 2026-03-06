using Hiper.Erp.Dominio.Atributos;
using Hiper.Erp.Dominio.Enumeradores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace Hiper.Erp.Infraestrutura.Migrations.Run.Scripts
{
    /// <summary>
    /// Gerador de scripts SQL por versão
    /// </summary>
    public class GeradorScriptVersao
    {
        private readonly DbContext _context;
        private readonly IRelationalDatabaseCreator _databaseCreator;

        public GeradorScriptVersao(DbContext context)
        {
            _context = context;
            _databaseCreator = (RelationalDatabaseCreator)context.Database.GetService<IRelationalDatabaseCreator>();
        }

        /// <summary>
        /// Gera scripts SQL separados por versão
        /// </summary>
        /// <returns>Dicionário com versão e script correspondente</returns>
        public Dictionary<EnumVersao, string> GerarScriptsPorVersao()
        {
            var scripts = new Dictionary<EnumVersao, string>();

            // Obtém todas as entidades do DbContext
            var entidades = ObterEntidadesDoContexto();

            // Agrupa entidades por versão
            var entidadesPorVersao = AgruparEntidadesPorVersao(entidades);

            // Gera script para cada versão
            foreach (var versao in entidadesPorVersao.Keys.OrderBy(v => v))
            {
                var scriptVersao = GerarScriptParaVersao(versao, entidadesPorVersao[versao], entidadesPorVersao);
                scripts[versao] = scriptVersao;
            }

            return scripts;
        }

        /// <summary>
        /// Obtém todas as entidades mapeadas no DbContext
        /// </summary>
        private List<Type> ObterEntidadesDoContexto()
        {
            var entidades = new List<Type>();

            var dbSetProperties = _context.GetType()
                .GetProperties()
                .Where(p => p.PropertyType.IsGenericType &&
                           p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            foreach (var prop in dbSetProperties)
            {
                var entityType = prop.PropertyType.GetGenericArguments()[0];
                entidades.Add(entityType);
            }

            return entidades;
        }

        /// <summary>
        /// Agrupa entidades por versão
        /// </summary>
        private Dictionary<EnumVersao, List<Type>> AgruparEntidadesPorVersao(List<Type> entidades)
        {
            var agrupamento = new Dictionary<EnumVersao, List<Type>>();

            foreach (var entidade in entidades)
            {
                var atributoVersao = entidade.GetCustomAttribute<VersaoTabelaAttribute>();
                var versao = atributoVersao?.Versao ?? EnumVersao.V1_0;

                if (!agrupamento.ContainsKey(versao))
                    agrupamento[versao] = new List<Type>();

                agrupamento[versao].Add(entidade);
            }

            return agrupamento;
        }

        /// <summary>
        /// Gera script SQL para uma versão específica
        /// </summary>
        private string GerarScriptParaVersao(EnumVersao versao, List<Type> entidades,
            Dictionary<EnumVersao, List<Type>> todasEntidades)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"-- ============================================");
            sb.AppendLine($"-- Script de Banco de Dados - Versão {versao}");
            sb.AppendLine($"-- Data de Geração: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine($"-- ============================================");
            sb.AppendLine();

            // Gera CREATE TABLE para tabelas novas desta versão
            foreach (var entidade in entidades)
            {
                sb.AppendLine(GerarCreateTable(entidade, versao));
                sb.AppendLine();
            }

            // Gera ALTER TABLE para campos adicionados em tabelas de versões anteriores
            var alterTables = GerarAlterTablesParaCamposNovos(versao, todasEntidades);
            if (!string.IsNullOrEmpty(alterTables))
            {
                sb.AppendLine("-- Adição de novos campos em tabelas existentes");
                sb.AppendLine(alterTables);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gera script CREATE TABLE para uma entidade
        /// </summary>
        private string GerarCreateTable(Type entidade, EnumVersao versaoTabela)
        {
            var sb = new StringBuilder();

            var tableAttr = entidade.GetCustomAttribute<TableAttribute>();
            var nomeTabela = tableAttr?.Name ?? entidade.Name;

            var atributoVersao = entidade.GetCustomAttribute<VersaoTabelaAttribute>();
            var descricao = atributoVersao?.Descricao ?? "Criação da tabela";

            sb.AppendLine($"-- {descricao}");
            sb.AppendLine($"CREATE TABLE {nomeTabela} (");

            var propriedades = entidade.GetProperties()
                .Where(p => p.GetCustomAttribute<NotMappedAttribute>() == null)
                .ToList();

            var colunas = new List<string>();

            foreach (var prop in propriedades)
            {
                var versaoCampo = prop.GetCustomAttribute<VersaoCampoAttribute>();

                // Só inclui campos da mesma versão da tabela ou anteriores
                if (versaoCampo != null && versaoCampo.Versao > versaoTabela)
                    continue;

                var coluna = GerarDefinicaoColuna(prop);
                colunas.Add(coluna);
            }

            sb.AppendLine("    " + string.Join(",\n    ", colunas));
            sb.AppendLine(");");

            return sb.ToString();
        }

        /// <summary>
        /// Gera ALTER TABLE para campos adicionados em versões posteriores
        /// </summary>
        private string GerarAlterTablesParaCamposNovos(EnumVersao versaoAtual,
            Dictionary<EnumVersao, List<Type>> todasEntidades)
        {
            var sb = new StringBuilder();

            // Percorre todas as versões anteriores
            foreach (var versaoAnterior in todasEntidades.Keys.Where(v => v < versaoAtual).OrderBy(v => v))
            {
                foreach (var entidade in todasEntidades[versaoAnterior])
                {
                    var tableAttr = entidade.GetCustomAttribute<TableAttribute>();
                    var nomeTabela = tableAttr?.Name ?? entidade.Name;

                    var camposNovos = entidade.GetProperties()
                        .Where(p => p.GetCustomAttribute<NotMappedAttribute>() == null)
                        .Where(p => p.GetCustomAttribute<VersaoCampoAttribute>()?.Versao == versaoAtual)
                        .ToList();

                    if (camposNovos.Any())
                    {
                        sb.AppendLine($"-- Adicionando campos na tabela {nomeTabela}");

                        foreach (var campo in camposNovos)
                        {
                            var versaoCampo = campo.GetCustomAttribute<VersaoCampoAttribute>();
                            var descricao = versaoCampo?.Descricao ?? $"Adição do campo {campo.Name}";

                            sb.AppendLine($"-- {descricao}");
                            sb.AppendLine($"ALTER TABLE {nomeTabela}");
                            sb.AppendLine($"    ADD {GerarDefinicaoColuna(campo)};");
                            sb.AppendLine();
                        }
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gera definição de coluna SQL
        /// </summary>
        private string GerarDefinicaoColuna(PropertyInfo prop)
        {
            var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
            var nomeColuna = columnAttr?.Name ?? prop.Name;

            var tipoCSharp = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            var tipoSql = MapearTipoSql(tipoCSharp, columnAttr?.TypeName);

            var isNullable = IsNullable(prop);
            var nullConstraint = isNullable ? "NULL" : "NOT NULL";

            // Verifica se é chave primária
            var isKey = prop.GetCustomAttributes().Any(a =>
                a.GetType().Name == "KeyAttribute");

            if (isKey)
            {
                if (_context.Database.IsSqlServer())
                {
                    return $"{nomeColuna} {tipoSql} PRIMARY KEY IDENTITY(1,1)";
                }
                else if (_context.Database.IsNpgsql())
                {
                    // Para int, usar SERIAL
                    if (tipoCSharp == typeof(int))
                    {
                        return $"{nomeColuna} SERIAL PRIMARY KEY";
                    }
                    // Para long, usar BIGSERIAL
                    else if (tipoCSharp == typeof(long))
                    {
                        return $"{nomeColuna} BIGSERIAL PRIMARY KEY";
                    }
                    // Fallback para outros tipos de chave primária, se houver
                    return $"{nomeColuna} {tipoSql} PRIMARY KEY";
                }
            }

            // Verifica se tem valor padrão
            var versaoCampo = prop.GetCustomAttribute<VersaoCampoAttribute>();
            var valorPadrao = versaoCampo?.ValorPadrao;

            if (!string.IsNullOrEmpty(valorPadrao))
            {
                return $"{nomeColuna} {tipoSql} {nullConstraint} DEFAULT {valorPadrao}";
            }

            return $"{nomeColuna} {tipoSql} {nullConstraint}";
        }

        /// <summary>
        /// Verifica se propriedade é nullable
        /// </summary>
        private bool IsNullable(PropertyInfo prop)
        {
            var type = prop.PropertyType;

            if (!type.IsValueType)
                return true;

            return Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// Mapeia tipo C# para tipo SQL
        /// </summary>
        private string MapearTipoSql(Type tipo, string tipoExplicito)
        {
            if (!string.IsNullOrEmpty(tipoExplicito))
                return tipoExplicito;

            // Verifica o tipo de banco de dados do contexto
            if (_context.Database.IsSqlServer())
            {
                if (tipo == typeof(int))
                    return "INT";
                if (tipo == typeof(long))
                    return "BIGINT";
                if (tipo == typeof(short))
                    return "SMALLINT";
                if (tipo == typeof(byte))
                    return "TINYINT";
                if (tipo == typeof(bool))
                    return "BIT";
                if (tipo == typeof(decimal))
                    return "DECIMAL(18,2)";
                if (tipo == typeof(double))
                    return "FLOAT";
                if (tipo == typeof(float))
                    return "REAL";
                if (tipo == typeof(DateTime))
                    return "DATETIME";
                if (tipo == typeof(TimeSpan))
                    return "TIME";
                if (tipo == typeof(Guid))
                    return "UNIQUEIDENTIFIER";
                if (tipo == typeof(string))
                    return "NVARCHAR(MAX)";
                if (tipo == typeof(byte[]))
                    return "VARBINARY(MAX)";
            }
            else if (_context.Database.IsNpgsql())
            {
                if (tipo == typeof(int))
                    return "INTEGER";
                if (tipo == typeof(long))
                    return "BIGINT";
                if (tipo == typeof(short))
                    return "SMALLINT";
                if (tipo == typeof(byte))
                    return "SMALLINT"; // PostgreSQL não tem TINYINT, SMALLINT é o mais próximo
                if (tipo == typeof(bool))
                    return "BOOLEAN";
                if (tipo == typeof(decimal))
                    return "NUMERIC(18,2)";
                if (tipo == typeof(double))
                    return "DOUBLE PRECISION";
                if (tipo == typeof(float))
                    return "REAL";
                if (tipo == typeof(DateTime))
                    return "TIMESTAMP WITHOUT TIME ZONE";
                if (tipo == typeof(TimeSpan))
                    return "INTERVAL"; // Ou TIME, dependendo do uso específico
                if (tipo == typeof(Guid))
                    return "UUID";
                if (tipo == typeof(string))
                    return "TEXT"; // Equivalente a NVARCHAR(MAX) no PostgreSQL
                if (tipo == typeof(byte[]))
                    return "BYTEA";
            }

            // Fallback para tipos não mapeados ou caso o provedor não seja reconhecido
            return "TEXT";
        }

        /// <summary>
        /// Gera script completo (todas as versões)
        /// </summary>
        public string GerarScriptCompleto()
        {
            return _databaseCreator.GenerateCreateScript();
        }
    }
}
