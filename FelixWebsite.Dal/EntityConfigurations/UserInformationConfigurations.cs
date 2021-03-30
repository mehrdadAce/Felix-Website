using FelixWebsite.Dal.Entities;

namespace FelixWebsite.Dal.EntityConfigurations
{
    public class UserInformationConfigurations : BaseEntityConfigurations<UserInformation>
    {
        public UserInformationConfigurations()
        {
            Property(ent => ent.Firstname)
                .IsOptional();
            Property(ent => ent.Lastname)
                .IsOptional();
            Property(ent => ent.Email)
                .IsRequired();
            Property(ent => ent.Gsm)
                .IsRequired();
            Property(ent => ent.LicensePlate)
                .IsRequired();
            Property(ent => ent.CreatedDate)
                .IsRequired();
            Property(ent => ent.Remarks)
                .IsOptional();
            Property(ent => ent.IsAcquisitionFlowCompleted)
                .IsRequired();
            Property(ent => ent.TyreState)
                .IsOptional();
            Property(ent => ent.Manual)
                .IsOptional();
            Property(ent => ent.AgentName)
               .IsOptional();
            Property(ent => ent.AgentCity)
               .IsOptional();
            Property(ent => ent.AgentCellphone)
               .IsOptional();
            Property(ent => ent.AgentEmail)
               .IsOptional();
            Property(ent => ent.Insurance)
                .IsOptional();
            Property(ent => ent.IsTakeOver)
               .IsOptional();
            Property(ent => ent.IsInsured)
                .IsRequired();
            Property(ent => ent.Chassisnumber)
                .IsOptional();
            Property(ent => ent.UmbracoPageId)
                .IsRequired();
            Property(ent => ent.Name)
                .IsRequired();
        }
    }
}