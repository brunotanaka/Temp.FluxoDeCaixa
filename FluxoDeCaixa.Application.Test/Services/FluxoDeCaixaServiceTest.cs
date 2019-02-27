using FluxoDeCaixa.Application.Dominio;
using FluxoDeCaixa.Application.Dominio.Enums;
using FluxoDeCaixa.Application.Repositorio;
using FluxoDeCaixa.Application.Services;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FluxoDeCaixa.Application.Test.Services
{
    public class FluxoDeCaixaServiceTest
    {
        private IFluxoDeCaixaService fluxoDeCaixaService;
        private IRepositorio<ConsolidadoFluxo> repositorioConsolidado;
        private IRepositorio<LancamentoFinanceiro> repositorioLancamento;

        public void GerarMoqVazio()
        {
            var mockRepConsolidado = new Mock<IRepositorio<ConsolidadoFluxo>>();
            var mockRepLancamento = new Mock<IRepositorio<LancamentoFinanceiro>>();

            mockRepConsolidado.Setup(x => x.BuscarPor_Async(It.IsAny<FilterDefinition<ConsolidadoFluxo>>())).Returns(() => Task.FromResult(new ConsolidadoFluxo() { Data = DateTime.MinValue }));
            mockRepConsolidado.Setup(x => x.Buscar_Async(It.IsAny<FilterDefinition<ConsolidadoFluxo>>())).Returns(() => Task.FromResult(new List<ConsolidadoFluxo>().AsEnumerable()));
            mockRepConsolidado.Setup(x => x.Salvar_Async(It.IsAny<ConsolidadoFluxo>())).Returns(() => Task.FromResult(new ConsolidadoFluxo()));

            mockRepLancamento.Setup(x => x.BuscarPor_Async(It.IsAny<FilterDefinition<LancamentoFinanceiro>>())).Returns(() => Task.FromResult(new LancamentoFinanceiro() { Data = DateTime.MinValue.ToString("dd-MM-yyyy") }));
            mockRepLancamento.Setup(x => x.Buscar_Async(It.IsAny<FilterDefinition<LancamentoFinanceiro>>())).Returns(() => Task.FromResult(new List<LancamentoFinanceiro>().AsEnumerable()));
            mockRepLancamento.Setup(x => x.Salvar_Async(It.IsAny<LancamentoFinanceiro>())).Returns(() => Task.FromResult(new LancamentoFinanceiro()));

            fluxoDeCaixaService = new FluxoDeCaixaService(mockRepConsolidado.Object, mockRepLancamento.Object);
        }

        public void GerarMoqNegativo()
        {
            var mockRepConsolidado = new Mock<IRepositorio<ConsolidadoFluxo>>();
            var mockRepLancamento = new Mock<IRepositorio<LancamentoFinanceiro>>();

            mockRepConsolidado.Setup(x => x.BuscarPor_Async(It.IsAny<FilterDefinition<ConsolidadoFluxo>>())).Returns(() => Task.FromResult(new ConsolidadoFluxo()
            {
                Data = DateTime.Now,
                Encargos = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 2.5m) },
                Entradas = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 10m) },
                Saidas = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 8m) },
                PosicaoDia = "",
                Total = 2m,
                Id = Guid.NewGuid().ToString()
            }));
            mockRepConsolidado.Setup(x => x.Buscar_Async(It.IsAny<FilterDefinition<ConsolidadoFluxo>>())).Returns(() => Task.FromResult(new List<ConsolidadoFluxo>() { new ConsolidadoFluxo()
            {
                Data = DateTime.Now,
                Encargos = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 2.5m) },
                Entradas = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 10m) },
                Saidas = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 8m) },
                PosicaoDia = "",
                Total = 2m,
                Id = Guid.NewGuid().ToString()
            }}.AsEnumerable()));
            mockRepConsolidado.Setup(x => x.Salvar_Async(It.IsAny<ConsolidadoFluxo>())).Returns(() => Task.FromResult(new ConsolidadoFluxo()
            {
                Data = DateTime.Now,
                Encargos = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 2.5m) },
                Entradas = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 10m) },
                Saidas = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 8m) },
                PosicaoDia = "",
                Total = 2m,
                Id = Guid.NewGuid().ToString()
            }));

            mockRepLancamento.Setup(x => x.BuscarPor_Async(It.IsAny<FilterDefinition<LancamentoFinanceiro>>())).Returns(() => Task.FromResult(new LancamentoFinanceiro()
            {
                Conta = "01",
                Banco = "237",
                CpfCnpj = "421.058.748-60",
                Data = DateTime.Now.ToString("dd-MM-yyyy"),
                Descricao = "teste",
                Encargos = 1.25m,
                Lancamento = TipoLancamento.Pagamento,
                TipoConta = TipoConta.ContaCorrente,
                Valor = 20000m
            }));
            mockRepLancamento.Setup(x => x.Buscar_Async(It.IsAny<FilterDefinition<LancamentoFinanceiro>>())).Returns(() => Task.FromResult(new List<LancamentoFinanceiro>() { new LancamentoFinanceiro()
            {
                Conta = "01",
                Banco = "237",
                CpfCnpj = "421.058.748-60",
                Data = DateTime.Now.ToString("dd-MM-yyyy"),
                Descricao = "teste",
                Encargos = 1.25m,
                Lancamento = TipoLancamento.Pagamento,
                TipoConta = TipoConta.ContaCorrente,
                Valor = 20000m
            }}.AsEnumerable()));
            mockRepLancamento.Setup(x => x.Salvar_Async(It.IsAny<LancamentoFinanceiro>())).Returns(() => Task.FromResult(new LancamentoFinanceiro()
            {
                Conta = "01",
                Banco = "237",
                CpfCnpj = "421.058.748-60",
                Data = DateTime.Now.ToString("dd-MM-yyyy"),
                Descricao = "teste",
                Encargos = 1.25m,
                Lancamento = TipoLancamento.Pagamento,
                TipoConta = TipoConta.ContaCorrente,
                Valor = 20000m
            }));

            fluxoDeCaixaService = new FluxoDeCaixaService(mockRepConsolidado.Object, mockRepLancamento.Object);
        }
        public void GerarMoqPositivo()
        {
            var mockRepConsolidado = new Mock<IRepositorio<ConsolidadoFluxo>>();
            var mockRepLancamento = new Mock<IRepositorio<LancamentoFinanceiro>>();

            mockRepConsolidado.Setup(x => x.BuscarPor_Async(It.IsAny<FilterDefinition<ConsolidadoFluxo>>())).Returns(() => Task.FromResult(new ConsolidadoFluxo()
            {
                Data = DateTime.Now,
                Encargos = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 2.5m) },
                Entradas = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 10m) },
                Saidas = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 8m) },
                PosicaoDia = "",
                Total = 2m,
                Id = Guid.NewGuid().ToString()
            }));
            mockRepConsolidado.Setup(x => x.Buscar_Async(It.IsAny<FilterDefinition<ConsolidadoFluxo>>())).Returns(() => Task.FromResult(new List<ConsolidadoFluxo>() { new ConsolidadoFluxo()
            {
                Data = DateTime.Now,
                Encargos = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 2.5m) },
                Entradas = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 10m) },
                Saidas = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 8m) },
                PosicaoDia = "",
                Total = 2m,
                Id = Guid.NewGuid().ToString()
            }}.AsEnumerable()));
            mockRepConsolidado.Setup(x => x.Salvar_Async(It.IsAny<ConsolidadoFluxo>())).Returns(() => Task.FromResult(new ConsolidadoFluxo()
            {
                Data = DateTime.Now,
                Encargos = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 2.5m) },
                Entradas = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 10m) },
                Saidas = new List<DataValor>() { new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 8m) },
                PosicaoDia = "",
                Total = 2m,
                Id = Guid.NewGuid().ToString()
            }));

            mockRepLancamento.Setup(x => x.BuscarPor_Async(It.IsAny<FilterDefinition<LancamentoFinanceiro>>())).Returns(() => Task.FromResult(new LancamentoFinanceiro()
            {
                Conta = "01",
                Banco = "237",
                CpfCnpj = "421.058.748-60",
                Data = DateTime.Now.ToString("dd-MM-yyyy"),
                Descricao = "teste",
                Encargos = 1.25m,
                Lancamento = TipoLancamento.Recebimento,
                TipoConta = TipoConta.ContaCorrente,
                Valor = 10m
            }));
            mockRepLancamento.Setup(x => x.Buscar_Async(It.IsAny<FilterDefinition<LancamentoFinanceiro>>())).Returns(() => Task.FromResult(new List<LancamentoFinanceiro>() { new LancamentoFinanceiro()
            {
                Conta = "01",
                Banco = "237",
                CpfCnpj = "421.058.748-60",
                Data = DateTime.Now.ToString("dd-MM-yyyy"),
                Descricao = "teste",
                Encargos = 1.25m,
                Lancamento = TipoLancamento.Recebimento,
                TipoConta = TipoConta.ContaCorrente,
                Valor = 10m
            }}.AsEnumerable()));
            mockRepLancamento.Setup(x => x.Salvar_Async(It.IsAny<LancamentoFinanceiro>())).Returns(() => Task.FromResult(new LancamentoFinanceiro()
            {
                Conta = "01",
                Banco = "237",
                CpfCnpj = "421.058.748-60",
                Data = DateTime.Now.ToString("dd-MM-yyyy"),
                Descricao = "teste",
                Encargos = 1.25m,
                Lancamento = TipoLancamento.Recebimento,
                TipoConta = TipoConta.ContaCorrente,
                Valor = 10m
            }));

            fluxoDeCaixaService = new FluxoDeCaixaService(mockRepConsolidado.Object, mockRepLancamento.Object);
        }

        [Fact]
        public void TestesDeMetodos()
        {
            GerarMoqVazio();

            var dadosConsolidados = fluxoDeCaixaService.BuscaDadosConsolidados().Result;
            var dadoConsolidado1 = fluxoDeCaixaService.BuscaDadoConsolidado(DateTime.Now).Result;
            
            Assert.True(dadosConsolidados.Count() == 31);
            Assert.True(dadoConsolidado1.Data == DateTime.MinValue);
            Assert.ThrowsAny<AggregateException>(() => fluxoDeCaixaService.AdicionarLancamento(null).Result);
            
            GerarMoqNegativo();

            var lancamento1 = new LancamentoFinanceiro(1, "teste", "1", "237", 1, "421.058-748-60", "R$ 1.000,00", "R$ 12,00", DateTime.Now.ToString("dd-MM-yyyy"));
            var lancamento2 = new LancamentoFinanceiro(2, "teste", "1", "237", 1, "421.058-748-60", "R$ 1.000,00", "R$ 12,00", DateTime.Now.ToString("dd-MM-yyyy"));
            dadoConsolidado1 = fluxoDeCaixaService.AdicionarLancamento(lancamento1).Result;
            var dadoConsolidado2 = fluxoDeCaixaService.AdicionarLancamento(lancamento2).Result;

            Assert.ThrowsAny<System.Exception>(() => fluxoDeCaixaService.EfetuaLancamento(lancamento1).Result);
            Assert.True(dadoConsolidado1 != null);
            Assert.True(dadoConsolidado2 != null);

            GerarMoqPositivo();

            Assert.True(fluxoDeCaixaService.EfetuaLancamento(lancamento1).Result != null);
        }

        [Fact]
        public void Juros()
        {
            GerarMoqVazio();

            var juros1 = fluxoDeCaixaService.CalcularJuros(100m, 10);
            var juros2 = fluxoDeCaixaService.CalcularJuros(100m, 1);
            var juros3 = fluxoDeCaixaService.CalcularJuros(100m, 0.5m);
            var juros4 = fluxoDeCaixaService.CalcularJuros(100m, 100);
            var juros5 = fluxoDeCaixaService.CalcularJuros(100m, 50);

            Assert.True(juros1 == 10);
            Assert.True(juros2 == 1);
            Assert.True(juros3 == 0.5m);
            Assert.True(juros4 == 100);
            Assert.True(juros5 == 50);
        }

        [Fact]
        public void BalancoDiario()
        {
            GerarMoqVazio();

            var listaBalanco1 = new List<LancamentoFinanceiro>()
            {
                new LancamentoFinanceiro(2, "teste", "1", "237", 1, "421.058-748-60", "R$ 1.000,00", "R$ 12,00", DateTime.Now.ToString("dd-MM-yyyy")),
                new LancamentoFinanceiro(2, "teste", "1", "237", 1, "421.058-748-60", "R$ 1.323,20", "R$ 212,30", DateTime.Now.ToString("dd-MM-yyyy")),
                new LancamentoFinanceiro(1, "teste", "1", "237", 1, "421.058-748-60", "R$ 9,75", "R$ 1,99", DateTime.Now.ToString("dd-MM-yyyy")),
            };

            var listaBalanco2 = new List<LancamentoFinanceiro>()
            {
                new LancamentoFinanceiro(1, "teste", "1", "237", 1, "421.058-748-60", "R$ 1.000,00", "R$ 12,00", DateTime.Now.ToString("dd-MM-yyyy")),
                new LancamentoFinanceiro(1, "teste", "1", "237", 1, "421.058-748-60", "R$ 1.323,20", "R$ 212,30", DateTime.Now.ToString("dd-MM-yyyy")),
                new LancamentoFinanceiro(2, "teste", "1", "237", 1, "421.058-748-60", "R$ 9,75", "R$ 1,99", DateTime.Now.ToString("dd-MM-yyyy")),
            };

            var balanco1 = fluxoDeCaixaService.CalcularBalancoDiario(listaBalanco1);
            var balanco2 = fluxoDeCaixaService.CalcularBalancoDiario(listaBalanco2);

            Assert.True(balanco1 > 0);
            Assert.True(balanco1 == 2087.16m);
            Assert.True(balanco2 < 0);
            Assert.True(balanco2 == -2539.74m);
        }

        [Fact]
        public void PosicaoDiaria()
        {
            GerarMoqVazio();

            var posicao1 = fluxoDeCaixaService.CalcularPosicao(12, 15);
            var posicao2 = fluxoDeCaixaService.CalcularPosicao(40, 28);
            var posicao3 = fluxoDeCaixaService.CalcularPosicao(30, 24);
            var posicao4 = fluxoDeCaixaService.CalcularPosicao(80, 100);
            var posicao5 = fluxoDeCaixaService.CalcularPosicao(-10, -20);
            var posicao6 = fluxoDeCaixaService.CalcularPosicao(-20, -10);

            Assert.True(posicao1 == "25.00%");
            Assert.True(posicao2 == "-30.00%");
            Assert.True(posicao3 == "-20.00%");
            Assert.True(posicao4 == "25.00%");
            Assert.True(posicao5 == "-100.00%");
            Assert.True(posicao6 == "50.00%");
        }
    }
}
