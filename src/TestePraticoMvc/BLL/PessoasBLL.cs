using Microsoft.Ajax.Utilities;
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
        private readonly PessoasContext _context = new PessoasContext();


        public async Task<List<Pessoa>> Get()
        {
            var pessoas = await _context.Pessoas
                                           .AsNoTracking()
                                           .ToListAsync();
            return pessoas;
        }

        public async Task<Pessoa> Exists(Guid id)
        {
            var pessoa = await _context.Pessoas
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(p=> p.Id == id);
            return pessoa;
        }

        public async Task<Resposta> Create(Pessoa pessoa)
        {
            var existeCpf = CpfExists(pessoa.Cpf);

            if (existeCpf) return new Resposta { Sucesso = false, Mensagem = "CPF já cadastrado." };

            var existeRg = RgExists(pessoa.Rg);

            if(existeRg) return new Resposta { Sucesso = false, Mensagem = "CPF já cadastrado." };

            pessoa.Id = Guid.NewGuid();

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            return new Resposta { Sucesso = true, Mensagem = "Pessoa criada com sucesso." }; 
        }

        public async Task<Resposta> Edit (Pessoa pessoaEditada)
        {
            Pessoa pessoaAnterior = await _context.Pessoas
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync(p => p.Id == pessoaEditada.Id);

            if(pessoaAnterior.Cpf != pessoaEditada.Cpf)
            {
                var existeCpf = CpfExists(pessoaEditada.Cpf);
                if (existeCpf) return new Resposta { Sucesso = false, Mensagem = "CPF já cadastrado." };
            }

            if(pessoaAnterior.Rg != pessoaEditada.Rg)
            {
                var existeRg = RgExists(pessoaEditada.Rg);
                if (existeRg) return new Resposta { Sucesso = false, Mensagem = "CPF já cadastrado." };
            }
    
            _context.Entry(pessoaEditada).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return new Resposta { Sucesso = true, Mensagem = "Pessoa editada com sucesso." };
        }

        public async Task<Resposta> Delete(Guid id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null) return new Resposta { Sucesso = false, Mensagem = "Essa pessoa não existe ou já foi removida." };

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            return new Resposta { Sucesso = true, Mensagem = "Pessoa removida com sucesso." };
        }

        public void Dispose()
        {
            _context.Dispose();
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