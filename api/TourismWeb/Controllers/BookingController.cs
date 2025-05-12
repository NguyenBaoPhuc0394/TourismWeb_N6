using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TourismWeb.DTOs.Booking;
using TourismWeb.Services.Implementations;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IAccountService _accountService;

        public BookingController(IBookingService bookingService, IAccountService accountService)
        {
            _bookingService = bookingService;
            _accountService = accountService;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult> CreateBooking([FromBody] BookingCreateDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }

                var usernameFromToken = User.FindFirst(ClaimTypes.Name)?.Value;
                string cusID = _accountService.GetCusByUsername(usernameFromToken);
                // Kiểm tra username từ token với 'Customer_Id' trong createDTO
                if (!string.IsNullOrEmpty(usernameFromToken) && cusID != createDTO.Customer_Id)
                {
                    return BadRequest(new { message = "Username không khớp với người dùng đã xác thực"});
                }

                // Tiến hành tạo booking
                var result = await _bookingService.CreateBooking(createDTO);
                return Ok(new { message = "Đặt tour thành công", data = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra", detail = ex.Message });
            }
        }

        [HttpGet("customer")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BookingTableDataDTO>>> GetAllBookingOfCustomer()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return NotFound(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }

                var usernameFromToken = User.FindFirst(ClaimTypes.Name)?.Value;
                string cusID = _accountService.GetCusByUsername(usernameFromToken);

                var bookings = await _bookingService.GetAllBookingOfCustomer(cusID);
                if (bookings == null) throw new KeyNotFoundException("Không tìm thấy dữ liệu booking");

                return Ok(bookings);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra", detail = ex.Message });
            }
        }

        [HttpPut("cancel")]
        [Authorize]
        public async Task<IActionResult> CancelBooking([FromBody] string bookId)
        {
            try
            {
                if (string.IsNullOrEmpty(bookId))
                    return BadRequest(new { message = "Mã booking không hợp lệ" });

                var success = await _bookingService.CancelBooking(bookId);

                if (!success)
                    return NotFound(new { message = "Không tìm thấy booking hoặc huỷ thất bại" });

                return Ok(new { message = "Huỷ booking thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra", detail = ex.Message });
            }
        }

        [HttpGet("all-bookings")]
        public async Task<ActionResult<IEnumerable<BookingDataTableDTO>>> GetAllBookingDataForTable()
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingDataTable();
                return Ok(bookings);
            } catch (Exception ex) {
                return BadRequest(new { message = "Có lỗi xảy ra", detail = ex.Message });
            }
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateBookingStatus(BookingUpdateDTO data)
        {
            if (data == null || string.IsNullOrEmpty(data.Id))
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var result = await _bookingService.UpdateStatus(data);

            if (result)
            {
                return Ok("Cập nhật trạng thái booking thành công.");
            }
            else
            {
                return NotFound("Booking không tồn tại.");
            }
        }

    }

}
