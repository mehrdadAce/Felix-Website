using AutoMapper;
using FelixWebsite.Bdo.Models.Acquisition;
using FelixWebsite.Bll.Services.Base;
using FelixWebsite.Bll.Services.Interfaces;
using FelixWebsite.Dal.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Logging;

namespace FelixWebsite.Bll.Services
{
    public class AcquisitionInformationService : BaseService<UserInformation, Dal.Entities.UserInformation>, IAcquisitionInformationService
    {
        public AcquisitionInformationService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IEnumerable<UserInformation> GetAllCompletedAcquisition()
        {
            try
            {
                var entitiesCompletedForAcquisition = Repository.GetAll().Where(x => x.IsAcquisitionFlowCompleted && x.IsTakeOver);
                return Mapper.Map<IEnumerable<UserInformation>>(entitiesCompletedForAcquisition);
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something went wrong while getting all completed entities for acquisition: ", e);
                return null;
            }
        }

        public IEnumerable<UserInformation> GetAllCompletedDamages()
        {
            try
            {
                var entitiesCompletedForDamages = Repository.GetAll().Where(x => x.IsAcquisitionFlowCompleted && !x.IsTakeOver);
                return Mapper.Map<IEnumerable<UserInformation>>(entitiesCompletedForDamages);
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something went wrong while getting all completed entities for damages: ", e);
                return null;
            }
        }
    }
}