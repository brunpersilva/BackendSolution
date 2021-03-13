namespace Backend.Models.PaginacaoModels
{
    public class RequisicaoBuscaProdutosModel
    {
        public RequisicaoBuscaPaginadaModel PaginaRequisicao { get; set; }
        public int FiltroId { get; set; }
        public string FiltroNome { get; set; }
    }
}
