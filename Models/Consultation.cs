using System;
using System.ComponentModel.DataAnnotations;

namespace DrKchida.Models
{
    public class Consultation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le téléphone est requis")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Le sujet est requis")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Le message est requis")]
        public string Message { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        
        public bool IsRead { get; set; } = false;
    }
}
