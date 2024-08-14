using System.ComponentModel.DataAnnotations.Schema;


namespace TestePraticoMvc.Utils
{
    [NotMapped]
    public class Resposta
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = "";
    }
}