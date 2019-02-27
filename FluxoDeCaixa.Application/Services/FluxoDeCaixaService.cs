using FluxoDeCaixa.Application.Dominio;
using FluxoDeCaixa.Application.Dominio.Enums;
using FluxoDeCaixa.Application.Repositorio;
using FluxoDeCaixa.Application.Util;
using MongoDB.Driver;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Application.Services
{
    public class FluxoDeCaixaService : IFluxoDeCaixaService
    {
        private readonly IRepositorio<ConsolidadoFluxo> _repositorioConsolidadoFluxo;
        private readonly IRepositorio<LancamentoFinanceiro> _repositorioLancamentoFinanceiro;

        public FluxoDeCaixaService(IRepositorio<ConsolidadoFluxo> repositorioConsolidadoFluxo, IRepositorio<LancamentoFinanceiro> repositorioLancamentoFinanceiro)
        {
            _repositorioConsolidadoFluxo = repositorioConsolidadoFluxo;
            _repositorioLancamentoFinanceiro = repositorioLancamentoFinanceiro;
        }

        public async Task<ConsolidadoFluxo> BuscaDadoConsolidado(DateTime data)
        {
            return await _repositorioConsolidadoFluxo.BuscarPor_Async(Builders<ConsolidadoFluxo>.Filter.Where(x => x.Data == data));
        }

        public async Task<IEnumerable<ConsolidadoFluxo>> BuscaDadosConsolidados()
        {
            List<ConsolidadoFluxo> consolidadoFluxos = new List<ConsolidadoFluxo>();

            var dataFim = DateTime.Now.AddDays(30).Date;
            var data = DateTime.Now.AddDays(-1).Date; // para o dia atual vir com posição
            var saldoDiaAnterior = 0m;

            for (int i = 0; data <= dataFim; i++)
            {
                var consolidado = await BuscaDadoConsolidado(data);
                var totalOriginal = 0m;

                if (consolidado == null)
                    consolidado = new ConsolidadoFluxo() { Data = data, Encargos = new List<DataValor>(), Entradas = new List<DataValor>(), Saidas = new List<DataValor>(), Total = 0m };
                else
                    consolidado.PosicaoDia = i == 0 ? "0,00%" : CalcularPosicao(saldoDiaAnterior, consolidado.Total);

                totalOriginal = consolidado.Total;

                if (saldoDiaAnterior < 0)
                    consolidado.Total += saldoDiaAnterior + CalcularJuros(saldoDiaAnterior, 0.83m);
                else
                    consolidado.Total += i == 0 ? 0m : saldoDiaAnterior;

                if (string.IsNullOrEmpty(consolidado.PosicaoDia))
                    consolidado.PosicaoDia = i == 0 ? "0,00%" : CalcularPosicao(saldoDiaAnterior, consolidado.Total);

                saldoDiaAnterior = consolidado.Total;
                consolidado.Total = totalOriginal;

                data = data.AddDays(1);
                consolidadoFluxos.Add(consolidado);
            }

            consolidadoFluxos.RemoveAt(0);

            return consolidadoFluxos.OrderBy(x => x.Data);
        }

        public decimal CalcularBalancoDiario(IEnumerable<LancamentoFinanceiro> lancamentoFinanceiros)
        {
            decimal balanco = 0;

            decimal valores_Positivos = lancamentoFinanceiros.Where(x => x.Lancamento == Dominio.Enums.TipoLancamento.Recebimento).Sum(x => x.Valor) - lancamentoFinanceiros.Where(x => x.Lancamento == Dominio.Enums.TipoLancamento.Recebimento).Sum(x => x.Encargos);
            decimal valores_Negativos = lancamentoFinanceiros.Where(x => x.Lancamento == Dominio.Enums.TipoLancamento.Pagamento).Sum(x => x.Valor) + lancamentoFinanceiros.Where(x => x.Lancamento == Dominio.Enums.TipoLancamento.Pagamento).Sum(x => x.Encargos);

            balanco = valores_Positivos + (valores_Negativos * -1);

            return balanco;
        }

        public string CalcularPosicao(decimal totalDiaAnterior, decimal totalDiaAtual)
        {
            var variacao = 0m;
            var diferenca = 0m;
            var dividendo = 0m;
            var valorMenor = totalDiaAnterior > totalDiaAtual ? totalDiaAtual : totalDiaAnterior;
            var valorMaior = totalDiaAnterior > totalDiaAtual ? totalDiaAnterior : totalDiaAtual;
            bool doMenorParaMaior = totalDiaAtual > totalDiaAnterior;

            if (valorMenor < 0)
                valorMenor = valorMenor * -1;

            if (valorMaior < 0)
                valorMaior = valorMaior * -1;

            dividendo = doMenorParaMaior ? valorMenor == 0m ? 1m : valorMenor : valorMaior == 0 ? 1m : valorMaior;
            diferenca = valorMaior - valorMenor;

            variacao = (diferenca / dividendo) * 100;

            if (totalDiaAtual > totalDiaAnterior)
            {
                if (variacao < 0)
                    variacao = variacao * -1;
            }
            else
            {
                if (variacao > 0)
                    variacao = variacao * -1;
            }

            if (variacao == 0m)
                return "0.00%";

            return $"{Math.Round(variacao, 2).ToString("F2", CultureInfo.InvariantCulture)}%";
        }
        public decimal CalcularJuros(decimal valorDivida, decimal percentual)
        {
            var valorCorrigido = valorDivida;

            valorCorrigido = ((valorDivida * percentual) / 100);

            return valorCorrigido;
        }

        public async Task<LancamentoFinanceiro> EfetuaLancamento(LancamentoFinanceiro lancamentoFinanceiro)
        {
            if (lancamentoFinanceiro.Lancamento == TipoLancamento.Pagamento)
            {
                var lancamentosDoDia = await _repositorioLancamentoFinanceiro.Buscar_Async(Builders<LancamentoFinanceiro>.Filter.Where(x => x.Data == lancamentoFinanceiro.Data));
                lancamentosDoDia = lancamentosDoDia.Append(lancamentoFinanceiro);

                var balancoDiario = CalcularBalancoDiario(lancamentosDoDia);

                if (balancoDiario <= -20000m)
                    throw new DominioException(ErrosSistemas.LimiteNegativoAtingido);
            }

            return await _repositorioLancamentoFinanceiro.Salvar_Async(lancamentoFinanceiro);
        }

        public void EnviaParaFila(LancamentoFinanceiro lancamento, ConnectionFactory connectionFactory)
        {
            var queue = $"queue-{lancamento.Lancamento.GetDescription().ToLower()}";

            using (var conn = connectionFactory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                channel.QueueDeclare(queue: queue,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

                var message = JsonConvert.SerializeObject(new
                {
                    lancamento.Descricao,
                    lancamento.Conta,
                    lancamento.Banco,
                    lancamento.CpfCnpj,
                    Valor = lancamento.Valor.ToString(),
                    Encargos = lancamento.Encargos.ToString(),
                    lancamento.Lancamento,
                    lancamento.TipoConta,
                    lancamento.Data
                });
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: queue,
                                     basicProperties: null,
                                     body: body);
            }
        }

        public async Task<ConsolidadoFluxo> AdicionarLancamento(LancamentoFinanceiro lancamento)
        {
            var dataLancamento = DateTime.ParseExact(lancamento.Data, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None);

            var consolidadoFluxo = await BuscaDadoConsolidado(dataLancamento);

            if (consolidadoFluxo == null)
                consolidadoFluxo = new ConsolidadoFluxo() { Data = dataLancamento, Encargos = new List<DataValor>(), Entradas = new List<DataValor>(), Saidas = new List<DataValor>() };

            if (lancamento.Lancamento == TipoLancamento.Pagamento)
                consolidadoFluxo.Saidas.Add(new DataValor(lancamento.Data, lancamento.Valor));

            if (lancamento.Lancamento == TipoLancamento.Recebimento)
                consolidadoFluxo.Entradas.Add(new DataValor(lancamento.Data, lancamento.Valor));

            consolidadoFluxo.Encargos.Add(new DataValor(lancamento.Data, lancamento.Encargos));

            consolidadoFluxo.Total = consolidadoFluxo.Entradas.Sum(x => x.Valor) + ((consolidadoFluxo.Saidas.Sum(x => x.Valor) + consolidadoFluxo.Encargos.Sum(x => x.Valor)) * -1);

            return await _repositorioConsolidadoFluxo.Salvar_Async(consolidadoFluxo);
        }
    }
}