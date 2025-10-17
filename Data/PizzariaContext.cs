using Microsoft.EntityFrameworkCore;

namespace SitePizzaFaculdade.Models
{
    public class PizzariaContext : DbContext
    {
        public PizzariaContext(DbContextOptions<PizzariaContext> options)
            : base(options) { }

        public DbSet<Pedido> Pedidos { get; set; }
    }
}