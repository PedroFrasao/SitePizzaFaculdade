using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitePizzaFaculdade.Models;
using System.Text.Json;

namespace SitePizzaFaculdade.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly PizzariaContext _context;

        public PedidosController(PizzariaContext context)
        {
            _context = context;
        }

        //  GET: api/pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            return await _context.Pedidos
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();
        }

        //  GET: api/pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == id);
            if (pedido == null) return NotFound();
            return pedido;
        }

        //  POST: api/pedidos/exportar
        [HttpPost("exportar")]
        public async Task<IActionResult> ExportarPedidos()
        {
            // 1. Busca todos os pedidos no banco
            var pedidos = await _context.Pedidos
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();

            // 2. Serializa para JSON (com indentação bonita)
            var json = JsonSerializer.Serialize(pedidos, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            // 3. Define o caminho completo do arquivo
            var caminho = @"C:\Users\Gabriel\Desktop\ENTEC\pedidos.json";

            // 4. Garante que a pasta exista
            var pasta = Path.GetDirectoryName(caminho);
            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta!);

            // 5. Salva o JSON no arquivo
            await System.IO.File.WriteAllTextAsync(caminho, json);

            // 6. Retorna resposta para o frontend
            return Ok(new { mensagem = $" Pedidos exportados para: {caminho}" });
        }
    }
}
