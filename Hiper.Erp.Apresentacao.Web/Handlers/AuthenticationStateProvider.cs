using Hiper.Erp.InterfaceUsuarios.RetaguardaWeb.Servicos;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Hiper.Erp.Apresentacao.Web.Handlers
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly StorageServico _storageServico;
        private bool _isInitialized = false;
        private string? _cachedToken = null;

        public CustomAuthenticationStateProvider(StorageServico storageServico)
        {
            _storageServico = storageServico;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // Se ainda não foi inicializado, tenta carregar o token
                if (!_isInitialized)
                {
                    _cachedToken = await _storageServico.GetItem<string>("auth_token");
                    _isInitialized = true;
                }

                // Se não tem token, retorna não autenticado
                if (string.IsNullOrWhiteSpace(_cachedToken))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                // Se tem token, retorna autenticado
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, "user"),
                    new Claim("token", _cachedToken)
                };

                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao obter estado de autenticação: {ex.Message}");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task MarkUserAsAuthenticated(string token)
        {
            _cachedToken = token;
            _isInitialized = true;
            await _storageServico.SetItem("auth_token", token);
            var authState = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task MarkUserAsLoggedOut()
        {
            _cachedToken = null;
            _isInitialized = true;
            await _storageServico.RemoveItem("auth_token");
            await _storageServico.RemoveItem("tenant_id");
            var authState = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }
    }
}
