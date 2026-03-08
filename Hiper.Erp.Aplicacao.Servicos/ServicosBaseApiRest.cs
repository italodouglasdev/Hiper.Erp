using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace Hiper.Erp.Aplicacao.Servicos
{
    public class ServicosBaseApiRest
    {
        private readonly HttpClient HttpClient;
        private readonly IHttpContextAccessor? HttpContextAccessor;

        public ServicosBaseApiRest(HttpClient httpClient)
        {
            HttpClient = httpClient;
            HttpContextAccessor = null;
        }

        public ServicosBaseApiRest(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            HttpClient = httpClient;
            HttpContextAccessor = httpContextAccessor;
        }

        public async Task<ResultadoOperacao<TDto>> GetAsync<TDto>(string url)
        {
            try
            {
                AdicionarTokenAoHeader();
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
                AdicionarTokenAoHeader();
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
                AdicionarTokenAoHeader();
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
                AdicionarTokenAoHeader();
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
                AdicionarTokenAoHeader();
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
                AdicionarTokenAoHeader();
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

        private void AdicionarTokenAoHeader()
        {
            // Obtém o token do HttpContext.Items (armazenado pelo middleware)
            if (HttpContextAccessor?.HttpContext != null)
            {
                var token = HttpContextAccessor.HttpContext.Items["JwtToken"] as string;
                if (!string.IsNullOrEmpty(token))
                {
                    HttpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
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
