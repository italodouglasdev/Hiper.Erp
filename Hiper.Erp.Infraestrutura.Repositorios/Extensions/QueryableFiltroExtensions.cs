using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros;
using Hiper.Erp.Dominio.Enumeradores;
using System.Linq.Expressions;
using System.Reflection;

namespace Hiper.Erp.Infraestrutura.Repositorios.Extensions
{
    public static class QueryableFiltroExtensions
    {
        public static IQueryable<T> AplicarFiltros<T>(this IQueryable<T> query, DtoFiltro filtro)
        {
            if (filtro.Campos == null || !filtro.Campos.Any())
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? expressaoFinal = null;

            foreach (var campo in filtro.Campos)
            {
                var propriedade = typeof(T)
                    .GetProperty(campo.Nome, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propriedade == null)
                    continue;

                Expression? expressaoColuna = null;

                foreach (var valor in campo.Valores)
                {
                    var left = Expression.Property(parameter, propriedade);
                    var valorConvertido = Convert.ChangeType(valor.Valor, propriedade.PropertyType);
                    var right = Expression.Constant(valorConvertido);

                    Expression comparacao = valor.TipoFiltro switch
                    {
                        EnumTipoFiltroQuery.Contem => Expression.Call(left, nameof(string.Contains), Type.EmptyTypes, right),
                        EnumTipoFiltroQuery.Igual => Expression.Equal(left, right),
                        EnumTipoFiltroQuery.Maior => Expression.GreaterThan(left, right),
                        EnumTipoFiltroQuery.MaiorOuIgual => Expression.GreaterThanOrEqual(left, right),
                        EnumTipoFiltroQuery.Menor => Expression.LessThan(left, right),
                        EnumTipoFiltroQuery.MenorOuIgual => Expression.LessThanOrEqual(left, right),

                        _ => throw new NotSupportedException()
                    };

                    expressaoColuna = expressaoColuna == null ? comparacao : Expression.OrElse(expressaoColuna, comparacao);
                }

                if (expressaoColuna != null)
                    expressaoFinal = expressaoFinal == null ? expressaoColuna : Expression.AndAlso(expressaoFinal, expressaoColuna);

            }

            if (expressaoFinal == null)
                return query;

            var lambda = Expression.Lambda<Func<T, bool>>(expressaoFinal, parameter);

            return query.Where(lambda);



        }
    }
}
