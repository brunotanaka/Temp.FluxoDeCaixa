using FluxoDeCaixa.Application.Dominio;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Application.Services
{
    public interface IFluxoDeCaixaService
    {
        Task<ConsolidadoFluxo> BuscaDadoConsolidado(DateTime data);
        Task<IEnumerable<ConsolidadoFluxo>> BuscaDadosConsolidados();
        Task<LancamentoFinanceiro> EfetuaLancamento(LancamentoFinanceiro lancamentoFinanceiro);

        decimal CalcularBalancoDiario(IEnumerable<LancamentoFinanceiro> lancamentoFinanceiros);
        string CalcularPosicao(decimal totalDiaAnterior, decimal totalDiaAtual);

        decimal CalcularJuros(decimal valorDivida, decimal percentual);

        void EnviaParaFila(LancamentoFinanceiro lancamento, ConnectionFactory connectionFactory);

        Task<ConsolidadoFluxo> AdicionarLancamento(LancamentoFinanceiro lancamento);
    }
}
