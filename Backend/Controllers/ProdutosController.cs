using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly BackendContext _context;

        public ProdutosController(BackendContext context)
        {
            _context = context;
        }

        // GET: api/Produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            var output = await _context.Produtos.ToListAsync();
            return output;
        }

        // GET: api/Produtos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        // POST: api/Produtos
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Produto>>> PostProduto(PagingInfo pageinfo)
        {
            if (pageinfo.PaginaAtual == 0)
            {
                pageinfo.PaginaAtual = 1;
            }
            if (pageinfo.ItensPorPagina == 0)
            {
                pageinfo.ItensPorPagina = 10;
            }


            int skip = (pageinfo.PaginaAtual - 1) * pageinfo.ItensPorPagina;

            var produtos = new List<Produto>();

            if (pageinfo.Filtros.FiltroId <= 0)
            {
                produtos = await _context.Produtos                                
                                .Where(x => x.Nome.Contains(pageinfo.Filtros.FiltroNome ?? ""))
                                .OrderBy(p => p.Id)                                
                                .ToListAsync();
            }
            else
            {
                produtos = await _context.Produtos.ToListAsync();
            }

            int totalitens = produtos.Count();
            int totalPaginas = totalitens / pageinfo.ItensPorPagina;

            produtos = produtos.Skip(skip).Take(pageinfo.ItensPorPagina).ToList();

            return Ok(new
            {
                Data = produtos,
                Paginação = new
                {
                    PaginaAtual = pageinfo.PaginaAtual,
                    TotalItems = totalitens,
                    TotalPaginas = totalPaginas

                }
            }); ;
        }

        // DELETE: api/Produtos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Produto>> DeleteProduto(long id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return produto;
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }

    }

}
