using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestePraticoMvc.Models
{
    public class Pessoa
    {
        [Key]
        public Guid Id { get; set; }

        [Required (ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "o campo {0} precisa ter entre {2} e {1} caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "o campo {0} precisa ter entre {1} e {2} caracteres.")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "O campo {0} está em formato incorreto.")]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Selecione uma opção válida para o campo {0}.")]
        [Display(Name = "Estado Civil")]
        public string EstadoCivil { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
    }
}