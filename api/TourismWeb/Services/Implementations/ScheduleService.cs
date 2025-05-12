using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TourismWeb.DTOs.Schedule;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Services.Implementations
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IMapper _mapper;

        public ScheduleService(IScheduleRepository scheduleRepository, IMapper mapper)
        {
            _scheduleRepository = scheduleRepository;
            _mapper = mapper;
        }

        public async Task<ScheduleDTO> CreateSchedule(ScheduleCreateDTO createDTO)
        {
            var addedSchedule = await _scheduleRepository.CreateSchedule(createDTO);
            return _mapper.Map<ScheduleDTO>(addedSchedule);
        }

        public async Task<IEnumerable<ScheduleCalendarDTO>> GetCalendarData(string tourID)
        {
            var result = await _scheduleRepository.GetSchedulesByTourID(tourID);
            return _mapper.Map<IEnumerable<ScheduleCalendarDTO>>(result);
        }

        public async Task<ScheduleBookingDTO> GetBookingData(string scheID)
        {
            var result = await _scheduleRepository.GetScheduleDataForBooking(scheID);
            return result;
        }

        public async Task<bool> UpdateForCancelBooking(ScheduleUpdateBookingCancelDTO data)
        {
            return await _scheduleRepository.UpdateScheduleForBookingCancel(data);
        }

        public async Task<IEnumerable<ScheduleDataTableDTO>> GetScheduleWithTourNameAsync()
        {
            try
            {
                var result = await _scheduleRepository.GetScheduleWithTourNameAsync();
                if (result == null) throw new Exception("Không tìm thấy dữ liệu");
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateScheduleDiscountAsync(string scheduleId, double discount)
        {
            await _scheduleRepository.UpdateScheduleDiscountAsync(scheduleId, discount);
        }
    }
}
