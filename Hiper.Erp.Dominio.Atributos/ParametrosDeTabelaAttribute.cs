namespace Hiper.Erp.Dominio.Atributos
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ParametrosDeTabelaAttribute : Attribute
    {
        public string? NomeColuna { get; set; }
        public string? DescricaoColuna { get; set; }
        public bool ExibirColuna { get; set; } = true;
        public bool ExibirFiltros { get; set; } = true;
    }
}
