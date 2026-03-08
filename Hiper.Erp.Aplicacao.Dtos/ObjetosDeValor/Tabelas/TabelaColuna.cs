namespace Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Tabelas
{
    public class TabelaColuna
    {
        public string NomePropriedade { get; set; } = string.Empty;
        public string NomeColuna { get; set; } = string.Empty;
        public bool ExibirFiltro { get; set; }
        public bool ExibirColuna { get; set; }
        public Type Tipo { get; set; } = typeof(string);
    }
}
