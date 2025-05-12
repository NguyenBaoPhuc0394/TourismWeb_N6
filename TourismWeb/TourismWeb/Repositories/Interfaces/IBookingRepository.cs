using Microsoft.AspNetCore.Hosting.Server;
using TourismWeb.DTOs.Booking;
using TourismWeb.DTOs.Schedule;
using TourismWeb.Models;

namespace TourismWeb.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBooking(BookingCreateDTO booking);
        Task<IEnumerable<BookingTableDataDTO>> GetAllBookingOfCustomer(string customerId);
        Tuple<string, DateOnly>? GetTourNameByBookingId(string bookingId);

        Task<bool> CancelBooking(string bookingId);
        Task<ScheduleUpdateBookingCancelDTO?> GetUpdateScheDataForCancel(string bookingId);

        Task<string?> getCustomerIdByBooking(string bookingId);
        Task<string?> getTouridByBooking(string bookingId);

        Task<bool> UpdateBookingStatusReviewed(string bookingId);

    }
}
