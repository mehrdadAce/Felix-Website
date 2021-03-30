using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Models.ViewModels
{
    public class CarRentalForm
    {
        public CarRentalForm() { }
        public CarRentalForm(CarRentalType carRentalType)
        {
            var umbracoHelper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);
            var groupHome = umbracoHelper.AssignedContentItem.AncestorOrSelf(1).OfType<GroupHome>();
            var businesses = groupHome.Children().OfType<Businesses>().FirstOrDefault().Children().OfType<BusinessBrand>().ToList();

            List<SelectListItem> businessList = new List<SelectListItem>();

            foreach (var item in businesses)
            {
                businessList.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.BusinessName + " " + item.BusinessLocation });
            }
            this.PhoneNumber = "+32";
            this.Businesses = businessList;
            this.CarType = carRentalType;
        }

        [Required(ErrorMessage = "Gelieve uw voornaam in te vullen.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Gelieve uw achternaam in te vullen.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gelieve een geldig e-mailadres in te vullen.")]
        [EmailAddress]
        public string Emailadress { get; set; }
        
        // RegEx for normal phone numbers & mobile phone numbers in Belgium.
        [Required(ErrorMessage = "gelieve een geldige gsm-nummer in te vullen.")]
        [RegularExpression(@"^(?:(((\+|00)32\s?|0)4(60|[789]\d)(\s?\d{2}){3}))|(?:(((\+|00)32\s?|0)(\d\s?\d{3}|\d{2}\s?\d{2})(\s?\d{2}){2}))$", ErrorMessage = "gelieve een geldig telefoonnummer op te geven dat begint met +32")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gelieve een vestiging te selecteren.")]
        public int BusinessId { get; set; }
        public List<SelectListItem> Businesses { get; set; }

        [Required(ErrorMessage = "Gelieve een type wagen door te geven.")]
        public CarRentalType CarType { get; set; }

        [Required(ErrorMessage = "Gelieve een startdatum op te geven.")]
        public string DateRange { get; set; }
    }
}


