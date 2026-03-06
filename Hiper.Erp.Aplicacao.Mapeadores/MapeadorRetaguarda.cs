using AutoMapper;
using Hiper.Erp.Aplicacao.Dtos.Agentes;
using Hiper.Erp.Aplicacao.Dtos.FormasPagamentos;
using Hiper.Erp.Aplicacao.Dtos.Produtos;
using Hiper.Erp.Aplicacao.Dtos.Vendas;
using Hiper.Erp.Dominio.Entidades.Agentes;
using Hiper.Erp.Dominio.Entidades.FormasPagamentos;
using Hiper.Erp.Dominio.Entidades.Produtos;
using Hiper.Erp.Dominio.Entidades.Vendas;

namespace Hiper.Erp.Aplicacao.Mapeadores
{
    public class MapeadorRetaguarda : Profile
    {
        public MapeadorRetaguarda()
        {
            CreateMap<DtoAgente, EntidadeAgente>();
            CreateMap<DtoProduto, EntidadeProduto>();
            CreateMap<DtoFormaPagamento, EntidadeFormaPagamento>();
            CreateMap<DtoVenda, EntidadeVenda>();
            CreateMap<DtoVendaItem, EntidadeVendaItem>();
        }
    }
}
