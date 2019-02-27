using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FluxoDeCaixa.Agent
{
    public class Startup
    {
        public IServiceProvider ServicesProvider;
        public IConfigurationRoot Configuration;

        public Startup()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Setup(environmentName);
            configure();
        }

        private void configure()
        {
            var di = new DependencyInjection(Configuration);
            ServicesProvider = di.ServicesProvider;
        }

        public void Setup(string environmentName)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: false)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
