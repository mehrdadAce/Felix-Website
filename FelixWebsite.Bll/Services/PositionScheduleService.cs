using System.Collections.Generic;
using FelixWebsite.Bdo.Models.JobOffer;
using FelixWebsite.Bll.Services.Interfaces;

namespace FelixWebsite.Bll.Services
{
    public class PPositionScheduleService : IPositionScheduleService
    {
        public List<PositionSchedule> GetPositionSchedules()
        {
            return new List<PositionSchedule>()
            {
                new PositionSchedule() {Code = "x:Day Work", Beschrijving = "Dagwerk" },
                new PositionSchedule() {Code = "x:Night Work", Beschrijving = "Nachtwerk" },
                new PositionSchedule() {Code = "x:Day And Night", Beschrijving = "Dag- en nachtwerk" },
                new PositionSchedule() {Code = "x:Weekend", Beschrijving = "Weekendwerk" },
                new PositionSchedule() {Code = "x:2 Shift System", Beschrijving = "2 shiften systeem" },
                new PositionSchedule() {Code = "x:3 Shift System", Beschrijving = "3 shiften systeem" },
                new PositionSchedule() {Code = "x:Interrupted Service", Beschrijving = "onderbroken dienst" }
            };
        }
        public List<PositionSchedule> GetPositionScheduleType()
        {
            return new List<PositionSchedule>()
            {
                new PositionSchedule() {Code = "Full Time", Beschrijving = "Voltijds" },
                new PositionSchedule() {Code = "Part Time", Beschrijving = "Deeltijds" }
            };
        }
    }
}
