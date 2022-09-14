using System.ComponentModel.DataAnnotations;

namespace PaymentAPI.src.Models
{
    public class Vendedor
    {
        public int Id { get; set; }
        public string Cpf { get; set; }
        [Required]
        public string Nome { get; set; }
        public string Email { get; set; }
        public int Telefone { get; set; }
        public List<Venda> Vendas { get; set; }

        public Vendedor()
        {            
        }

        public Vendedor(string cpf, string nome, string email, int telefone)
        {
            
            Cpf = cpf;
            Nome = nome;
            Email = email;
            Telefone = telefone;
            Vendas = new();
        }
    }
}
