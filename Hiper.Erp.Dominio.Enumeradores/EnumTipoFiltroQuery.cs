namespace Hiper.Erp.Dominio.Enumeradores
{
    public enum EnumTipoFiltroQuery
    {
        Contem = 1,
        Igual = 2,
        Maior = 3,
        MaiorOuIgual = 4,
        Menor = 5,
        MenorOuIgual = 6
    }

    public class EnumTipoFiltroQueryItem
    {
        public int Codigo { get; set; }
        public EnumTipoFiltroQuery Valor { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }

    public static class EnumTipoFiltroQueryExtensions
    {
        public static List<EnumTipoFiltroQueryItem> Listar()
        {
            return Enum.GetValues(typeof(EnumTipoFiltroQuery))
                .Cast<EnumTipoFiltroQuery>()
                .Select(e => new EnumTipoFiltroQueryItem
                {
                    Codigo = (int)e,
                    Valor = e,
                    Descricao = e.GetDescricao()
                })
                .ToList();
        }

        public static string GetDescricao(this EnumTipoFiltroQuery tipo)
        {
            return tipo switch
            {
                EnumTipoFiltroQuery.Contem => "Contém",
                EnumTipoFiltroQuery.Igual => "Igual a",
                EnumTipoFiltroQuery.Maior => "Maior que",
                EnumTipoFiltroQuery.MaiorOuIgual => "Maior ou igual a",
                EnumTipoFiltroQuery.Menor => "Menor que",
                EnumTipoFiltroQuery.MenorOuIgual => "Menor ou igual a",
                _ => tipo.ToString()
            };
        }
    }
}
