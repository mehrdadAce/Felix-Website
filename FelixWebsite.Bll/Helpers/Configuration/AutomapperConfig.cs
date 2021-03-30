using AutoMapper;
using FelixWebsite.Bdo.Models;
using FelixWebsite.Bdo.Models.Acquisition;

namespace FelixWebsite.Bll.Helpers.Configuration
{
    public class AutomapperConfig
    {
        public static void ConfigureMapper()
        {
            Mapper.Configuration.AddProfile<CustomConfig>();

        }
    }

    public class CustomConfig : Profile
    {
        protected override void Configure()
        {
            CreateMap<Dal.Entities.UserInformation, UserInformation>();
            CreateMap<UserInformation, Dal.Entities.UserInformation>()
                .ForMember(src => src.CreatedDate, des => des.Ignore());
            CreateMap<UserInformation, UserDataTakeOver>()
                 .ForMember(userData => userData.PossibleManualStates, userInf => userInf.Ignore())
                 .ForMember(userData => userData.PossibleTyreStates, userInf => userInf.Ignore());
            CreateMap<UserInformation, UserDataDamage>();
            CreateMap<UserDataTakeOver, UserInformation>()
                .ForMember(userInf => userInf.Id, userData => userData.Ignore());
            CreateMap<UserDataDamage, UserInformation>()
                .ForMember(userInf => userInf.Id, userData => userData.Ignore());
            CreateMap<PhotoInfo, Dal.Entities.PhotoInfo>().ReverseMap();
            CreateMap<SchemeDamageData, SchemeDamage>().ReverseMap();
            CreateMap<SchemeDamage, Dal.Entities.SchemeDamage>().ReverseMap();
            CreateMap<OverviewDamageModel, OverviewModel>().ReverseMap();
            base.Configure();
        }
    }
}
