using Microsoft.EntityFrameworkCore;
using Dominio.Models;

namespace Dominio.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Galleta> Galletas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItemPedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Guarda el enum EstadoPedido como texto
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Estado)
                .HasConversion<string>();

            // Relación: Pedido -> ItemPedido
            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Pedido)
                .WithMany(p => p.ItemPedidos)
                .HasForeignKey(ip => ip.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación: Galleta -> ItemPedido
            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Galleta)
                .WithMany(g => g.ItemPedidos)
                .HasForeignKey(ip => ip.GalletaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Precarga de Galletas
            modelBuilder.Entity<Galleta>().HasData(
                new Galleta { Id = 1, Sabor = "Orégano", Precio = 100 },
                new Galleta { Id = 2, Sabor = "Semillas", Precio = 100 },
                new Galleta { Id = 3, Sabor = "Queso", Precio = 120 },
                new Galleta { Id = 4, Sabor = "Comunes", Precio = 80 },
                new Galleta { Id = 5, Sabor = "Ajo", Precio = 100 }
            );
        }
    }
}
