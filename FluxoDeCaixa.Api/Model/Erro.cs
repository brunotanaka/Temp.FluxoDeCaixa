namespace FluxoDeCaixa.Api.Model
{
    public class Erro
    {
        public int Codigo { get; set; }
        public string Mensagem { get; set; }
        public Erro(int codigo, string mensagem)
        {
            Codigo = codigo;
            Mensagem = mensagem;
        }
    }
}
