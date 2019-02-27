using FluxoDeCaixa.Application.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FluxoDeCaixa.Application.Test.Dominio
{
    public class DataValorTest
    {
        [Fact]
        public void ValoresDasPropriedades()
        {
            var dataValor1 = new DataValor(DateTime.Now.ToString("dd-MM-yyyy"), 10m);
            var dataValor2 = new DataValor("08-12-2018", 10m);
            var dataValor3 = new DataValor("teste", -30m);

            Assert.Equal(Convert.ToDateTime(dataValor1.Data), DateTime.Now.Date);
            Assert.True(dataValor1.Valor == 10m);
            Assert.Same(dataValor2.Data, "08-12-2018");
            Assert.Same(dataValor3.Data, "teste");
        }
    }
}
