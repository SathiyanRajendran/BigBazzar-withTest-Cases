using BigBazzar.Models;
using BigBazzar.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BigBazzar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<ActionResult<IList<Products>>> GetProducts()
        {

            return await _repository.GetAllProduct();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProductbyId(int id)
        {
            return await  _repository.GetProductbyId(id);
        }
        [HttpPost("AddProduct")]
        public async Task<ActionResult<Products>> PostProduct(Products product)
        {
            return await _repository.AddProduct(product);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Products>> PutProduct(int productId, Products product)
        {
            return await _repository.EditProduct(productId, product);
        }
        [HttpDelete("{id}")]
        public async Task DeleteProduct(int id)
        {
            await _repository.DeleteProduct(id);
        }    
    }
}
