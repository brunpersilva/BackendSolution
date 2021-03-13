using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class BackendContext : DbContext
    {
        public BackendContext(DbContextOptions<BackendContext> options)
           : base(options)
        {
        }
        public DbSet<ProdutoModel> Produtos { get; set; }
    }
}
