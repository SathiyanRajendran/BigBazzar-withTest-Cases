using BigBazzar.Helper;
using BigBazzar.Models;
using BigBazzar.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BigBazzar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepo _repository;
        public CustomersController(ICustomerRepo repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetCustomers()
        {

            return await _repository.GetAllCustomers();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Customers>> GetCustomer(int id)
        {
            return await _repository.GetCustomerById(id);
        }
        [HttpPost]
        public async Task<ActionResult<Customers>> PostCustomer(Customers customer)
        {
            Customers c = customer;
            var PasswordHash = EncodePassword.GetMd5Hash(c.Password);
            customer.Password = PasswordHash;
            customer.ConfirmPassword = PasswordHash;
            return await _repository.AddCustomer(customer);
        }
        [HttpPost("Feedback")]
        public async Task<ActionResult<Feedback>> PostFeedback(Feedback feedback)
        {
            return await _repository.AddFeedback(feedback);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<Customers>> CustomerLogin(Customers customer)
        {
            Customers c = customer;
            var PasswordHash = EncodePassword.GetMd5Hash(c.Password);
            customer.Password = PasswordHash;
            customer.ConfirmPassword = PasswordHash;
           
            var logincustomer= await _repository.CustomerLogin(customer);
            if(logincustomer==null)
            {
                return BadRequest("Invalid Credentials");
            }
            return logincustomer;
          

        }
        [HttpPut("id")]
        public async Task<ActionResult<Customers>> PutCustomer(int id,Customers customer)
        {
            Customers c = customer;
            var PasswordHash = EncodePassword.GetMd5Hash(c.Password);
            customer.Password = PasswordHash;
            customer.ConfirmPassword = PasswordHash;
            return await _repository.UpdateCustomer(id, customer);
        }

    }
}
