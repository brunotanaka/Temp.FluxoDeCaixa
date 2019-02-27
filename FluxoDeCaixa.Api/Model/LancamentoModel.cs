using FluxoDeCaixa.Application;
using FluxoDeCaixa.Application.Dominio;
using System;

namespace FluxoDeCaixa.Api.Model
{
    public class LancamentoModel
    {
        public int tipo_da_lancamento { get; set; }
        public string descricao { get; set; }
        public string conta_destino { get; set; }
        public string banco_destino { get; set; }
        public int tipo_de_conta { get; set; }
        public string cpf_cnpj_destino { get; set; }
        public string valor_do_lancamento { get; set; }
        public string encargos { get; set; }
        public string data_de_lancamento { get; set; }

        public LancamentoModel()
        {

        }

        public virtual LancamentoFinanceiro GerarLancamento()
        {
            try
            {
                return new LancamentoFinanceiro(tipo_da_lancamento, descricao, conta_destino, banco_destino, tipo_de_conta, cpf_cnpj_destino, valor_do_lancamento, encargos, data_de_lancamento);
            }
            catch (DominioException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
