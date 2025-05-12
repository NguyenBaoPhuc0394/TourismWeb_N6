using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TourismWeb.DTOs.Booking;
using TourismWeb.Services.Interfaces;
using NLog; 

namespace TourismWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IAccountService _accountService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public BookingController(IBookingService bookingService, IAccountService accountService)
        {
            _bookingService = bookingService;
            _accountService = accountService;
            Logger.Debug("BookingController initialized.");
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult> CreateBooking([FromBody] BookingCreateDTO createDTO)
        {
            Logger.Info($"Booking creation attempt for customer ID: {createDTO.Customer_Id}");
            try
            {
                if (!ModelState.IsValid)
                {
                    Logger.Warn("Invalid model state for booking creation.");
                    return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }

                var usernameFromToken = User.FindFirst(ClaimTypes.Name)?.Value;
                string cusID = _accountService.GetCusByUsername(usernameFromToken);
                Logger.Debug($"Username from token: {usernameFromToken}, Customer ID: {cusID}");

                // Kiểm tra username từ token với 'Customer_Id' trong createDTO
                if (!string.IsNullOrEmpty(usernameFromToken) && cusID != createDTO.Customer_Id)
                {
                    Logger.Warn($"Booking creation failed: Username {usernameFromToken} does not match customer ID {createDTO.Customer_Id}.");
                    return BadRequest(new { message = "Username không khớp với người dùng đã xác thực" });
                }

                // Tiến hành tạo booking
                var result = await _bookingService.CreateBooking(createDTO);
                Logger.Info($"Booking created successfully for customer ID: {createDTO.Customer_Id}");
                return Ok(new { message = "Đặt tour thành công", data = result });
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Booking creation failed: {ex.Message}");
                return NotFound(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                Logger.Warn($"Booking creation failed: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error creating booking for customer ID: {createDTO.Customer_Id}");
                return StatusCode(500, new { message = "Có lỗi xảy ra", detail = ex.Message });
            }
        }

        [HttpGet("customer")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BookingTableDataDTO>>> GetAllBookingOfCustomer()
        {
            Logger.Info("Fetching all bookings for customer.");
            try
            {
                if (!ModelState.IsValid)
                {
                    Logger.Warn("Invalid model state for fetching customer bookings.");
                    return NotFound(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }

                var usernameFromToken = User.FindFirst(ClaimTypes.Name)?.Value;
                string cusID = _accountService.GetCusByUsername(usernameFromToken);
                Logger.Debug($"Fetching bookings for customer ID: {cusID} (username: {usernameFromToken})");

                var bookings = await _bookingService.GetAllBookingOfCustomer(cusID);
                if (bookings == null)
                {
                    Logger.Warn($"No bookings found for customer ID: {cusID}");
                    throw new KeyNotFoundException("Không tìm thấy dữ liệu booking");
                }

                Logger.Info($"Retrieved {bookings.Count()} bookings for customer ID: {cusID}");
                return Ok(bookings);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Fetching bookings failed: {ex.Message}");
                return NotFound(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                Logger.Warn($"Fetching bookings failed: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error fetching bookings for customer.");
                return StatusCode(500, new { message = "Có lỗi xảy ra", detail = ex.Message });
            }
        }

        [HttpPut("cancel")]
        [Authorize]
        public async Task<IActionResult> CancelBooking([FromBody] string bookId)
        {
            Logger.Info($"Cancel booking attempt for booking ID: {bookId}");
            try
            {
                if (string.IsNullOrEmpty(bookId))
                {
                    Logger.Warn("Cancel booking failed: Invalid booking ID.");
                    return BadRequest(new { message = "Mã booking không hợp lệ" });
                }

                var success = await _bookingService.CancelBooking(bookId);
                if (!success)
                {
                    Logger.Warn($"Cancel booking failed for booking ID: {bookId}. Booking not found or cancellation failed.");
                    return NotFound(new { message = "Không tìm thấy booking hoặc huỷ thất bại" });
                }

                Logger.Info($"Booking ID: {bookId} cancelled successfully.");
                return Ok(new { message = "Huỷ booking thành công" });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error cancelling booking ID: {bookId}");
                return StatusCode(500, new { message = "Có lỗi xảy ra", detail = ex.Message });
            }
        }

        [HttpGet("all-bookings")]
        public async Task<ActionResult<IEnumerable<BookingDataTableDTO>>> GetAllBookingDataForTable()
        {
            Logger.Info("Fetching all booking data for table.");
            try
            {
                var bookings = await _bookingService.GetAllBookingDataTable();
                Logger.Debug($"Retrieved {bookings.Count()} bookings for table.");
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error fetching all booking data for table.");
                return BadRequest(new { message = "Có lỗi xảy ra", detail = ex.Message });
            }
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateBookingStatus(BookingUpdateDTO data)
        {
            Logger.Info($"Updating status for booking ID: {data?.Id}");
            try
            {
                if (data == null || string.IsNullOrEmpty(data.Id))
                {
                    Logger.Warn("Update booking status failed: Invalid data or booking ID.");
                    return BadRequest("Dữ liệu không hợp lệ.");
                }

                var result = await _bookingService.UpdateStatus(data);
                if (result)
                {
                    Logger.Info($"Booking status updated successfully for booking ID: {data.Id}");
                    return Ok("Cập nhật trạng thái booking thành công.");
                }
                else
                {
                    Logger.Warn($"Update booking status failed: Booking ID {data.Id} not found.");
                    return NotFound("Booking không tồn tại.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error updating status for booking ID: {data?.Id}");
                return StatusCode(500, new { message = "Có lỗi xảy ra", detail = ex.Message });
            }
        }
    }
}