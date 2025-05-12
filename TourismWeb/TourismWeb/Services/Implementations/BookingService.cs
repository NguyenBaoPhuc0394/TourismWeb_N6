using AutoMapper;
using TourismWeb.DTOs.Booking;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly IScheduleRepository _scheduleRepository;
        public BookingService(IBookingRepository bookingRepository, IMapper mapper, IScheduleRepository scheduleRepository)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _scheduleRepository = scheduleRepository;
        }

        public async Task<BookingDTO> CreateBooking(BookingCreateDTO createDTO)
        {
            var createdBooking = await _bookingRepository.CreateBooking(createDTO);
            return _mapper.Map<BookingDTO>(createdBooking);
        }

        public async Task<IEnumerable<BookingTableDataDTO>> GetAllBookingOfCustomer(string customerId)
        {
            var bookings = await _bookingRepository.GetAllBookingOfCustomer(customerId);

            foreach (var b in bookings)
            {
                var nameAndSD = _bookingRepository.GetTourNameByBookingId(b.Id);

                if (nameAndSD != null)
                {
                    b.StartDate = nameAndSD.Item2;
                    b.Tour_name = nameAndSD.Item1;
                }
            }

            return bookings;
        }


        public async Task<bool> CancelBooking(string bookingId)
        {
            try
            {
                var result = await _bookingRepository.CancelBooking(bookingId);
                if (result == false) return false;
                var scheUpdateData = await _bookingRepository.GetUpdateScheDataForCancel(bookingId);
                if (scheUpdateData == null) throw new KeyNotFoundException("Không tìm thấy dữ liệu để update schedule");
                return await _scheduleRepository.UpdateScheduleForBookingCancel(scheUpdateData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string?> GetCusIdByBooking(string bookingId)
        {
            return await _bookingRepository.getCustomerIdByBooking(bookingId);
        }

        public async Task<string?> GetTourIdByBooking(string bookingId)
        {
            return await _bookingRepository.getTouridByBooking(bookingId);
        }

        public async Task<bool> UpadateBookingReviewed(string bookingId)
        {
            return await _bookingRepository.UpdateBookingStatusReviewed(bookingId);
        }

    }
}
