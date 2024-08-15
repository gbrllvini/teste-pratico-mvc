using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
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
            foreach (var pessoa in pessoas)
            {
                // entrega dados formatados
                pessoa.Cpf = FormataCpf(pessoa.Cpf);
                pessoa.Rg = FormataRg(pessoa.Rg);
            }

            return pessoas;
        }

        public async Task<Pessoa> Exists(Guid id, string acao)
        {
            var pessoa = await _context.Pessoas
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(p=> p.Id == id);

            if (acao != "excluir") {

                // entrega dados formatados
                pessoa.Cpf = FormataCpf(pessoa.Cpf);
                pessoa.Rg = FormataRg(pessoa.Rg);

            }

            return pessoa;
        }

        public async Task<Resposta> Create(Pessoa pessoa)
        {
            var cpfValido = await ValidaCpf(pessoa.Cpf);

            if (!cpfValido.Sucesso) return new Resposta { Sucesso = false, Mensagem = cpfValido.Mensagem };

            var rgValido = await ValidaRg(pessoa.Rg);

            if(!rgValido.Sucesso) return new Resposta { Sucesso = false, Mensagem = rgValido.Mensagem };

            pessoa.Id = Guid.NewGuid();

            // padroniza cadastro no backend
            pessoa.Nome = PadronizaCadastro(pessoa.Nome);
            pessoa.Sobrenome = PadronizaCadastro(pessoa.Sobrenome);

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
                var cpfValido = await ValidaCpf(pessoaEditada.Cpf);
                if (!cpfValido.Sucesso) return new Resposta { Sucesso = false, Mensagem = cpfValido.Mensagem };
            }

            if(pessoaAnterior.Rg != pessoaEditada.Rg)
            {
                var rgValido = await ValidaRg(pessoaEditada.Rg);
                if (!rgValido.Sucesso) return new Resposta { Sucesso = false, Mensagem = rgValido.Mensagem };
            }

            pessoaEditada.Nome = PadronizaCadastro(pessoaEditada.Nome);
            pessoaEditada.Sobrenome = PadronizaCadastro(pessoaEditada.Sobrenome);

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

        private async Task<Resposta> ValidaCpf (string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf;
            string digito;

            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11) return new Resposta { Sucesso = false, Mensagem = "O CPF precisa ter 11 números." }; 

            var existe = await _context.Pessoas.AnyAsync(p => p.Cpf == cpf);
            if (existe) return new Resposta { Sucesso = false, Mensagem = "CPF já cadastrado." }; 

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;

            if (resto < 2) resto = 0;
            else resto = 11 - resto;

            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;

            if (resto < 2) resto = 0;
            else resto = 11 - resto;

            digito = digito + resto.ToString();
            if(!cpf.EndsWith(digito)) return new Resposta { Sucesso = false, Mensagem = "CPF inválido." };
            return new Resposta { Sucesso = true, Mensagem = "CPF validado." };
        }

        private async Task<Resposta> ValidaRg (string rg) 
        {
            rg = rg.Trim();
            rg = rg.Replace(".", "").Replace("-", "");

            if (!Regex.IsMatch(rg, @"^\d+$")) return new Resposta { Sucesso = false, Mensagem = "Formato de RG inválido." }; 

            if (rg.Length != 9) return new Resposta { Sucesso = false, Mensagem = "O RG precisa ter 9 números." };

            var existe = await _context.Pessoas.AnyAsync(p => p.Rg == rg);
            if (existe) return new Resposta { Sucesso = false, Mensagem = "RG já cadastrado." };

            return new Resposta { Sucesso = true, Mensagem = "RG validado." }; ;
        }

        private static string PadronizaCadastro(string value)
        {
            return char.ToUpper(value[0]) + value.Substring(1);
        }

        private static string FormataCpf(string cpf)
        {
            return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
        }

        public static string FormataRg(string rg)
        {
            return $"{rg.Substring(0, 2)}.{rg.Substring(2, 3)}.{rg.Substring(5, 3)}-{rg.Substring(8, 1)}";
        }

    }
}