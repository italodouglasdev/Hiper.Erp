using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiper.Erp.Utilitarios.ValidadorHelper
{
    public class ValidadorDecimal
    {
        public static ResultadoValidacao<bool> MaiorQueZero(decimal valor)
        {
            if (valor > 0)
                return ResultadoValidacao<bool>.Ok(true);

            return ResultadoValidacao<bool>.Falha("O valor informado deve ser maior do que zero.");
        }

    }
}
