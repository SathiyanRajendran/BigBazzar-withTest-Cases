using BigBazzar.Data;
using BigBazzar.Models;
using BigBazzar.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBazzarApixUnitTesting.Repository
{
    public class ProductRepositoryTest
    {
        private async Task<BigBazzarContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<BigBazzarContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;
            var databaseContext = new BigBazzarContext(options);
            databaseContext.Database.EnsureCreated();
            int idno = 10;
            if (await databaseContext.Products.CountAsync() <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    var product = new Products()
                    {
                        ProductId = idno++,
                        ProductName = "Boost" + i,
                        UnitPrice = 150 + i,
                        ProductQuantity = 12,
                    };
                    databaseContext.Products.Add(product);
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }
        [Fact]
        public async Task ProductRepository_GetAllProducts_ReturnGetProduct()
        {
            //Arrange
            var dbContext=await GetDatabaseContext();
            var productRepository = new ProductRepository(dbContext);

            //Act
            var result = await productRepository.GetAllProduct();
            //Assert
            var count=result.Count();
            result.Should().HaveCount(count);
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task ProductRepository_GetProductById_ReturnProductbyId()
        {
            //Arrange
            //var id = 10;
            var dbContext=await GetDatabaseContext();
            var productRepository = new ProductRepository(dbContext);
            //Act
            var result = await productRepository.GetProductbyId(10);
            //Assert
            var name = "Boost0";
            name.Should().Be(result.ProductName);
        }
        [Fact]
        public async Task ProductRepository_AddProduct_ReturnPostProduct()
        {
            //Arrange
            var product = new Products()
            {
                ProductName = "Complan",
                ProductQuantity = 10,
                UnitPrice = 180,
            };
            var dbContext=await GetDatabaseContext();
            var productRepository=new ProductRepository(dbContext);
            //Act
            var result = await productRepository.AddProduct(product);
            //Assert
           dbContext.Products.Should().HaveCount(6);
            result.Should().BeEquivalentTo(product);
           

        }
        [Fact]
        public async Task ProductRepository_EditProduct_ReturnEdit()
        {
            var id = 10;
            var product = new Products()
            {
                ProductId = id,
                ProductName = "Viva",
                UnitPrice = 156,
                ProductQuantity = 10,
            };
            var dbContext= await GetDatabaseContext();
            var productRepository= new ProductRepository(dbContext);

            //Act
            var productfind = await dbContext.Products.FindAsync(id);
            dbContext.Entry<Products>(productfind).State= EntityState.Detached;
            var result = await productRepository.EditProduct(id,product);
            //Assert
            result.Should().BeEquivalentTo(product);
            dbContext.Products.Should().HaveCount(5);
        }
        [Fact]
        public async Task ProductRepository_DeleteProduct_ReturnDelete()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var productRepository = new ProductRepository(dbContext);
            //Act
            var result = await productRepository.DeleteProduct(10);
            result.Should().BeTrue();
            dbContext.Products.Should().HaveCount(4);
        }
    }
}
