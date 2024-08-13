using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TestePraticoMvc.Models;

namespace TestePraticoMvc.Data
{
    public class PessoasContext : DbContext
    {
        public PessoasContext() : base("PessoasContext")
        {
        }

        public DbSet<Pessoa> Pessoas { get; set; }
    }
}