using BigBazzar.Models;

namespace BigBazzar.Repository
{
    public interface ITraderRepo
    {
        Task<List<Traders>> GetAllTraders();
        Task<Traders> GetTraderbyId(int TraderId);
        Task<List<Products>> GetProductByTraderId(int id);
        Task<Traders> AddNewTraders(Traders T);
        Task<Traders> UpdateTraders(int TraderId, Traders T);
        Task<TraderToken> TraderLogin(Traders T);
        Task DeleteTraders(int TraderId);
    }
}
