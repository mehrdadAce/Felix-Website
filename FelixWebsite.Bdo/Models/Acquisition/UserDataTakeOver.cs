using System.Collections.Generic;
using FelixWebsite.Core.App_GlobalResources;
using System.ComponentModel.DataAnnotations;
using FelixWebsite.Shared.enums;
using System.Web.Mvc;
using FelixWebsite.Core.enums;

namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class UserDataTakeOver: UserData
    {
        [Required(ErrorMessageResourceType = typeof(FelixResources), ErrorMessageResourceName = "overname_errormessage_wheelstate")]
        public TyreState TyreState { get; set; }

        [Required(ErrorMessageResourceType = typeof(FelixResources), ErrorMessageResourceName = "overname_errormessage_manual")]
        public Manual Manual { get; set; }

        public IEnumerable<SelectListItem> PossibleManualStates => new List<SelectListItem>
        {
            new SelectListItem
            {
                Text = Manual.Volledig.GetText(),
                Value = Manual.Volledig.ToString()
            },
            new SelectListItem
            {
                Text = Manual.Gedeeltelijk.GetText(),
                Value = Manual.Gedeeltelijk.ToString()
            },
            new SelectListItem
            {
                Text = Manual.Niet.GetText(),
                Value = Manual.Niet.ToString()
            }
        };

        public IEnumerable<SelectListItem> PossibleTyreStates => new List<SelectListItem>
        {
            new SelectListItem
            {
                Text = TyreState.Goed.GetText(),
                Value = TyreState.Goed.ToString()
            },
            new SelectListItem
            {
                Text = TyreState.Matig.GetText(),
                Value = TyreState.Matig.ToString()
            },
            new SelectListItem
            {
                Text = TyreState.Slecht.GetText(),
                Value = TyreState.Slecht.ToString()
            }
        };
    }
}
