using System;
using System.Collections.Generic;

namespace Backend.Models
{
    public class RequisicaoBuscaPaginadaModel
    {
        public int PaginaAtual { get; set; }

        public int ItensPorPagina { get; set; }
    }

    public class ResultadoBuscaPaginadaModel
    {
        public int PaginaAtual { get; set; }
        public int TotalItens { get; set; }
        public int TotalPaginas { get; set; }

    }

    public class RequisicaoBuscaProdutosModel
    {
        public RequisicaoBuscaPaginadaModel PaginaRequisicao { get; set; }
        public int FiltroId { get; set; }
        public string FiltroNome { get; set; }
    }

    public class ResultadoBuscaProdutosModel
    {
        public ResultadoBuscaPaginadaModel Paginacao { get; set; }
        public List<Produto> Itens { get; set; } = new List<Produto>();
    }


}