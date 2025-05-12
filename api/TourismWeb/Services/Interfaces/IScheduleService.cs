using TourismWeb.DTOs.Schedule;

namespace TourismWeb.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<ScheduleDTO> CreateSchedule(ScheduleCreateDTO createDTO);

        Task<IEnumerable<ScheduleCalendarDTO>> GetCalendarData(string tourID);

        Task<ScheduleBookingDTO> GetBookingData(string scheID);

        Task<bool> UpdateForCancelBooking(ScheduleUpdateBookingCancelDTO data);

        Task<IEnumerable<ScheduleDataTableDTO>> GetScheduleWithTourNameAsync();

        Task UpdateScheduleDiscountAsync(string scheduleId, double discount);


    }
}
