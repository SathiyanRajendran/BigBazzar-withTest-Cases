using BigBazzar.Data;
using BigBazzar.Models;
using Microsoft.EntityFrameworkCore;

namespace BigBazzar.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly BigBazzarContext _context;
        public ProductRepository(BigBazzarContext context)
        {
            _context = context;
        }

        public async Task<Products> AddProduct(Products product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProduct(int productId)
        {
            try
            {
                Products P=_context.Products.Find(productId);
                _context.Products.Remove(P);
                await _context.SaveChangesAsync();  
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<Products> EditProduct(int ProductId, Products product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Products>> GetAllProduct()
        {
            try
            {
                return await _context.Products.Include(x=>x.Categories).Include(x=>x.Traders).ToListAsync();
            }
            catch
            {
                throw new NotImplementedException();

            }
        }
        public async Task<List<Products>> Search(string option,string search)
        {
           
            return await _context.Products.Where(x => x.ProductName.StartsWith(search) || search == null).ToListAsync();  
           
        }

        public async Task<Products> GetProductbyId(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }
    }
}
