using System;
using FelixWebsite.Bdo.Models.Base;
using FelixWebsite.Core.enums;
using FelixWebsite.Shared.enums;

namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class UserInformation:BaseModel
    {
        private string name { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string Name
        {
            get
            {
                return !string.IsNullOrEmpty(name) ? name : $"{Firstname} {Lastname}";
            }
            set
            {
                name = value;
            }
        }


        public string Email { get; set; }
        public string Gsm { get; set; }
        public string LicensePlate { get; set; }
        public TyreState TyreState { get; set; }
        public Manual Manual { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsAcquisitionFlowCompleted { get; set; }
        public string AgentName { get; set; }

        public string AgentCity { get; set; }

        public string AgentCellphone { get; set; }

        public string AgentEmail { get; set; }

        public string Insurance { get; set; }

        public bool IsTakeOver { get; set; }

        public string TyreStateString => TyreState.GetText();
        public string ManualString => Manual.GetText();

        public bool IsInsured { get; set; }

        public string Chassisnumber { get; set; }

        public int UmbracoPageId { get; set; }

    }
}
