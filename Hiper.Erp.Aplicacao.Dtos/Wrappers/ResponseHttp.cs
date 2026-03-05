namespace Hiper.Erp.Aplicacao.Dtos.Wrappers
{
    /// <summary>
    /// Representa uma resposta padronizada da API com tipagem forte.
    /// </summary>
    /// <typeparam name="T">Tipo do objeto de dados retornado</typeparam>
    public class ResponseHttp<T>
    {
        /// <summary>
        /// Código de status HTTP
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Indica se a operação foi bem-sucedida
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Mensagem descritiva do resultado
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// Dados retornados pela operação
        /// </summary>
        public T Dados { get; set; }

        /// <summary>
        /// Paginação no caso de listas
        /// </summary>
        public Paginacao Paginacao { get; set; }

        /// <summary>
        /// Lista de erros de validação (opcional)
        /// </summary>
        public List<string> Erros { get; set; }

        /// <summary>
        /// Timestamp da resposta
        /// </summary>
        public DateTime Timestamp { get; set; }

        // Construtor privado para forçar uso dos factory methods
        private ResponseHttp(int statusCode, bool sucesso, string mensagem, T dados, List<string> erros = null, Paginacao paginacao = null)
        {
            StatusCode = statusCode;
            Sucesso = sucesso;
            Mensagem = mensagem ?? string.Empty;
            Dados = dados;
            Erros = erros ?? new List<string>();
            Timestamp = DateTime.UtcNow;
            Paginacao = paginacao;
        }

        public ResponseHttp() { }

        #region Factory Methods - Sucesso

        /// <summary>
        /// Cria uma resposta de sucesso (200 OK)
        /// </summary>
        public static ResponseHttp<T> Ok(T dados, string mensagem = "Operação realizada com sucesso", Paginacao paginacao = null)
        {
            return new ResponseHttp<T>(200, true, mensagem, dados, null, paginacao);
        }

        /// <summary>
        /// Cria uma resposta de sucesso (200 OK)
        /// </summary>
        public static ResponseHttp<T> Ok(ResultadoOperacao<T> resultadoServico)
        {
            return new ResponseHttp<T>(200, true, resultadoServico.Mensagem, resultadoServico.Dados, resultadoServico.Erros, resultadoServico.Paginacao);
        }

        /// <summary>
        /// Cria uma resposta de criação bem-sucedida (201 Created)
        /// </summary>
        public static ResponseHttp<T> Created(T dados, string mensagem = "Recurso criado com sucesso")
        {
            return new ResponseHttp<T>(201, true, mensagem, dados);
        }

        /// <summary>
        /// Cria uma resposta de sucesso sem conteúdo (204 No Content)
        /// </summary>
        public static ResponseHttp<T> NoContent(string mensagem = "Operação realizada com sucesso")
        {
            return new ResponseHttp<T>(204, true, mensagem, default);
        }

        #endregion

        #region Factory Methods - Erro

        /// <summary>
        /// Cria uma resposta de erro de validação (400 Bad Request)
        /// </summary>
        public static ResponseHttp<T> BadRequest(string mensagem, List<string> erros = null)
        {
            return new ResponseHttp<T>(400, false, mensagem, default, erros);
        }

        /// <summary>
        /// Cria uma resposta de não autorizado (401 Unauthorized)
        /// </summary>
        public static ResponseHttp<T> Unauthorized(string mensagem = "Não autorizado")
        {
            return new ResponseHttp<T>(401, false, mensagem, default);
        }

        /// <summary>
        /// Cria uma resposta de acesso negado (403 Forbidden)
        /// </summary>
        public static ResponseHttp<T> Forbidden(string mensagem = "Acesso negado")
        {
            return new ResponseHttp<T>(403, false, mensagem, default);
        }

        /// <summary>
        /// Cria uma resposta de não encontrado (404 Not Found)
        /// </summary>
        public static ResponseHttp<T> NotFound(string mensagem = "Recurso não encontrado", List<string> erros = null, Paginacao paginacao = null)
        {
            return new ResponseHttp<T>(404, false, mensagem, default, erros, paginacao);
        }

        /// <summary>
        /// Cria uma resposta de não encontrado (404 Not Found)
        /// </summary>
        public static ResponseHttp<T> NotFound(ResultadoOperacao<T> resultadoServico)
        {
            return new ResponseHttp<T>(404, false, resultadoServico.Mensagem, resultadoServico.Dados, resultadoServico.Erros, resultadoServico.Paginacao);
        }

        /// <summary>
        /// Cria uma resposta de conflito (409 Conflict)
        /// </summary>
        public static ResponseHttp<T> Conflict(string mensagem, List<string> erros = null)
        {
            return new ResponseHttp<T>(409, false, mensagem, default, erros);
        }

        /// <summary>
        /// Cria uma resposta de erro interno do servidor (500 Internal Server Error)
        /// </summary>
        public static ResponseHttp<T> InternalServerError(string mensagem = "Erro interno do servidor")
        {
            return new ResponseHttp<T>(500, false, mensagem, default);
        }

        /// <summary>
        /// Cria uma resposta de erro customizada
        /// </summary>
        public static ResponseHttp<T> Error(int statusCode, string mensagem, List<string> erros = null)
        {
            return new ResponseHttp<T>(statusCode, false, mensagem, default, erros);
        }

        #endregion
    }
}
