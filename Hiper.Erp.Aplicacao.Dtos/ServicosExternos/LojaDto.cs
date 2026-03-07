namespace Hiper.Erp.Aplicacao.Dtos.ServicosExternos
{
    public class LojaDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string NomeFantasia { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public Guid? XTenantId { get; set; }
    }
}
