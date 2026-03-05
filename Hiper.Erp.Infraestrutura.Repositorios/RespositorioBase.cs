using AutoMapper;
using Hiper.Erp.Infraestrutura.Bancos;

namespace Hiper.Erp.Infraestrutura.Repositorios
{
    public class RespositorioBase<TEntity> where TEntity : class
    {
        protected readonly RetaguardaDbContext contexto;
        protected readonly HttpClient httpClient;
        protected readonly IMapper mapeador;

        // Construtor para Repositórios de Banco (Injetando o DbContext já configurado pelo DI)
        public RespositorioBase(RetaguardaDbContext contexto)
        {
            this.contexto = contexto;
        }

        // Construtor para Repositórios de API
        public RespositorioBase(HttpClient _http, IMapper mapeador)
        {
            this.httpClient = _http;
            this.mapeador = mapeador;
        }

        // Construtor base para mapeamento
        public RespositorioBase(IMapper mapeador)
        {
            this.mapeador = mapeador;
        }
    }
}
