using FelixWebsite.Core.App_GlobalResources;
using System.ComponentModel.DataAnnotations;

namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class SchemeDamageData
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string ImageType { get; set; }

        public bool IsSelected { get; set; }

        public bool Damage { get; set; }

        public bool HeavyDamage { get; set; }

        public bool Marks { get; set; }

        public bool HailDamage { get; set; }

        public bool Dent { get; set; }

        public string Remarks { get; set; }

        [Required(ErrorMessageResourceType = typeof(FelixResources), ErrorMessageResourceName = "overname_errormessage_is_insured")]
        public bool IsInsured { get; set; }
    }
}
