using FluxoDeCaixa.Application.Dominio;
using System;
using System.Collections.Generic;
using Xunit;

namespace FluxoDeCaixa.Application.Test.Dominio
{
    public class ConsulidadoFluxoTest
    {
        [Fact]
        public void ValoresDasPropriedades()
        {
            var consolidaoFluxo = new ConsolidadoFluxo() { Data = DateTime.Now, Encargos = new List<DataValor>(), Entradas = new List<DataValor>() { }, Saidas = new List<DataValor>(), PosicaoDia = "0", Total = 1m, Id = Guid.NewGuid().ToString() };

            consolidaoFluxo.Entradas.Add(new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 10));
            consolidaoFluxo.Saidas.Add(new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 2));

            consolidaoFluxo.Encargos.Add(new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 2));
            consolidaoFluxo.Encargos.Add(new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 1));

            Assert.False(string.IsNullOrEmpty(consolidaoFluxo.Id));
            Assert.True(consolidaoFluxo.Data.Date == DateTime.Now.Date);
            Assert.True(consolidaoFluxo.Entradas.Count == 1);
            Assert.True(consolidaoFluxo.Saidas.Count == 1);
            Assert.True(consolidaoFluxo.Encargos.Count == 2);
            Assert.True(consolidaoFluxo.Total == 1m);
            Assert.Same(consolidaoFluxo.PosicaoDia, "0");
        }
    }
}
