namespace FluxoDeCaixa.Api.Model
{
    public class DataValorModel
    {
        public string data { get; set; }
        public string valor { get; set; }
        public DataValorModel(string Data, decimal Valor)
        {
            var _valor = Valor == 0m ? "0,00" : Valor.ToString("#,#0.00");
            
            data = Data;
            valor = $"R$ {_valor}";
        }
    }
}
