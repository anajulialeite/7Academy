using System;

namespace EventManager.Web.Models
{
    /// <summary>
    /// Registro de cada tentativa de prova feita pelo aluno.
    /// Usado para calcular o tempo de espera (30 min) após reprovação.
    /// </summary>
    public class QuizAttempt
    {
        public int Id { get; set; }

        public int EventRegistrationId { get; set; }
        public EventRegistration? EventRegistration { get; set; }

        /// <summary>Quantidade de acertos (0 a 10).</summary>
        public int Score { get; set; }

        public bool Passed { get; set; }

        public DateTime AttemptDate { get; set; } = DateTime.Now;
    }
}
