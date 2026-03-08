using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using System.Net.Http.Json;

namespace Hiper.Erp.Aplicacao.Servicos
{
    public class ServicosBaseApiRest
    {
        private readonly HttpClient HttpClient;

        public ServicosBaseApiRest(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<ResultadoOperacao<TDto>> GetAsync<TDto>(string url)
        {
            try
            {
                var response = await HttpClient.GetAsync(url);
                return await ProcessarResposta<TDto>(response);
            }
            catch (Exception ex)
            {
                return ResultadoOperacao<TDto>.Falha($"Falha ao consultar! Detalhes: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacao<TDto>> PostAsync<TDto>(string url, TDto dto)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync(url, dto);

                return await ProcessarResposta<TDto>(response);
            }
            catch (Exception ex)
            {
                return ResultadoOperacao<TDto>.Falha($"Falha ao inserir! Detalhes: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacao<TDto>> PostAsync<TDto>(string url, DtoFiltro filtro)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync(url, filtro);
                return await ProcessarResposta<TDto>(response);
            }
            catch (Exception ex)
            {
                return ResultadoOperacao<TDto>.Falha($"Falha ao consultar! Detalhes: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacao<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest dto)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync(url, dto);

                return await ProcessarResposta<TResponse>(response);
            }
            catch (Exception ex)
            {
                return ResultadoOperacao<TResponse>.Falha($"Falha ao inserir! Detalhes: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacao<TDto>> PutAsync<TDto>(string url, TDto dto)
        {
            try
            {
                var response = await HttpClient.PutAsJsonAsync(url, dto);
                return await ProcessarResposta<TDto>(response);
            }
            catch (Exception ex)
            {
                return ResultadoOperacao<TDto>.Falha($"Falha ao atualizar! Detalhes: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacao<bool>> DeleteAsync(string url)
        {
            try
            {
                var response = await HttpClient.DeleteAsync(url);

                if (!response.IsSuccessStatusCode)
                    return ResultadoOperacao<bool>.Falha("Erro ao excluir o registro na API.");

                return ResultadoOperacao<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return ResultadoOperacao<bool>.Falha($"Falha ao excluir! Detalhes: {ex.Message}");
            }
        }

        private async Task<ResultadoOperacao<TDto>> ProcessarResposta<TDto>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var stringErro = await response.Content.ReadAsStringAsync();
                return ResultadoOperacao<TDto>.Falha($"Erro {response.StatusCode}: {stringErro}");
            }

                   var resultado = await response.Content.ReadFromJsonAsync<ResponseHttp<TDto>>();

            if (resultado == null)
                return ResultadoOperacao<TDto>.Falha("Resposta inválida da API.");

            return ResultadoOperacao<TDto>.ConverterResponseHttp(
                resultado.Dados,
                resultado.Erros,
                resultado.Paginacao);
        }
    }
}