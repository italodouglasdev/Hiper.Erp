namespace Hiper.Erp.Aplicacao.Dtos.Wrappers
{
    public class ResultadoOperacao<T>
    {
        public T Dados { get; set; }
        public Paginacao Paginacao { get; set; }
        public List<string> Erros { get; set; } = new List<string>();
        public bool Sucesso => Erros.Count == 0;
        public string Mensagem => Erros.Count > 0 ? this.Erros.FirstOrDefault() : "Operação realizada com sucesso.";

        public void AdicionarErro(string erro)
        {
            this.Erros.Add(erro);
        }

        public void AdicionarErros(List<string> erros)
        {
            this.Erros.AddRange(erros);
        }

        public void AdicionarPaginacao(int PaginaAtual, int QuantidadeItensEmExibicao, int QuantidadeItensTotal)
        {
            this.Paginacao = new Paginacao(PaginaAtual, QuantidadeItensEmExibicao, QuantidadeItensTotal);
        }

        public static ResultadoOperacao<T> Ok(T dados)
        {
            return new ResultadoOperacao<T> { Dados = dados };
        }

        public static ResultadoOperacao<T> Falha(params string[] erros)
        {
            return new ResultadoOperacao<T> { Erros = erros.ToList() };
        }

        public static ResultadoOperacao<T> ConverterResponseHttp(T dados, List<string> erros, Paginacao paginacao)
        {
            return new ResultadoOperacao<T> { Dados = dados, Erros = erros, Paginacao = paginacao };
        }


    }
}
