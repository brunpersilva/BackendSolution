using System.Collections.Generic;


namespace Backend.Models.PaginacaoModels
{
    public class ResultadoBuscaProdutosModel
    {
        public ResultadoBuscaPaginadaModel Paginacao { get; set; }
        public List<ProdutoModel> Itens { get; set; } = new List<ProdutoModel>();
    }
}
