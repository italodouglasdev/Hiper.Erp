using AutoMapper;

namespace Hiper.Erp.Aplicacao.Servicos
{
    public class ServicoBase
    {
        public readonly IMapper mapeador;

        private string hashAcesso;

        public ServicoBase(IMapper mapper)
        {
            this.mapeador = mapper;
        }

        public void ObtenhaHashAcesso(string hashAcesso)
        {
            this.hashAcesso = hashAcesso;
        }

        
    }
}
