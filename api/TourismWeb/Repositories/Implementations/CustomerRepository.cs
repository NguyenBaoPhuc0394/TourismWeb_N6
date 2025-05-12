using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TourismWeb.Data;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;

namespace TourismWeb.Repositories.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly TourismDbContext _context;
        
        public CustomerRepository(TourismDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            var result = _context.Customers
                .FromSqlRaw("EXEC CreateCustomer @Email = {0}",
                    customer.Email)
                .AsEnumerable()
                .FirstOrDefault();
            return result;
        }

        public async Task<Customer> GetCustomerById(string id)
        {
            var result = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(cus => cus.Email == customer.Email);
            if(existingCustomer == null)
            {
                throw new Exception("Customer not found!");
            }
            existingCustomer.Name = customer.Name;
            existingCustomer.DateOfBirth = customer.DateOfBirth;
            existingCustomer.Gender = customer.Gender;
            //existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;

            await _context.SaveChangesAsync();
            return existingCustomer;
        }

        public async Task<Customer> GetCustomerByEmail(string email)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(cus => cus.Email == email);
            return customer;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _context.Customers.ToListAsync();
        }
    }
}
