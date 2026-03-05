using Hiper.Erp.Dominio.Enumeradores;

namespace Hiper.Erp.Dominio.Atributos
{
    /// <summary>
    /// Atributo para marcar a versão em que uma tabela foi criada
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class VersaoTabelaAttribute : Attribute
    {
        /// <summary>
        /// Versão em que a tabela foi criada
        /// </summary>
        public EnumVersao Versao { get; }

        /// <summary>
        /// Descrição da alteração
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="versao">Versão da tabela</param>
        public VersaoTabelaAttribute(EnumVersao versao)
        {
            Versao = versao;
        }
    }
}
