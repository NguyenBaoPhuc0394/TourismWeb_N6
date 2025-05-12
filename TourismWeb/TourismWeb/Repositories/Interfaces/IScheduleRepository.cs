using TourismWeb.DTOs.Schedule;
using TourismWeb.Models;

namespace TourismWeb.Repositories.Interfaces
{
    public interface IScheduleRepository
    {
        Task<Schedule> CreateSchedule(Schedule schedule);

        Task<IEnumerable<Schedule>> GetSchedulesByTourID(string tourID);
        Task<ScheduleBookingDTO> GetScheduleDataForBooking(string tourID);
        Task<bool> UpdateScheduleForBookingCancel(ScheduleUpdateBookingCancelDTO data);
    }
}
