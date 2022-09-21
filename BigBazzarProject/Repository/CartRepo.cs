using BigBazzar.Data;
using BigBazzar.Models;
using Microsoft.EntityFrameworkCore;

namespace BigBazzar.Repository
{
    public class CartRepo : ICartRepo
    {
        private readonly BigBazzarContext _context;
        public CartRepo(BigBazzarContext context)
        {
            _context = context;
        }

        public async Task<Carts>? AddToCart(Carts cart)
                                   //When the customer wants to add a product to their cart it will update on a cart when he/she already have
                                   //or else it will go and create a new cart.
        {
            if(await isCartExists(cart))
            {
                return cart;
            }
            else
            {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
            }
        }

        public async Task<bool> DeleteFromCart(int id)
            //here we delete the cart by the cartid.
        {
            try
            {
                Carts carts = _context.Carts.Find(id);
                _context.Carts.Remove(carts);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<Carts> EditCart(int id,Carts C)
            //here we edit the cart by the cartid.
        {
            _context.Update(C);
            _context.SaveChanges();
            return C;
        }

        public async Task<List<Carts>> GetAllCart(int customerId) //get all products of the cart(CartId) by the customerid
                                                          // Here include denotes the "Instead of productid here I use the productname,quantity"
                                                          //customer can only see the carts which he have.
        {
            List<Carts> cartList =await (from c in _context.Carts.Include(x => x.Products) where c.CustomerId == customerId select c).ToListAsync();
            return cartList;
        }
        public async Task<Carts> GetCartById(int cartid)
            //here we get the carts by the cartid.
        {
            var result = await (from c in _context.Carts.Include(x => x.Products) 
                                where c.CartId == cartid 
                                select c).SingleAsync();
            return result;
        }
        private async Task<bool> isCartExists(Carts ct)  //THIS IS INTERCONNECTED WITH THE ADD TO CART
        {
            var cart = (from c in _context.Carts
                        where c.ProductId == ct.ProductId && c.CustomerId == ct.CustomerId 
                        select c).FirstOrDefault();
            
            if(cart==null)  //Here we add the products to the new cart

            {
                return false;
            }
            else   //Here we update the products quantity.
            {
                cart.ProductQuantity += ct.ProductQuantity;
                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
