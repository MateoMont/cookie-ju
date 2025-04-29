using System;
using System.Collections.Generic;

namespace Dominio.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Now;
        public Estado Estado { get; set; } = Estado.Pendiente;

        // Relación con ItemPedido
        public List<ItemPedido> ItemPedidos { get; set; } = new();



        public decimal CalcularTotal()
        {
            return ItemPedidos.Sum(ip => ip.CalcularPrecio());
        }

    }

    public enum Estado
    {
        Pendiente,
        PagoConfirmado,
        Entregado,
        Cancelado
    }
 

}
