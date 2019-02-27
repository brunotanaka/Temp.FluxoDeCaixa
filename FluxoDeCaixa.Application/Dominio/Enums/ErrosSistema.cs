using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FluxoDeCaixa.Application.Dominio.Enums
{
    public enum ErrosSistemas : int
    {
        [Description("Tipo de lancamento invalido, permitido somente (1 - Pagamento ou 2 - Recebimento)")]
        TipoLancamentoInvalido = 1,
        [Description("Tipo de Conta invalido, permitido somente (1 - ContaCorrente ou 2 - Poupanca)")]
        TipoContaInvalida = 2,
        [Description("Data invalida, formato dd-mm-aaaa")]
        DataFormatoInvalido = 3,
        [Description("Não é possível fazer lancamento retroativo.")]
        LancamentoRetroativo = 4,
        [Description("Limite negativo atingido.")]
        LimiteNegativoAtingido = 5,
        [Description("Valor lancamento inválido .")]
        ValorLancamentoInvalido = 6,
        [Description("Valor encargo inválido .")]
        ValorEncargoInvalido = 7,
    }
}
