using FelixWebsite.Core.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace FelixWebsite.Core.Models
{
    public class ContactForm
    {
        [Required(ErrorMessage = "gelieve uw naam in te vullen.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "gelieve een e-mailadres in te vullen.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "gelieve een bericht achter te laten.")]
        public string Message { get; set; }
        public string SecondHandBusinessBrandNodeId { get; set; }
        public string MailTo { get; set; }
        public string Business { get; set; }
        public string CarModel { get; set; }
        public string Url { get; set; }
    }
}
