namespace Dominio.Models
{
    public class ItemPedido
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }

        // Claves foráneas
        public int GalletaId { get; set; }
        public Galleta Galleta { get; set; } = null!;

        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;

        // Propiedades derivadas por pedido individual
        public bool Integral { get; set; }
        public bool TieneSal { get; set; }

        public ItemPedido(int cantidad, Galleta galleta, bool integral, bool tieneSal)
        {
            Cantidad = cantidad;
            Galleta = galleta;
            GalletaId = galleta.Id;
            Integral = integral;
            TieneSal = tieneSal;
        }

        public ItemPedido() { }
        public decimal CalcularPrecio()
        {
            decimal precioUnitario = Galleta.Precio;

            if (Integral)
            {
                precioUnitario += 40;
            }

            return precioUnitario * Cantidad;
        }

    }
}
