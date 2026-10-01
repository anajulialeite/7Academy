using System;
using System.ComponentModel.DataAnnotations;

namespace EventManager.Web.Models
{
    public class EventRegistration
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event? Event { get; set; }

        [Required]
        public string ParticipantId { get; set; } = string.Empty;
        public ApplicationUser? Participant { get; set; }

        [Display(Name = "Data da Inscrição")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [Display(Name = "Presença Confirmada")]
        public bool IsPresenceConfirmed { get; set; } = false;

        [Display(Name = "Nota (Avaliação)")]
        [Range(1, 10)]
        public int? Rating { get; set; }

        [Display(Name = "Feedback")]
        [StringLength(500)]
        public string? Feedback { get; set; }
    }
}