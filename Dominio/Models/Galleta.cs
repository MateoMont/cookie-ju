using System.Collections.Generic;

namespace Dominio.Models
{
    public class Galleta
    {
        public int Id { get; set; }

        // 'Sabor' en lugar de 'Nombre'
        public string Sabor { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        // Relación con los detalles de pedidos
        public ICollection<ItemPedido> ItemPedidos { get; set; } = new List<ItemPedido>();

        public Galleta(int id, string sabor, decimal precio)
        {
            Id = id;
            Sabor = sabor;
            Precio = precio;
        }

        public Galleta() { }
    }
}
