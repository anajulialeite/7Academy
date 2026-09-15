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
    }
}
