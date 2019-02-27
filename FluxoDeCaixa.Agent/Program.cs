using FluxoDeCaixa.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Agent
{
    public class Program
    {
        public static IConfiguration Configuration { get; set; }
        public static IFluxoDeCaixaService FluxoDeCaixaService { get; set; }

        public static void Main(string[] args)
        {
            Thread.Sleep(TimeSpan.FromSeconds(10));

            var startup = new Startup();
            var servicesProvider = startup.ServicesProvider;

            Configuration = servicesProvider.GetRequiredService<IConfiguration>();
            FluxoDeCaixaService = servicesProvider.GetRequiredService<IFluxoDeCaixaService>();

            var tasks = new Task[]
           {
                Task.Run(() =>
                {
                    var consumerPagamento = new ConsumerPagamento(FluxoDeCaixaService, Configuration);
                    consumerPagamento.ConsumirFila("queue-pagamento");

                }),
                Task.Run(() =>
                {
                    var consumerPagamento = new ConsumerRecebimento(FluxoDeCaixaService, Configuration);
                    consumerPagamento.ConsumirFila("queue-recebimento");
                })
           };

            Task.WaitAll(tasks, -1);
        }
    }
}
