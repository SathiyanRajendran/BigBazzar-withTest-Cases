using BigBazzar.Models;

namespace BigBazzar.Repository
{
    public interface IProductRepository
    {
        Task<Products> AddProduct(Products product);
        Task<Products> EditProduct(int ProductId, Products product);
        Task DeleteProduct(int productId);
        Task<List<Products>> GetAllProduct();
        Task<Products> GetProductbyId(int productId);
    }
}
