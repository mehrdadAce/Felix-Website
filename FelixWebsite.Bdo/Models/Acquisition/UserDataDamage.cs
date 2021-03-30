using FelixWebsite.Bdo.Models.Validation;
using FelixWebsite.Core.App_GlobalResources;
using System.ComponentModel.DataAnnotations;

namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class UserDataDamage : UserData
    {
        [Required(ErrorMessageResourceType = typeof(FelixResources), ErrorMessageResourceName = "overname_errormessage_agent_name")]
        public bool IsInsured { get; set; }

        //[RequiredIf(nameof(IsInsured), ErrorMessageResourceType = typeof(FelixResources), ErrorMessageResourceName = "overname_errormessage_agent_name")]
        public string AgentName { get; set; }

        //[RequiredIf(nameof(IsInsured), ErrorMessageResourceType = typeof(FelixResources), ErrorMessageResourceName = "overname_errormessage_agent_city")]
        public string AgentCity { get; set; }

        //[RequiredIf(nameof(IsInsured), ErrorMessageResourceType = typeof(FelixResources), ErrorMessageResourceName = "overname_errormessage_agent_cellphone")]
        public string AgentCellphone { get; set; }

        //[RequiredIf(nameof(IsInsured), ErrorMessageResourceType = typeof(FelixResources), ErrorMessageResourceName = "overname_errormessage_agent_email")]
        //[EmailAddress]
        public string AgentEmail { get; set; }

        [RequiredIf(nameof(IsInsured), ErrorMessageResourceType = typeof(FelixResources), ErrorMessageResourceName = "overname_errormessage_insurance")]
        public string Insurance { get; set; }

        public string InsuranceErrorMessage => FelixResources.overname_errormessage_insurance;
    }
}