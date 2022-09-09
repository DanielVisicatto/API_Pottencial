using Microsoft.EntityFrameworkCore;
using PaymentAPI.src.Models;

namespace PaymentAPI.src.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }

        public DbSet<Venda> Vendas{ get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Venda>(e =>
            {
                e.HasKey(e => e.IdPedido);
                e.HasMany(p => p.Produtos).WithOne().HasForeignKey(p => p.Id);
            });

            builder.Entity<Vendedor>(e =>   
            {
                e.HasKey(e => e.Id);
                e.HasMany(v => v.Vendas).WithOne().HasForeignKey(p => p.Produtos);
            });

            builder.Entity<Produto>(e =>
            {
                e.HasKey(e => e.Id);                
            });
        }
    }
}
