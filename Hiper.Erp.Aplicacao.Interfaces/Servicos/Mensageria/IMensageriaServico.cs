namespace Hiper.Erp.Aplicacao.Interfaces.Servicos.Mensageria
{
    public interface IMensageriaServico
    {
        Task PublicarMensagemAsync<T>(string nomeFila, T mensagem);
    }
}
