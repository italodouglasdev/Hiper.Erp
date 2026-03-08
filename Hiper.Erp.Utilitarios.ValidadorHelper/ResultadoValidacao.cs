namespace Hiper.Erp.Utilitarios.ValidadorHelper
{
    public class ResultadoValidacao<T>
    {
        public T Dados { get; set; }
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

        public static ResultadoValidacao<T> Ok(T dados)
        {
            return new ResultadoValidacao<T> { Dados = dados };
        }

        public static ResultadoValidacao<T> Falha(params string[] erros)
        {
            return new ResultadoValidacao<T> { Erros = erros.ToList() };
        }

    }
}
