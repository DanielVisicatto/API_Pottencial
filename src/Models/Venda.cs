namespace PaymentAPI.src.Models
{
    public class Venda
    {
        public string NomeVendedor { get; set; }
        public DateTime Data { get; set; }
        public int IdPedido { get; set; }
        public EnumStatusVenda Status { get; set; }
        public List<Produto> Produtos { get; set; }

        public Venda()
        {            
        }       

        public Venda(string nomeVendedor, DateTime data)
        {
            NomeVendedor = nomeVendedor;
            Data = data;            
            Status = EnumStatusVenda.AguardandoPagamento;
            Produtos = new();
        }
    }
}
