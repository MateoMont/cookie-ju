using Dominio.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Dominio.Data
{
    public class Sistema
    {
  
        private readonly AppDbContext _context;
        private const string ClaveSecreta = "tati";

        public Sistema(AppDbContext context)
        {
            _context = context;
        }

        public async Task AgregarPedidoAsync(Pedido pedido)
        {
            try
            {
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();
                Console.WriteLine("Pedido guardado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el pedido: {ex.Message}");
            }
        }
        public async Task<Galleta?> ObtenerGalletaPorIdAsync(int id)
        {
            return await _context.Galletas.FindAsync(id);
        }


        public async Task<List<Pedido>> ObtenerPedidosAsync()
        {
            return await _context.Pedidos
                .Include(p => p.ItemPedidos)
                    .ThenInclude(ip => ip.Galleta)
                .ToListAsync();
        }

        public async Task<List<Pedido>> BuscarPedidosPorCelularAsync(string celular)
        {
            return await _context.Pedidos
                .Include(p => p.ItemPedidos)
                .Where(p => p.Telefono == celular)
                .ToListAsync();
        }

        public async Task<bool> CambiarEstadoPedidoAsync(int pedidoId, Estado nuevoEstado)
        {
            var pedido = await _context.Pedidos.FindAsync(pedidoId);
            if (pedido != null)
            {
                pedido.Estado = nuevoEstado;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }



        public async Task<List<Pedido>> ObtenerPedidosFiltradosAsync(Estado? estado)
        {
            var query = _context.Pedidos
                .Include(p => p.ItemPedidos)
                .ThenInclude(ip => ip.Galleta)
                .AsQueryable();

            if (estado != null)
                query = query.Where(p => p.Estado == estado);

            return await query.ToListAsync();
        }

        public async Task<List<Pedido>> ObtenerPedidosEntregadosPorTelefonoAsync(string telefono)
        {
            return await _context.Pedidos
                .Include(p => p.ItemPedidos)
                .ThenInclude(ip => ip.Galleta)
                .Where(p => p.Estado == Estado.Entregado && p.Telefono == telefono)
                .ToListAsync();
        }

  

    }
}
