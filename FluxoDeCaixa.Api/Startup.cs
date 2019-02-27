using FluxoDeCaixa.Application.Repositorio;
using FluxoDeCaixa.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;

namespace FluxoDeCaixa.Api
{
    public class Startup
    {
        public IConfigurationRoot Configuration;
        private readonly string nameDatabase = "fluxoDeCaixa";

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            var cultureInfo = new CultureInfo("pt-BR");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Fluxo de Caixa API", Version = "v1" });
            });

            var connection = Configuration.GetConnectionString("MongoDBConnection");
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

            services.AddScoped(typeof(IRepositorio<>), typeof(Repositorio<>));
            services.AddScoped<IFluxoDeCaixaService, FluxoDeCaixaService>();
            services.AddSingleton(Configuration);
            services.AddSingleton(database);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fluxo de Caixa API V1");
            });

            app.UseMvc();
        }
    }
}
