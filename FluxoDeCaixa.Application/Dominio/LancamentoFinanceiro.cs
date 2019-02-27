using FluxoDeCaixa.Application.Dominio.Enums;
using System;
using System.Text.RegularExpressions;

namespace FluxoDeCaixa.Application.Dominio
{
    public class LancamentoFinanceiro : Entidade
    {
        public TipoLancamento Lancamento { get; set; }
        public string Descricao { get; set; }
        public string Conta { get; set; }
        public string Banco { get; set; }
        public TipoConta TipoConta { get; set; }
        public string CpfCnpj { get; set; }
        public decimal Valor { get; set; }
        public decimal Encargos { get; set; }
        public string Data { get; set; }

        public LancamentoFinanceiro(int lancamento, string descricao, string conta, string banco, int tipoConta, string cpfCnpj, string valor, string encargos, string data)
        {
            Regex somenteNumeros = new Regex(@"[^\d]");
            var _data = DateTime.Now.Date;
            var _valor = 0m;
            var _encargos = 0m;

            if (lancamento == 0)
                throw new DominioException(ErrosSistemas.TipoLancamentoInvalido);

            if (tipoConta == 0)
                throw new DominioException(ErrosSistemas.TipoContaInvalida);

            if (!DateTime.TryParseExact(data, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out _data))
                throw new DominioException(ErrosSistemas.DataFormatoInvalido);

            if (_data < DateTime.Now.Date)
                throw new DominioException(ErrosSistemas.LancamentoRetroativo);

            if (!decimal.TryParse(valor.Replace("R$", "").Replace(".", "").Trim(), out _valor))
                throw new DominioException(ErrosSistemas.ValorLancamentoInvalido);

            if (!decimal.TryParse(encargos.Replace("R$", "").Replace(".", "").Trim(), out _encargos))
                throw new DominioException(ErrosSistemas.ValorEncargoInvalido);
            
            Descricao = descricao;
            Conta = conta;
            Banco = banco;
            CpfCnpj = somenteNumeros.Replace(cpfCnpj, "");
            Valor = _valor;
            Encargos = _encargos;
            Lancamento = (TipoLancamento)lancamento;
            TipoConta = (TipoConta)tipoConta;
            Data = data;
        }

        public LancamentoFinanceiro()
        {

        }
    }
}
