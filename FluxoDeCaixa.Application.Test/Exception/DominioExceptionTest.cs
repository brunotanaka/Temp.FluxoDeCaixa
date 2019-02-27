using FluxoDeCaixa.Application.Dominio.Enums;
using FluxoDeCaixa.Application.Util;
using Xunit;

namespace FluxoDeCaixa.Application.Test.Exception
{
    public class DominioExceptionTest
    {
        [Fact]
        public void Construtor()
        {
            var exception1 = new DominioException();
            var exception2 = new DominioException(ErrosSistemas.DataFormatoInvalido);
            var exception3 = new DominioException(ErrosSistemas.LancamentoRetroativo, new System.Exception("erro teste"));

            Assert.True(typeof(DominioException) == exception1.GetType());
            Assert.Equal(exception2.Codigo , (int)ErrosSistemas.DataFormatoInvalido);
            Assert.True(exception2.Message == ErrosSistemas.DataFormatoInvalido.GetDescription());
            Assert.Equal(exception3.Codigo , (int)ErrosSistemas.LancamentoRetroativo);
            Assert.True(exception3.Message == ErrosSistemas.LancamentoRetroativo.GetDescription());
        }
    }
}
