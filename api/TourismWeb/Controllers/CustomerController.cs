using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourismWeb.DTOs.Customer;
using TourismWeb.Models;
using TourismWeb.Services.Interfaces;
using NLog;

namespace TourismWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
            Logger.Debug("CustomerController initialized.");
        }

        // POST: api/Customer/create
        [HttpPost("create")]
        public async Task<ActionResult<Customer>> CreateCustomer(CustomerCreateDTO createDTO)
        {
            Logger.Info($"Customer creation attempt for email: {createDTO.Email}");
            try
            {
                var result = await _customerService.CreateCustomer(createDTO);
                if (result == null)
                {
                    Logger.Warn("Customer creation failed: Result is null.");
                    return BadRequest("Thêm khách hàng thất bại");
                }

                Logger.Info($"Customer created successfully: {result.Id}");
                return CreatedAtAction(
                    nameof(GetCustomerById),
                    new { id = result.Id },
                    result
                );
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error creating customer for email: {createDTO.Email}");
                return StatusCode(500, new { message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Customer/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(string id)
        {
            Logger.Info($"Fetching customer with ID: {id}");
            try
            {
                var customer = await _customerService.GetCustomerById(id);
                if (customer == null)
                {
                    Logger.Warn($"Customer with ID {id} not found.");
                    return NotFound();
                }

                Logger.Debug($"Customer retrieved: {id}");
                return Ok(customer);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error fetching customer with ID: {id}");
                return StatusCode(500, new { message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // PUT: api/Customer/update
        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<CustomerUpdateDTO>> UpdateCustomer(CustomerUpdateDTO updateDTO)
        {
            Logger.Info($"Updating customer with Email: {updateDTO.Email}");
            try
            {
                var customer = await _customerService.UpdateCustomer(updateDTO);
                if (customer == null)
                {
                    Logger.Warn($"Customer with Email {updateDTO.Email} not found or update failed.");
                    return NotFound(new { message = "Không tìm thấy khách hàng hoặc cập nhật thất bại." });
                }

                Logger.Info($"Customer updated successfully: {updateDTO.Email}");
                return Ok(customer);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error updating customer with ID: {updateDTO.Email}");
                return StatusCode(500, new { message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Customer/email
        [HttpGet("email")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerByEmail([FromQuery] string email)
        {
            Logger.Info($"Fetching customer with email: {email}");
            try
            {
                var customer = await _customerService.GetCustomerByEmail(email);
                if (customer == null)
                {
                    Logger.Warn($"Customer with email {email} not found.");
                    return NotFound(new { message = "Không tìm thấy khách hàng với email này." });
                }

                Logger.Debug($"Customer retrieved for email: {email}");
                return Ok(customer);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error fetching customer with email: {email}");
                return StatusCode(500, new { message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Customer/all-customers
        [HttpGet("all-customers")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAllCustomers()
        {
            Logger.Info("Fetching all customers.");
            try
            {
                var result = await _customerService.GetAllCustomers();
                Logger.Debug($"Retrieved {result.Count()} customers.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error fetching all customers.");
                return StatusCode(500, new { message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }
    }
}