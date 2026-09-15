using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventManager.Web.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Título")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Descrição")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Data e Hora")]
        public DateTime DateTime { get; set; }

        [Required]
        [Display(Name = "Carga Horária (em horas)")]
        public int WorkloadHours { get; set; }

        [Required]
        [Display(Name = "Limite de Vagas")]
        public int TotalSlots { get; set; }

        [Required]
        public string OrganizerId { get; set; } = string.Empty;
        
        public ApplicationUser? Organizer { get; set; }

        public ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();
    }
}
