using FluxoDeCaixa.Application.Dominio.Enums;
using FluxoDeCaixa.Application.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluxoDeCaixa.Application
{
    public class DominioException : Exception
    {
        public int Codigo { get; set; }

        public DominioException()
        {

        }

        public DominioException(ErrosSistemas erro) : base(erro.GetDescription())
        {
            Codigo = (int)erro;
        }

        public DominioException(ErrosSistemas erro, Exception innerException) : base(erro.GetDescription(), innerException)
        {
            Codigo = (int)erro;
        }
    }
}
