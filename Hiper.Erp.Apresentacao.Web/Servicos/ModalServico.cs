using Microsoft.JSInterop;

namespace idSaas.Erp.InterfaceUsuarios.RetaguardaWeb.Servicos
{
    public class ModalServico
    {
        private readonly IJSRuntime _js;

        public ModalServico(IJSRuntime js)
        {
            _js = js;
        }

        public async Task Abrir(string id)
        {
            await _js.InvokeVoidAsync("bootstrapModal.show", id);
        }

        public async Task Fechar(string id)
        {
            await _js.InvokeVoidAsync("bootstrapModal.hide", id);
        }
    }
}
