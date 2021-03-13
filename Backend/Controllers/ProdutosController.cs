using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Models.PaginacaoModels;

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
        public async Task<ActionResult> PostProduto(RequisicaoBuscaProdutosModel model)
        {
            //CHECK INICIAL 
            if(model.FiltroId <= 0 && string.IsNullOrEmpty(model.FiltroNome))
            {
                return BadRequest("Id e ou Nome invalidos");
            }

            //SENAO VIER PREENCHIDO, SETAMOS O DEFAULT
            if (model.PaginaRequisicao.PaginaAtual == 0)
            {
                model.PaginaRequisicao.PaginaAtual = 1;
            }
            if (model.PaginaRequisicao.ItensPorPagina == 0)
            {
                model.PaginaRequisicao.ItensPorPagina = 10;
            }

            //Objeto que ira carregar a query
            IQueryable<Produto> query = null;

            //Setando query de acordo com FiltroId
            if (model.FiltroId > 0)
            {
                query = _context.Produtos
                                .Where(x => x.Id == model.FiltroId);                
            }

            //Setando query de acordo FiltroNome
            if (!string.IsNullOrEmpty(model.FiltroNome) && model.FiltroId <= 0)
            {
                query = _context.Produtos
                                .Where(x => x.Nome.Contains(model.FiltroNome ?? ""));
            }

            //Calculando quantos items se pula na query dependendo na quantidade de ItensPorPagina e PaginaAtual
            int skip = (model.PaginaRequisicao.PaginaAtual - 1) * model.PaginaRequisicao.ItensPorPagina;

            //resultado da query List<Produtos> sendo passado para itens
            var itens = await query.Skip(skip).Take(model.PaginaRequisicao.ItensPorPagina).ToListAsync();

            //Calculando total de items na lista de produtos
            int totalItens = await query.CountAsync();

            //Calculo do numero de paginas que é obtido pela divisão do totalItens pelo numero de ItensPorPagina e arredondado para cima
            int totalPaginas = Convert.ToInt32(Math.Ceiling((double)totalItens / model.PaginaRequisicao.ItensPorPagina));

            //Checando se a pagina solicitada é maior que o total de paginas calculado
            if (model.PaginaRequisicao.PaginaAtual > totalPaginas)
            {
                return BadRequest($"O numero total de paginas é {totalPaginas}, não é possivel retornar a pagina {model.PaginaRequisicao.PaginaAtual}");
            }

            //Retornando Objeto ResultadoBuscaProdutosModel contendo a lista de produtos e dados de Paginação
            return Ok(new ResultadoBuscaProdutosModel
            {
                Paginacao = new ResultadoBuscaPaginadaModel
                {
                    PaginaAtual = model.PaginaRequisicao.PaginaAtual,
                    TotalItens = totalItens,
                    TotalPaginas = totalPaginas
                },

                Itens = itens

            }); 
        }

        [HttpPost]
        [Route("SalvarProduto")]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
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
