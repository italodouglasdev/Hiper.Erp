using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.Filtros;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;
using System.Net.Http.Json;

namespace Hiper.Erp.Infraestrutura.Repositorios
{
    internal class RepositorioApi
    {
        private readonly HttpClient HttpClient;
        private readonly IMapper Mapeador;

        public RepositorioApi(HttpClient httpClient, IMapper mapeador)
        {
            this.HttpClient = httpClient;
            this.Mapeador = mapeador;
        }

        public async Task<ResultadoOperacao<TEntidade>> GetAsync<TEntidade, TDto>(string url)
        {
            try
            {
                var response = await HttpClient.GetAsync(url);
                return await ProcessarResposta<TEntidade, TDto>(response);
            }
            catch (Exception ex)
            {
                return ResultadoOperacao<TEntidade>.Falha($"Falha ao consultar! Detalhes: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacao<TEntidade>> PostAsync<TEntidade, TDto>(string url, TEntidade entidade)
        {
            try
            {
                var dto = Mapeador.Map<TDto>(entidade);

                var response = await HttpClient.PostAsJsonAsync(url, dto);

                return await ProcessarResposta<TEntidade, TDto>(response);
            }
            catch (Exception ex)
            {
                return ResultadoOperacao<TEntidade>.Falha($"Falha ao inserir! Detalhes: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacao<TEntidade>> PostAsync<TEntidade, TDto>(string url, DtoFiltro filtro)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync(url, filtro);

                return await ProcessarResposta<TEntidade, TDto>(response);
            }
            catch (Exception ex)
            {
                return ResultadoOperacao<TEntidade>.Falha($"Falha ao inserir! Detalhes: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacao<TEntidade>> PutAsync<TEntidade, TDto>(string url, TEntidade entidade)
        {
            try
            {
                var dto = Mapeador.Map<TDto>(entidade);

                var response = await HttpClient.PutAsJsonAsync(url, dto);

                return await ProcessarResposta<TEntidade, TDto>(response);
            }
            catch (Exception ex)
            {
                return ResultadoOperacao<TEntidade>.Falha($"Falha ao atualizar! Detalhes: {ex.Message}");
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


        private async Task<ResultadoOperacao<TEntidade>> ProcessarResposta<TEntidade, TDto>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var stringErro = await response.Content.ReadAsStringAsync();

                return ResultadoOperacao<TEntidade>.Falha($"Erro {response.StatusCode}: {stringErro}");
            }

            var resultado = await response.Content.ReadFromJsonAsync<ResponseHttp<TDto>>();

            if (resultado == null)
                return ResultadoOperacao<TEntidade>.Falha("Resposta inválida da API.");

            return ResultadoOperacao<TEntidade>.ConverterResponseHttp(
                Mapeador.Map<TEntidade>(resultado.Dados),
                resultado.Erros,
                resultado.Paginacao);
        }

        internal Task<Task<ResultadoOperacao<T>>> GetAsync<T>(string v)
        {
            throw new NotImplementedException();
        }
    }
}
