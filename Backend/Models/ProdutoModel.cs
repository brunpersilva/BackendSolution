using System;
namespace Backend.Models
{
    public class ProdutoModel
    {
        public int Id { get; private set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public DateTime Data { get; set; }
    }
}
