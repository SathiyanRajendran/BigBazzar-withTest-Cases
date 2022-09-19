using BigBazzar.Data;
using BigBazzar.Models;
using Microsoft.EntityFrameworkCore;

namespace BigBazzar.Repository
{
    public class AdminRepo : IAdminRepo
    {
        private readonly BigBazzarContext _context;
        public AdminRepo(BigBazzarContext context)
        {
            _context = context;
        }

        public async Task<Admin> AddNewAdmin(Admin A)
        {
            _context.Admins.Add(A);
            await _context.SaveChangesAsync();
            return A;
        }

        public async Task<Categories> AddNewCategory(Categories category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Admin> AdminLogin(Admin A)
        {
            var c = (from i in _context.Admins where i.AdminEmail == A.AdminEmail && i.AdminPassword == A.AdminPassword select i).FirstOrDefault();
            return c;
        }

        public async Task DeleteAdmin(int AdminId)
        {
            try
            {
                Admin admin = _context.Admins.Find(AdminId);
                _context.Admins.Remove(admin);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task DeleteCategory(int CategoryId)
        {
            try
            {
                Categories C=_context.Categories.Find(CategoryId);
                _context.Categories.Remove(C);
                await _context.SaveChangesAsync();  
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<Admin> EditAdmin(int AdminId)
        {
            Admin A= await _context.Admins.FindAsync(AdminId);
            _context.Update(A);
            _context.SaveChanges();
            return A;
        }

        public async Task<List<Categories>> GetAllCategories()
        {
            try
            {
                return await _context.Categories.ToListAsync();
            }
            catch
            {
                throw new NotImplementedException();

            }
        }
    }
}
