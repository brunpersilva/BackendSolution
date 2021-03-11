using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class BackendContext : DbContext
    {
        public BackendContext(DbContextOptions<BackendContext> options)
           : base(options)
        {
        }
        public DbSet<Produto> Produtos { get; set; }
    }
}
