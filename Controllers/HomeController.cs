using Microsoft.AspNetCore.Mvc;
using SitePizzaFaculdade.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace SitePizzaFaculdade.Controllers
{
    public class HomeController : Controller
    {
        private readonly PizzariaContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        // Cardápio fixo em memória
        private static List<Pizza> cardapio = new List<Pizza>
{
    new Pizza {
        Id = 1,
        Nome = "Mussarela",
        Descricao = "Molho de tomate, mussarela derretida e orégano",
        Preco = 35.00m,
        ImagemUrl = "https://images.unsplash.com/photo-1604068549290-dea0e4a305ca?w=300&h=200&fit=crop&crop=center"
    },
    new Pizza {
        Id = 2,
        Nome = "Calabresa",
        Descricao = "Molho, mussarela e calabresa fatiada",
        Preco = 38.00m,
        ImagemUrl = "https://images.unsplash.com/photo-1595708684082-a173bb3a06c5?w=300&h=200&fit=crop&crop=center"
    },
    new Pizza {
        Id = 3,
        Nome = "Portuguesa",
        Descricao = "Molho, mussarela, presunto, ovos, azeitonas e cebola",
        Preco = 42.00m,
        ImagemUrl = "https://images.unsplash.com/photo-1593560708920-61dd98c46a4e?w=300&h=200&fit=crop&crop=center"
    },
    new Pizza {
        Id = 4,
        Nome = "Frango com Catupiry",
        Descricao = "Molho, mussarela, frango desfiado e catupiry",
        Preco = 45.00m,
        ImagemUrl = "https://images.unsplash.com/photo-1593246049226-ded77bf90326?w=300&h=200&fit=crop&crop=center"
    },
    new Pizza {
        Id = 5,
        Nome = "Margherita",
        Descricao = "Molho, mussarela, tomate fresco e manjericão",
        Preco = 40.00m,
        ImagemUrl = "https://images.unsplash.com/photo-1604068549290-dea0e4a305ca?w=300&h=200&fit=crop&crop=center"
    },
    new Pizza {
        Id = 6,
        Nome = "Quatro Queijos",
        Descricao = "Mussarela, provolone, parmesão e gorgonzola",
        Preco = 48.00m,
        ImagemUrl = "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?w=300&h=200&fit=crop&crop=center"
    }
};

        private static List<Pizza> carrinho = new List<Pizza>();

        public HomeController(PizzariaContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cardapio()
        {
            ViewBag.CarrinhoCount = carrinho.Count;
            return View(cardapio);
        }

        [HttpPost]
        public ActionResult AdicionarAoCarrinho(int pizzaId)
        {
            var pizza = cardapio.FirstOrDefault(p => p.Id == pizzaId);
            if (pizza != null)
            {
                carrinho.Add(pizza);
            }
            return RedirectToAction("Cardapio");
        }

        public ActionResult Carrinho()
        {
            ViewBag.CarrinhoCount = carrinho.Count;
            return View(carrinho);
        }

        [HttpPost]
        public async Task<ActionResult> FinalizarPedido(string nomeCliente, string endereco, string telefone)
        {
            if (carrinho.Any())
            {
                var pedido = new Pedido
                {
                    NomeCliente = nomeCliente,
                    Endereco = endereco,
                    Telefone = telefone,
                    DataPedido = DateTime.Now,
                    Pizzas = new List<Pizza>(carrinho)
                };

                // ?? Gravar pedido no banco
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                // Agora chamar a API para exportar os pedidos
                var client = _httpClientFactory.CreateClient();
                var url = "https://localhost:44321/api/pedidos/exportar"; // URL da sua API

                // Envia a requisição POST para exportar os pedidos
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    // Resposta bem-sucedida, você pode informar ao usuário que os pedidos foram exportados
                    ViewBag.MensagemExportacao = "Pedidos exportados com sucesso!";
                }
                else
                {
                    // Caso não consiga exportar
                    ViewBag.MensagemExportacao = "Erro ao exportar os pedidos.";
                }

                carrinho.Clear(); // Limpa o carrinho após a finalização
                ViewBag.PedidoId = pedido.Id;
                ViewBag.Total = pedido.Total;

                return View("Confirmação");
            }

            return RedirectToAction("Cardapio");
        }

        public ActionResult LimparCarrinho()
        {
            carrinho.Clear();
            return RedirectToAction("Cardapio");
        }
    }
}
