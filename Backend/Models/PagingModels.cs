using System;
using System.Collections.Generic;

namespace Backend.Models
{
    public class PagingInfo
    {
        public int PaginaAtual { get; set; }

        public int ItensPorPagina { get; set; }

        public Filtro Filtros { get; set; }

    }

    public class Filtro
    {
        public int FiltroId { get; set; }
        public string FiltroNome { get; set; }
    }
}