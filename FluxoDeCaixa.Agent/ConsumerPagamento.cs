using FluxoDeCaixa.Application.Dominio;
using FluxoDeCaixa.Application.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Agent
{
    public class ConsumerPagamento : IConsumer
    {
        private IConfiguration _configuration;
        private IFluxoDeCaixaService _fluxoDeCaixaService;
        private ConnectionFactory _connection
        {
            get
            {
                var hostName = _configuration["RabbitHostName"];
                var userName = _configuration["RabbitUserName"];
                var password = _configuration["RabbitPassword"];
                var virtualHost = _configuration["RabbitVirtualHost"];
                var port = Convert.ToInt32(_configuration["RabbitPort"]);

                if (connection == null)
                    connection = new ConnectionFactory() { HostName = hostName, UserName = userName, Password = password, VirtualHost = virtualHost, Port = port };

                return connection;
            }
            set { connection = value; }
        }
        private ConnectionFactory connection { get; set; }

        public ConsumerPagamento(IFluxoDeCaixaService fluxoDeCaixaService, IConfiguration configuration)
        {
            _configuration = configuration;
            _fluxoDeCaixaService = fluxoDeCaixaService;
        }

        public Task ConsumirFila(string fila)
        {
            while (true)
            {
                using (var connection = _connection.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: fila,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var lancamento = JsonConvert.DeserializeObject<LancamentoFinanceiro>(message, new JsonSerializerSettings
                        {
                            Culture = new System.Globalization.CultureInfo("pt-BR")
                        });

                        await ConsumirMensagem(lancamento);
                    };

                    channel.BasicConsume(queue: fila,
                                         autoAck: true,
                                         consumer: consumer);

                    Thread.Sleep(100);
                }
            }
        }

        public async Task ConsumirMensagem(LancamentoFinanceiro lancamentoFinanceiro)
        {
            await _fluxoDeCaixaService.AdicionarLancamento(lancamentoFinanceiro);
        }
    }
}
