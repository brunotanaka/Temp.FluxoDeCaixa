using FluxoDeCaixa.Application.Dominio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Api.Model
{
    public class ConsolidadoModel
    {
        public string data { get; set; }

        public List<DataValorModel> entradas { get; set; }

        public List<DataValorModel> saidas { get; set; }

        public List<DataValorModel> encargos { get; set; }

        public string total { get; set; }

        public string posicao_do_dia { get; set; }

        public ConsolidadoModel(ConsolidadoFluxo consolidadoFluxo)
        {
            var _total = consolidadoFluxo.Total == 0m ? "0,00" :  consolidadoFluxo.Total.ToString("#,#0.00");

            data = consolidadoFluxo.Data.ToString("dd-MM-yyyy");
            total = $"R$ {_total}";
            posicao_do_dia = consolidadoFluxo.PosicaoDia;

            entradas = new List<DataValorModel>();
            saidas = new List<DataValorModel>();
            encargos = new List<DataValorModel>();

            foreach (var item in consolidadoFluxo.Entradas)
                entradas.Add(new DataValorModel(item.Data, item.Valor));

            foreach (var item in consolidadoFluxo.Saidas)
                saidas.Add(new DataValorModel(item.Data, item.Valor));

            foreach (var item in consolidadoFluxo.Encargos)
                encargos.Add(new DataValorModel(item.Data, item.Valor));
        }
    }
}
