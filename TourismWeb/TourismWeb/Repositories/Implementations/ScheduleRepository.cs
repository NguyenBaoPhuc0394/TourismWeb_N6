using Microsoft.EntityFrameworkCore;
using TourismWeb.Data;
using TourismWeb.DTOs.Schedule;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;


namespace TourismWeb.Repositories.Implementations
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly TourismDbContext _context;

        public ScheduleRepository(TourismDbContext context)
        {
            _context = context;
        }

        public async Task<Schedule> CreateSchedule(Schedule schedule)
        {
            var result = _context.Schedules.FromSqlRaw("EXEC CreateSchedule " +
                "@Tour_Id = {0}, @StartDate = {1}, @AdultPrice = {2}, @ChildrenPrice = {3}, @Discount = {4}",
                schedule.Tour_Id, schedule.Start_date, schedule.Adult_price, schedule.Children_price, schedule.Discount)
                .AsEnumerable()
                .FirstOrDefault();

            if (result == null)
            {
                throw new Exception("Schedule not found!");
            }
            return result;
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByTourID(string tourID)
        {
            var result = await _context.Schedules.Where(x => x.Tour_Id == tourID).ToListAsync();
            return result;
        }

        public async Task<ScheduleBookingDTO> GetScheduleDataForBooking(string scheID)
        {
            var result = await _context.Schedules
                .Where(s => s.Id == scheID)
                .Select(s => new ScheduleBookingDTO
                {
                    Id = s.Id,
                    TourName = s.Tour.Name,
                    Duration = s.Tour.Duration,
                    Available = s.Available,
                    AdultPrice = s.Adult_price,
                    ChildPrice = s.Children_price
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> UpdateScheduleForBookingCancel(ScheduleUpdateBookingCancelDTO data)
        {
            var schedule = await _context.Schedules.FindAsync(data.Id);

            if (schedule == null)
                return false;

            schedule.Available += data.Number_Adult + data.Number_Child;
            schedule.Status = 1;

            _context.Schedules.Update(schedule);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
