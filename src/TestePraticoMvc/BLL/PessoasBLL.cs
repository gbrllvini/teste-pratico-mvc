using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TestePraticoMvc.DAL;
using TestePraticoMvc.Models;
using TestePraticoMvc.Utils;

namespace TestePraticoMvc.BLL
{
    public class PessoasBLL
    {
        private readonly PessoasContext _context;

        public PessoasBLL(PessoasContext context)
        {
            _context = context;
        }

        public async Task<List<Pessoa>> Get()
        {
            var pessoas = await _context.Pessoas.ToListAsync();
            return pessoas;
        }

        public async Task<Pessoa> Exists(Guid id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            return pessoa;
        }

        public Resposta Create(Pessoa pessoa)
        {
            var existeCpf = CpfExists(pessoa.Cpf);

            if (existeCpf) return new Resposta { Sucesso = false, Mensagem = "CPF já cadastrado." };

            var existeRg = RgExists(pessoa.Rg);

            if(existeRg) return new Resposta { Sucesso = false, Mensagem = "CPF já cadastrado." };

            pessoa.Id = Guid.NewGuid();

            _context.Pessoas.Add(pessoa);
            _context.SaveChanges();

            return new Resposta { Sucesso = true, Mensagem = "" }; 
        }

        public Resposta Edit (Pessoa pessoa)
        {
            var existeCpf = CpfExists(pessoa.Cpf);

            if (existeCpf) return new Resposta { Sucesso = false, Mensagem = "CPF já cadastrado." };

            var existeRg = RgExists(pessoa.Rg);

            if (existeRg) return new Resposta { Sucesso = false, Mensagem = "CPF já cadastrado." };

            _context.Entry(pessoa).State = EntityState.Modified;
            _context.SaveChanges();

            return new Resposta { Sucesso = true, Mensagem = "" };
        }
        private bool CpfExists (string cpf)
        {
            var existe = _context.Pessoas.Any(p => p.Cpf == cpf);
            return existe;
        }

        private bool RgExists (string rg) 
        {
            var existe = _context.Pessoas.Any(p=>p.Rg == rg);
            return existe;
        }

    }
}