using BigBazzar.Models;
using BigBazzar.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BigBazzar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepo _repository;
        public CartsController(ICartRepo repository)
        {
            _repository = repository;
        }
        [HttpGet("{id}")]
        //get all carts of a particular customer by using the customer id (/api/carts/{id})
        public async Task<ActionResult<List<Carts>>> Getcarts(int id)
        {

            List<Carts> result = await _repository.GetAllCart(id);
            return result;
        }
        [HttpGet("id")]
        public async Task<ActionResult<Carts>> CartById(int id)
        {
            return await _repository.GetCartById(id);
        }
        [HttpPost]
        public async Task<ActionResult<Carts>> AddCart(Carts cart)//(api/carts)
        {
            return await _repository.AddToCart(cart);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Carts>> UpdateCart(int id, Carts C) //(API/CARTS/{ID})
        {
            return await _repository.EditCart(id, C);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCart(int id) //(API/CARTS/{ID})
        {
            
            bool ans=await _repository.DeleteFromCart(id);
            if (ans)
                return Ok();
            else
                return BadRequest();
        }

    }
}
