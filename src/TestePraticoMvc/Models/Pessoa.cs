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
        [StringLength(30, MinimumLength = 2, ErrorMessage = "o campo {0} precisa ter entre {2} e {1} caracteres.")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "O campo {0} está em formato incorreto.")]
        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Selecione uma opção válida para o campo {0}.")]
        [Display(Name = "Estado Civil")]
        public string EstadoCivil { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O campo {0} precisa ter {1} caracteres.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "O campo {0} precisa ter {1} caracteres.")]
        public string Rg { get; set; }
    }
}