using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FluxoDeCaixa.Application.Dominio.Enums
{
    public enum TipoLancamento : int
    {
        Pagamento = 1,
        Recebimento = 2
    }
}
