using BigBazzar.Data;
using BigBazzar.Models;
using Microsoft.EntityFrameworkCore;

namespace BigBazzar.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly BigBazzarContext _context;
        public CustomerRepo(BigBazzarContext context)
        {
            _context = context;
        }

        public async Task<Customers> AddCustomer(Customers customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Feedback> AddFeedback(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<Customers> CustomerLogin(Customers customer)
        {
            var C = (from i in _context.Customers where i.CustomerEmail == customer.CustomerEmail && i.Password == customer.Password select i).FirstOrDefault();
            return C;
        } 

        public async Task<List<Customers>> GetAllCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customers> GetCustomerById(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customers> UpdateCustomer(int id, Customers customer)
        {
          

            _context.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}
