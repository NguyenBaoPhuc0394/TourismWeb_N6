using Microsoft.EntityFrameworkCore;
using TourismWeb.Data;
using TourismWeb.DTOs.Booking;
using TourismWeb.DTOs.Schedule;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;

namespace TourismWeb.Repositories.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly TourismDbContext _context;

        public BookingRepository(TourismDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBooking(BookingCreateDTO booking)
        {
            var result = _context.Bookings
                .FromSqlInterpolated($@"
                    EXEC CreateBooking 
                        @CustomerId = {booking.Customer_Id}, 
                        @ScheduleId = {booking.Schedule_Id}, 
                        @NumberOfAdultBookings = {booking.Number_of_adult_bookings}, 
                        @NumberOfChildrenBookings = {booking.Number_of_children_bookings}, 
                        @TotalPrice = {booking.Total_price}
                ")
                .AsEnumerable()
                .FirstOrDefault();

            if (result == null)
            {
                throw new Exception("Booking not found!");
            }
            return result;
        }

        public Tuple<string, DateOnly>? GetTourNameByBookingId(string bookingId)
        {
            var result = _context.Tours
                .Join(_context.Schedules,
                      t => t.Id,
                      s => s.Tour_Id,
                      (t, s) => new { Tour = t, Schedule = s })
                .Join(_context.Bookings,
                      ts => ts.Schedule.Id,
                      b => b.Schedule_Id,
                      (ts, b) => new { ts.Tour, ts.Schedule, Booking = b })
                .Where(tb => tb.Booking.Id == bookingId)
                .Select(tb => new Tuple<string, DateOnly>(
                    tb.Tour.Name,
                    tb.Schedule.Start_date 

                ))
                .FirstOrDefault();

            return result;
        }

        public async Task<string?> getCustomerIdByBooking(string bookingId)
        {
            var result = await _context.Bookings
                .Where(b => b.Id == bookingId)
                .Select(b => b.Customer_Id)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<string?> getTouridByBooking(string bookingId)
        {
            var result = await _context.Tours
                .Join(_context.Schedules,
                      t => t.Id,
                      s => s.Tour_Id,
                      (t, s) => new { Tour = t, Schedule = s })
                .Join(_context.Bookings,
                      ts => ts.Schedule.Id,
                      b => b.Schedule_Id,
                      (ts, b) => new { ts.Tour, ts.Schedule, Booking = b })
                .Where(tb => tb.Booking.Id == bookingId)
                .Select(tb => tb.Tour.Id)
                .FirstOrDefaultAsync();

            return result;
        }


        public async Task<IEnumerable<BookingTableDataDTO>> GetAllBookingOfCustomer(string customerId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.Customer_Id == customerId)
                .Select(b => new BookingTableDataDTO
                {
                    Id = b.Id,
                    Booking_date = b.Booking_date,
                    Number_of_adult_bookings = b.Number_of_adult_bookings,
                    Number_of_children_bookings = b.Number_of_children_bookings,
                    Total_price = b.Total_price,
                    Status = b.Status
                })
                .ToListAsync();

            return bookings;
        }

        public async Task<bool> CancelBooking(string bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            if (booking == null)
            {
                return false; 
            }

            booking.Status = 0;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ScheduleUpdateBookingCancelDTO?> GetUpdateScheDataForCancel(string bookingId)
        {
            var result = await _context.Bookings
                .Where(b => b.Id == bookingId)
                .Select(b => new ScheduleUpdateBookingCancelDTO
                {
                    Id = b.Schedule_Id,
                    Number_Adult = b.Number_of_adult_bookings,
                    Number_Child = b.Number_of_children_bookings
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> UpdateBookingStatusReviewed(string bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                return false;
            }
            booking.Status = 3;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Booking>> GetAllBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        public async Task<bool> UpdateStatus(BookingUpdateDTO data)
        {
            var booking = await _context.Bookings.FindAsync(data.Id);

            if (booking == null)
                return false;

            booking.Status = data.Status;

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
