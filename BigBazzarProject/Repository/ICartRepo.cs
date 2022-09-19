using BigBazzar.Models;

namespace BigBazzar.Repository
{
    public interface ICartRepo
    {
        Task<Carts> AddToCart(Carts cart);
        Task DeleteFromCart(int id);
        Task<Carts> GetCartById(int id);
        Task<List<Carts>> GetAllCart(int customerId);
        Task<Carts> EditCart(int id,Carts C);

    }
}
