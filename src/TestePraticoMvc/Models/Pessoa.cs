using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestePraticoMvc.Models
{
    public class Pessoa
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string EstadoCivil { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
    }
}