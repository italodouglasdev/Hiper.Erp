using Microsoft.JSInterop;
using System.Text.Json;

namespace Hiper.Erp.InterfaceUsuarios.RetaguardaWeb.Servicos
{
    public class StorageServico
    {
        private readonly IJSRuntime _js;

        public StorageServico(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SetItem<T>(string chave, T valor)
        {
            var json = JsonSerializer.Serialize(valor);
            await _js.InvokeVoidAsync("localStorage.setItem", chave, json);
        }

        public async Task<T?> GetItem<T>(string chave)
        {
            var json = await _js.InvokeAsync<string>("localStorage.getItem", chave);

            if (string.IsNullOrEmpty(json))
                return default;

            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task RemoveItem(string chave)
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", chave);
        }

        public async Task Clear()
        {
            await _js.InvokeVoidAsync("localStorage.clear");
        }
    }
}