using FluxoDeCaixa.Application.Repositorio;
using FluxoDeCaixa.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Net.Sockets;

namespace FluxoDeCaixa.Agent
{
    public class DependencyInjection
    {
        public IServiceProvider ServicesProvider;
        private readonly string nameDatabase = "fluxoDeCaixa";

        public DependencyInjection(IConfiguration configuration)
        {
            try
            {
                var services = new ServiceCollection();
                var connection = configuration.GetConnectionString("MongoDBConnection");

                var url = MongoUrl.Create(connection);

                var settings = new MongoClientSettings()
                {
                    Server = url.Server,
                    WaitQueueSize = 1000,
                    MaxConnectionPoolSize = 1000,
                    MaxConnectionIdleTime = TimeSpan.FromSeconds(60)
                };

                void SocketConfigurator(Socket s) => s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                settings.ClusterConfigurator = builder => builder
                    .ConfigureTcp(tcp => tcp.With(socketConfigurator: (Action<Socket>)SocketConfigurator));

                var client = new MongoClient(settings);

                var database = client.GetDatabase(nameDatabase);

                services.AddSingleton(configuration);
                services.AddScoped(typeof(IRepositorio<>), typeof(Repositorio<>));
                services.AddScoped(typeof(IFluxoDeCaixaService), typeof(FluxoDeCaixaService));
                services.AddSingleton(database);

                ServicesProvider = services.BuildServiceProvider();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na injeção de dependencia: Message: {ex.Message}");
                Console.WriteLine($"Erro na injeção de dependencia: Trace: {ex.StackTrace}");
            }
        }
    }
}
