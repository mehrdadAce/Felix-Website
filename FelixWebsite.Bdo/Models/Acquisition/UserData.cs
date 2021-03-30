using FelixWebsite.Core.App_GlobalResources;
using System.ComponentModel.DataAnnotations;

namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class UserData
    {
        //[Required(ErrorMessageResourceType = typeof(FelixResources),ErrorMessageResourceName = "overname_errormessage_firstname")]
        //public string Firstname { get; set; }

        //[Required(ErrorMessageResourceType = typeof(FelixResources), ErrorMessageResourceName = "overname_errormessage_lastname")]
        //public string Lastname { get; set; }

        [Required(ErrorMessageResourceType = typeof(FelixResources), ErrorMessageResourceName = "overname_errormessage_first_lastname")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(FelixResources), ErrorMessageResourceName = "overname_errormessage_email")]
        [EmailAddress]
        public string Email { get; set; }
       
        [Required(ErrorMessageResourceType = typeof(FelixResources), ErrorMessageResourceName = "overname_errormessage_gsm")]
        public string Gsm { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(FelixResources), ErrorMessageResourceName = "overname_errormessage_licenseplate")]
        public string LicensePlate { get; set; }

        //[Required(ErrorMessageResourceType = typeof(FelixResources), ErrorMessageResourceName = "overname_errormessage_chassisnumber")]
        public string ChassisNumber { get; set; }

        // Wordt enkel in frontend gebruikt
        public int TabIndex { get; set; }

    }
}
