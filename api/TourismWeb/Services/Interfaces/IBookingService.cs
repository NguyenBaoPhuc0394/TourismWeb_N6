using TourismWeb.DTOs.Booking;

namespace TourismWeb.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingDTO> CreateBooking(BookingCreateDTO createDTO);

        Task<IEnumerable<BookingTableDataDTO>> GetAllBookingOfCustomer(string customerId);

        Task<bool> CancelBooking(string bookingId);
        Task<string?> GetCusIdByBooking(string bookingId);
        Task<string?> GetTourIdByBooking(string bookingId);
        Task<bool> UpadateBookingReviewed(string bookingId);

        Task<IEnumerable<BookingDataTableDTO>> GetAllBookingDataTable();

        Task<bool> UpdateStatus(BookingUpdateDTO data);

    }
}
