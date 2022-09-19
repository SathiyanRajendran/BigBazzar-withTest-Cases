using BigBazzar.Models;
using BigBazzar.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BigBazzar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepo _repository;
        public AdminController(IAdminRepo repository)
        {
            _repository = repository;
        }
        [HttpPost("AddAdmin")]
        public async Task<ActionResult<Admin>> PostAdmin(Admin A)
        {
            return await _repository.AddNewAdmin(A);
        }
        [HttpDelete("DeleteAdmin")]
        public async Task AdminDelete(int AdminId)
        {
            await _repository.DeleteAdmin(AdminId);
        }
        [HttpGet]
        public async Task<List<Categories>> GetAllCategories()
        {
            return await _repository.GetAllCategories();
        }

        [HttpPost("AddCategory")]
        public async Task<ActionResult<Categories>> PostCategory(Categories C)
        {
            return await _repository.AddNewCategory(C);
        }
        
        [HttpDelete("DeleteCategory")]
        public async Task DeleteCategory(int CategoryId)
        {
            await _repository.DeleteCategory(CategoryId);   
        }
        [HttpPost("AdminLogin")]
        public async Task<ActionResult<Admin>> Login(Admin admin)
        {
            var loginadmin = await _repository.AdminLogin(admin);
            if(loginadmin==null)
            {
                return BadRequest("Invalid Credentials");
            }
            return loginadmin;
        }
    }
}

