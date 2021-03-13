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

        // POST: api/Produtos
        [HttpPost("Buscarprodutos")]
        public async Task<ActionResult> PostProduto(RequisicaoBuscaProdutosModel model)
        {
            //CHECK INICIAL 
            if (model.FiltroId <= 0 && string.IsNullOrEmpty(model.FiltroNome))
            {
                return BadRequest("Id e ou Nome invalidos");
            }

            //Caso PaginaAtual e ItensPorPagina venham vazio, setamos o default
            if (model.Paginacao.PaginaAtual == 0)
            {
                model.Paginacao.PaginaAtual = 1;
            }
            if (model.Paginacao.ItensPorPagina == 0)
            {
                model.Paginacao.ItensPorPagina = 10;
            }

            //Objeto que ira carregar a query
            IQueryable<ProdutoModel> query = null;

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
            int skip = (model.Paginacao.PaginaAtual - 1) * model.Paginacao.ItensPorPagina;

            //resultado da query List<Produtos> sendo passado para itens
            var itens = await query.Skip(skip).Take(model.Paginacao.ItensPorPagina).ToListAsync();

            //Calculando total de items na lista de produtos
            int totalItens = await query.CountAsync();

            //Calculo do numero de paginas que é obtido pela divisão do totalItens pelo numero de ItensPorPagina e arredondado para cima
            int totalPaginas = Convert.ToInt32(Math.Ceiling((double)totalItens / model.Paginacao.ItensPorPagina));

            //Checando se a pagina solicitada é maior que o total de paginas calculado
            if (model.Paginacao.PaginaAtual > totalPaginas)
            {
                return BadRequest($"O numero total de paginas é {totalPaginas}, não é possivel retornar a pagina {model.Paginacao.PaginaAtual}");
            }

            //Retornando Objeto ResultadoBuscaProdutosModel contendo a lista de produtos e dados de Paginação
            return Ok(new ResultadoBuscaProdutosModel
            {
                Paginacao = new ResultadoBuscaPaginadaModel
                {
                    PaginaAtual = model.Paginacao.PaginaAtual,
                    TotalItens = totalItens,
                    TotalPaginas = totalPaginas
                },

                Itens = itens

            });
        }

        // GET: api/Produtos/5
        [HttpGet("ObterProduto/{id}")]
        public async Task<ActionResult<ProdutoModel>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        [HttpPost]
        [Route("SalvarProduto")]
        public async Task<ActionResult<ProdutoModel>> PostProduto(ProdutoModel produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }

        // DELETE: api/Produtos/5

        [HttpDelete("ExcluirProduto/{id}")]
        public async Task<ActionResult<ProdutoModel>> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound($"Não existe produto com o id {id}");
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return produto;
        }
    }
}
