using Microsoft.Data.SqlClient;
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

        public async Task<Schedule> CreateSchedule(ScheduleCreateDTO schedule)
        {
            var result = _context.Schedules.FromSqlRaw("EXEC CreateSchedule " +
                "@TourId = {0}, @StartDate = {1}, @Discount = {2}",
                schedule.Tour_Id, schedule.Start_date, schedule.Discount)
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

        public async Task<IEnumerable<ScheduleDataTableDTO>> GetScheduleWithTourNameAsync()
        {
            var result = await _context.Schedules
                .Include(s => s.Tour)
                .Select(s => new ScheduleDataTableDTO
                {
                    Id = s.Id,
                    TourName = s.Tour.Name,
                    StartDate = s.Start_date,
                    Available = s.Available,
                    AdultPrice = s.Adult_price,
                    ChildrenPrice = s.Children_price,
                    Discount = s.Discount
                })
                .ToListAsync();

            return result;
        }

        public async Task UpdateScheduleDiscountAsync(string scheduleId, double discount)
        {
            var scheduleIdParam = new SqlParameter("@ScheduleId", scheduleId);
            var discountParam = new SqlParameter("@Discount", discount);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC UpdateScheduleDiscount @ScheduleId, @Discount",
                scheduleIdParam, discountParam
            );
        }
    }
}
