using System;
using FelixWebsite.Dal.Entities.Base;
using FelixWebsite.Core.enums;
using FelixWebsite.Shared.enums;

namespace FelixWebsite.Dal.Entities
{
    public class UserInformation : BaseEntity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        public string Gsm { get; set; }
        public string LicensePlate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Remarks { get; set; }
        public TyreState TyreState { get; set; }
        public Manual Manual { get; set; }
        public bool IsAcquisitionFlowCompleted { get; set; } = false;
        public string AgentName { get; set; }

        public string AgentCity { get; set; }

        public string AgentCellphone { get; set; }

        public string AgentEmail { get; set; }

        public string Insurance { get; set; }

        public bool IsInsured { get; set; }

        public bool IsTakeOver { get; set; }

        public string Chassisnumber { get; set; }

        public int UmbracoPageId { get; set; }


        public UserInformation()
        {
            CreatedDate = DateTime.Now;
            AuditDeleted = false;
        }
    }
}