using BigBazzar.Models;

namespace BigBazzar.Repository
{
    public interface IAdminRepo
    {
        Task<Admin> AddNewAdmin(Admin A);
        Task<bool> DeleteAdmin(int AdminId);
        Task<List<Categories>> GetAllCategories();
        Task<Categories> AddNewCategory(Categories category);
        Task<bool> DeleteCategory(int CategoryId);
        Task<Admin> EditAdmin(int AdminId,Admin A);
        Task<Admin> AdminLogin(Admin A);
    }
}
