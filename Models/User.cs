using System.ComponentModel.DataAnnotations;

namespace ElegantWebFormApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele complet este obligatoriu.")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Emailul este obligatoriu.")]
        [EmailAddress(ErrorMessage = "Adresa de email nu este validă.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Numărul de telefon este obligatoriu.")]
        [Phone(ErrorMessage = "Număr de telefon invalid.")]
        public string PhoneNumber { get; set; }
    }
}
