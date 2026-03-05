using Hiper.Erp.Dominio.Enumeradores;

namespace Hiper.Erp.Dominio.Atributos
{
    /// <summary>
    /// Atributo para marcar a versão em que um campo foi adicionado
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class VersaoCampoAttribute : Attribute
    {
        /// <summary>
        /// Versão em que o campo foi adicionado
        /// </summary>
        public EnumVersao Versao { get; }

        /// <summary>
        /// Descrição da alteração
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Indica se o campo pode ser nulo (usado para gerar ALTER TABLE correto)
        /// </summary>
        public bool Nullable { get; set; }

        /// <summary>
        /// Valor padrão para o campo (usado em ALTER TABLE)
        /// </summary>
        public string ValorPadrao { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="versao">Versão do campo</param>
        public VersaoCampoAttribute(EnumVersao versao)
        {
            Versao = versao;
            Nullable = true;
        }
    }
}
