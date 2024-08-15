using System.Data.Entity;
using TestePraticoMvc.Models;

namespace TestePraticoMvc.DAL
{
    public class PessoasContext : DbContext
    {
        public PessoasContext() : base("PessoasContext")
        {
        }

        public DbSet<Pessoa> Pessoas { get; set; }
    }
}