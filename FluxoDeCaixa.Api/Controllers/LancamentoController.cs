using FluxoDeCaixa.Api.Model;
using FluxoDeCaixa.Application;
using FluxoDeCaixa.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Api.Controllers
{
    [Route("api/[controller]")]
    public class LancamentoController : Controller
    {
        private readonly IFluxoDeCaixaService _fluxoDeCaixaService;
        private readonly IConfigurationRoot _configuration;
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

        public LancamentoController(IFluxoDeCaixaService fluxoDeCaixaService, IConfigurationRoot configuration)
        {
            _fluxoDeCaixaService = fluxoDeCaixaService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var model = new List<ConsolidadoModel>();

            try
            {
                var dados = await _fluxoDeCaixaService.BuscaDadosConsolidados();

                foreach (var dado in dados)
                    model.Add(new ConsolidadoModel(dado));

                return Ok(model);
            }
            catch (DominioException ex)
            {
                return BadRequest($"{ex.Codigo} - {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]LancamentoModel value)
        {
            try
            {
                var lancamento = value.GerarLancamento();
                await _fluxoDeCaixaService.EfetuaLancamento(lancamento);
                _fluxoDeCaixaService.EnviaParaFila(lancamento, _connection);

                return Ok("Lancamento efetuado com sucesso !");
            }
            catch (DominioException ex)
            {
                return BadRequest($"{ex.Codigo} - {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}