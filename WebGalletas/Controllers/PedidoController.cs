using Microsoft.AspNetCore.Mvc;
using Dominio.Models;
using Dominio.Data;
using WebGalletas.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Text.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebGalletas.Controllers
{
    public class PedidoController : Controller
    {
        private readonly AppDbContext _context;

        private readonly Sistema _sistema;
        private readonly string _adminPassword = "tati";

        public PedidoController(AppDbContext context)
        {
            _sistema = new Sistema(context); // 👈 Solo usamos Sistema
            _context = context;
        }

        public IActionResult IngresarClave()
        {
            return View();
        }

        public async Task<IActionResult> VerPedidos(string password, Estado? estadoFiltro)
        {
            if (password != _adminPassword)
                return Unauthorized("Clave secreta incorrecta.");

            var pedidos = await _sistema.ObtenerPedidosFiltradosAsync(estadoFiltro);
            ViewBag.EstadoSeleccionado = estadoFiltro;
            return View(pedidos);
        }

        public async Task<IActionResult> HistorialPorTelefono(string telefono, string password)
        {
            if (password != _adminPassword)
                return Unauthorized("Clave secreta incorrecta.");

            var pedidosEntregados = await _sistema.ObtenerPedidosEntregadosPorTelefonoAsync(telefono);
            ViewBag.Telefono = telefono;
            return View("VerPedidos", pedidosEntregados);
        }

        public IActionResult Confirmacion()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(IFormCollection form)
        {
            var cliente = form["Cliente"];
            var telefono = form["Telefono"];
            var descripcion = form["Descripcion"];
            var itemsJson = form["ItemsJson"];

            var items = JsonSerializer.Deserialize<List<ItemDto>>(itemsJson);

            var pedido = new Pedido
            {
                Cliente = cliente,
                Telefono = telefono,
                Descripcion = descripcion,
                Fecha = DateTime.Now,
                ItemPedidos = new List<ItemPedido>()
            };

            var galletaIds = items.Select(i => i.GalletaId).ToList();
            var galletas = await _context.Galletas
                .Where(g => galletaIds.Contains(g.Id))
                .ToListAsync();

            foreach (var i in items)
            {
                var galleta = galletas.FirstOrDefault(g => g.Id == i.GalletaId);
                if (galleta == null) continue;

                var precioFinal = galleta.Precio;
                if (i.Integral)
                {
                    precioFinal += 40;
                }

                var item = new ItemPedido
                {
                    GalletaId = i.GalletaId,
                    Cantidad = i.Cantidad,
                    Integral = i.Integral,
                    TieneSal = i.TieneSal
                };

                pedido.ItemPedidos.Add(item);
            }

            await _sistema.AgregarPedidoAsync(pedido);

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> ActualizarEstado(int id, Estado nuevoEstado)
        {
            var exito = await _sistema.CambiarEstadoPedidoAsync(id, nuevoEstado);
            if (!exito)
                return NotFound();

            TempData["Clave"] = _adminPassword;
            return RedirectToAction("VerPedidos", new { password = _adminPassword });
        }


    }
}
