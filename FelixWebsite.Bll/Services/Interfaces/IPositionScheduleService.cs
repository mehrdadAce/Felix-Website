using FelixWebsite.Bdo.Models.JobOffer;
using System.Collections.Generic;

namespace FelixWebsite.Bll.Services.Interfaces
{
    public interface IPositionScheduleService
    {
        List<PositionSchedule> GetPositionSchedules();
        List<PositionSchedule> GetPositionScheduleType();
    }
}
