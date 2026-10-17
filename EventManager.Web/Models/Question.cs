using System.ComponentModel.DataAnnotations;

namespace EventManager.Web.Models
{
    /// <summary>
    /// Questão de múltipla escolha vinculada a um evento (prova de certificação).
    /// </summary>
    public class Question
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event? Event { get; set; }

        [Required]
        [Display(Name = "Enunciado")]
        public string Text { get; set; } = string.Empty;

        [Required] public string OptionA { get; set; } = string.Empty;
        [Required] public string OptionB { get; set; } = string.Empty;
        [Required] public string OptionC { get; set; } = string.Empty;
        [Required] public string OptionD { get; set; } = string.Empty;

        /// <summary>Alternativa correta: "A", "B", "C" ou "D".</summary>
        [Required]
        [StringLength(1)]
        public string CorrectOption { get; set; } = "A";
    }
}
