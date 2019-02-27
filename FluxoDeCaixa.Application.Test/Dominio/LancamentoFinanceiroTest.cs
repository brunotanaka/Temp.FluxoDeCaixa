using FluxoDeCaixa.Application.Dominio;
using FluxoDeCaixa.Application.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FluxoDeCaixa.Application.Test.Dominio
{
    public class LancamentoFinanceiroTest
    {
        [Fact]
        public void Construtor()
        {
            Assert.ThrowsAny<DominioException>(() => new LancamentoFinanceiro(0, "Lançamento Teste", "01", "237", 1, "42105874860", "R$ 1.000,00", "R$ 12,53", DateTime.Now.ToString("dd-MM-yyyy")));
            Assert.ThrowsAny<DominioException>(() => new LancamentoFinanceiro(1, "Lançamento Teste", "01", "237", 0, "42105874860", "R$ 1.000,00", "R$ 12,53", DateTime.Now.ToString("dd-MM-yyyy")));
            Assert.ThrowsAny<DominioException>(() => new LancamentoFinanceiro(1, "Lançamento Teste", "01", "237", 1, "42105874860", "R$ 1.000,00", "R$ 12,53", DateTime.Now.ToString("dd/MM/yyyy")));
            Assert.ThrowsAny<DominioException>(() => new LancamentoFinanceiro(1, "Lançamento Teste", "01", "237", 1, "42105874860", "R$ 1.000,00", "R$ 12,53", DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy")));
            Assert.ThrowsAny<DominioException>(() => new LancamentoFinanceiro(1, "Lançamento Teste", "01", "237", 1, "42105874860", "teste", "R$ 12,53", DateTime.Now.ToString("dd-MM-yyyy")));
            Assert.ThrowsAny<DominioException>(() => new LancamentoFinanceiro(1, "Lançamento Teste", "01", "237", 1, "42105874860", "1.000,00", "teste", DateTime.Now.ToString("dd-MM-yyyy")));
        }

        [Fact]
        public void ValoresDasPropriedades()
        {
            var lancamento1 = new LancamentoFinanceiro(1, "Lançamento Teste", "01", "237", 1, "421.058.748-60", "R$ 1.000,00", "R$ 12,53", DateTime.Now.ToString("dd-MM-yyyy"));
            var lancamento2 = new LancamentoFinanceiro(2, "Lançamento Teste", "01", "237", 2, "421.058.748-60", "R$ 1.000.000,00", "R$ 1.002,53", DateTime.Now.ToString("dd-MM-yyyy"));
            var lancamento3 = new LancamentoFinanceiro(2, "Lançamento Teste", "01", "237", 1, "421.058.748-60", "R$ 1,20", "R$ 0,23", DateTime.Now.ToString("dd-MM-yyyy"));
            var lancamento4 = new LancamentoFinanceiro(1, "Lançamento Teste", "01", "237", 2, "421.058.748-60", "R$ 650,00", "R$ 38,23", DateTime.Now.ToString("dd-MM-yyyy"));

            Assert.True(lancamento1.CpfCnpj == "42105874860");
            Assert.Same(lancamento1.Descricao, "Lançamento Teste");
            Assert.Same(lancamento1.Conta, "01");
            Assert.Same(lancamento1.Banco, "237");
            Assert.Equal(lancamento2.Data, DateTime.Now.ToString("dd-MM-yyyy"));
            Assert.Equal(lancamento3.Data, DateTime.Now.ToString("dd-MM-yyyy"));
            Assert.Equal(lancamento4.Data, DateTime.Now.ToString("dd-MM-yyyy"));
            Assert.True(lancamento1.Valor == 1000m && lancamento1.Encargos == 12.53m);
            Assert.True(lancamento2.Valor == 1000000m && lancamento2.Encargos == 1002.53m);
            Assert.True(lancamento3.Valor == 1.2m && lancamento3.Encargos == 0.23m);
            Assert.True(lancamento4.Valor == 650m && lancamento4.Encargos == 38.23m);
            Assert.True(lancamento1.Lancamento == TipoLancamento.Pagamento && lancamento1.TipoConta == TipoConta.ContaCorrente);
            Assert.True(lancamento2.Lancamento == TipoLancamento.Recebimento && lancamento2.TipoConta == TipoConta.Poupanca);
            Assert.True(lancamento3.Lancamento == TipoLancamento.Recebimento && lancamento3.TipoConta == TipoConta.ContaCorrente);
            Assert.True(lancamento4.Lancamento == TipoLancamento.Pagamento && lancamento4.TipoConta == TipoConta.Poupanca);
        }
    }
}
