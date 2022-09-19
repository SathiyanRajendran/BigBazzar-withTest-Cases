using BigBazzar.Models;

namespace BigBazzar.Repository
{
    public interface ICustomerRepo
    {
        Task<List<Customers>> GetAllCustomers();

        Task<Customers> GetCustomerById(int id);
        Task<Customers> AddCustomer(Customers customer);

        Task<Customers> UpdateCustomer(int id, Customers customer);

        Task<Feedback> AddFeedback(Feedback feedback);
        Task<Customers> CustomerLogin(Customers customer);
    }
}
