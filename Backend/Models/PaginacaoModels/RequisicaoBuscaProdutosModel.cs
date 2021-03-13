namespace Backend.Models.PaginacaoModels
{
    public class RequisicaoBuscaProdutosModel
    {
        public RequisicaoBuscaPaginadaModel Paginacao { get; set; }
        public int FiltroId { get; set; }
        public string FiltroNome { get; set; }
    }
}
