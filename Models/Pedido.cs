using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SitePizzaFaculdade.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required]
        public string NomeCliente { get; set; } = string.Empty;

        [Required]
        public string Endereco { get; set; } = string.Empty;

        [Required]
        public string Telefone { get; set; } = string.Empty;

        public DateTime DataPedido { get; set; } = DateTime.Now;

        [NotMapped]
        public List<Pizza> Pizzas { get; set; } = new List<Pizza>();

        [NotMapped]
        public decimal Total => Pizzas.Sum(p => p.Preco);
    }
}
