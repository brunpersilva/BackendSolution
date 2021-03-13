using System.Collections.Generic;


namespace Backend.Models.PaginacaoModels
{
    public class ResultadoBuscaProdutosModel
    {
        public ResultadoBuscaPaginadaModel Paginacao { get; set; }
        public List<Produto> Itens { get; set; } = new List<Produto>();
    }
}
