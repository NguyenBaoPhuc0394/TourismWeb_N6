using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourismWeb.DTOs.Customer;
using TourismWeb.Models;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Customer>> CreateCustomer(CustomerCreateDTO createDTO)
        {
            var result = await _customerService.CreateCustomer(createDTO);
            if (result == null)
                return BadRequest("Thêm khách hàng thất bại");
            return CreatedAtAction(
                nameof(GetCustomerById), 
                new { id = result.Id },    
                result                     
            );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(string id) {
            var customer = await _customerService.GetCustomerById(id);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }


        [HttpPut("update")]
        public async Task<ActionResult<CustomerUpdateDTO>> UpdateCustomer(CustomerUpdateDTO updateDTO)
        {
            var customer = await _customerService.UpdateCustomer(updateDTO);
            return Ok(customer);
        }

        [HttpGet("email")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerByEmail([FromQuery] string email)
        {
            var customer = await _customerService.GetCustomerByEmail(email);
            return Ok(customer);
        }
    }
}
