using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TestePraticoMvc.BLL.Validacoes
{
    public class RgAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var rg = value as string;

            if (!IsRgValid(rg))
            {
                return new ValidationResult("O RG é inválido.");
            }

            return ValidationResult.Success;
        }

        private bool IsRgValid(string rg)
        {
            rg = rg.Trim();
            rg = rg.Replace(".", "").Replace("-", "");

            if (!Regex.IsMatch(rg, @"^\d+$")) return false;

            if (rg.Length != 9) return false;

            return true;
        }
    }
}