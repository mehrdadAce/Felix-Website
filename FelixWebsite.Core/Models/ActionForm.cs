using FelixWebsite.Bdo.Enums;
using System.ComponentModel.DataAnnotations;
using Umbraco.Core.Models;

namespace FelixWebsite.Core.Models
{
    public class ActionForm
    {
        public ActionForm()
        {
            Phone = "+32";
        }
        public ActionForm(IPublishedContent page, ActionType type)
        {
            Page = page;
            Type = type;
            Phone = "+32";
        }

        public IPublishedContent Page { get; set; }

        [Required(ErrorMessage = "gelieve voornaam in te vullen.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "gelieve een achternaam in te vullen.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "gelieve een telefoonnummer in te vullen.")]
        [RegularExpression(@"^(?:(((\+|00)32\s?|0)4(60|[789]\d)(\s?\d{2}){3}))|(?:(((\+|00)32\s?|0)(\d\s?\d{3}|\d{2}\s?\d{2})(\s?\d{2}){2}))$", ErrorMessage = "gelieve een geldig telefoonnummer op te geven dat begint met +32")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "gelieve een e-mailadres in te vullen.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public ActionType Type { get; set; }

        [Required(ErrorMessage = "gelieve een model te selecteren.")]
        public string ModelName { get; set; }
        public string ModelLink { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public int StoreId { get; set; }

        public string BusinessMailModal { get; set; }
        public string SecondHandBusinessBrandNodeId { get; set; }
    }
}
