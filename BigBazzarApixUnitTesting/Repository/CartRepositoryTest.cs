using BigBazzar.Models;
using BigBazzar.Repository;
using BigBazzarApixUnitTesting.DatabaseContext;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBazzarApixUnitTesting.Repository
{
    public class CartRepositoryTest
    {
        [Fact]
        public async Task CartRepository_GetAllCartsByCustomerId_ReturnGetCarts()
        {
            //Arrange
            var useinmemorytest = new DbContextTest();
            var dbContext = await useinmemorytest.GetDatabaseContext();
            var cartRepository = new CartRepo(dbContext);

            //Act
            var result = await cartRepository.GetAllCart(1000);
            //Assert
            //result.Count.Should().Be(4);
            4.Should().Be(result.Count);
            var tempdata = result.First();
            12.Should().Be(tempdata.ProductQuantity);

            //Act
            try
            {
                var result1 = await cartRepository.GetAllCart(100);
                //Assert
                result1.Should().BeNullOrEmpty();
            }
            catch(Exception e)
            {
                throw new ArgumentNullException("Id is not matched");
            }
        }
        [Fact]
        public async Task CartRepository_GetCartById_ReturnCarts()
        {
            //Arrange
            var useinmemorytest = new DbContextTest();
            var dbContext = await useinmemorytest.GetDatabaseContext();
            var cartRepository = new CartRepo(dbContext);
            //Act
            var result = await cartRepository.GetCartById(1002);
            //Assert
            var qty = 12;
            qty.Should().Be(result.ProductQuantity);
        }
        [Fact]
        public async Task CartRepository_AddCarts_ReturnAddCarts()
        {
            //Arrange
            //Here we increase the product quantity of already existing cart (12+12=24)
            var cart = new Carts()
            {
                ProductQuantity = 12,
                ProductId = 101,
                CustomerId = 1000,
            };
            var inmemorytest=new DbContextTest();
            var dbContext=await inmemorytest.GetDatabaseContext();
            var cartRepository=new CartRepo(dbContext);
            //Act
            var result = await cartRepository.AddToCart(cart);
            //Assert
           // dbContext.Carts.Should().HaveCount(5);
           result.Should().BeEquivalentTo(cart);
            4.Should().Be(dbContext.Carts.Count());

            //We add a new cart for an already existing customer here customerid and productid are not equal to the inmemory database

            var cart2 = new Carts()
            {
                ProductQuantity = 12,
                ProductId = 110,
                CustomerId = 1000,
            };
            var inmemorytest2 = new DbContextTest();
            var dbContext2 = await inmemorytest2.GetDatabaseContext();
            var cartRepository2 = new CartRepo(dbContext2);
            //Act
            var result2 = await cartRepository2.AddToCart(cart2);
            //Assert
            // dbContext.Carts.Should().HaveCount(5);
            result2.Should().BeEquivalentTo(cart2);
            5.Should().Be(dbContext2.Carts.Count());

        }
        [Fact]
        public async Task cartRepository_DeleteCart_ReturnDelete()
        {
            //Arrange
            var inmemorytest = new DbContextTest();
            var dbContext=await inmemorytest.GetDatabaseContext();
            var cartRepository=new CartRepo(dbContext);
            //Act
            var result = await cartRepository.DeleteFromCart(1002);
            //Assert
            result.Should().BeTrue();
            3.Should().Be(dbContext.Carts.Count());
        }
        [Fact]
        public async Task cartRepository_EditCart_ReturnEdit()
        {
            //Arrange
            var id = 1002;
            var cart = new Carts()
            {
                CartId = id,
                ProductQuantity = 15,
            };
            var inmemorytest=new DbContextTest();
            var dbContext = await inmemorytest.GetDatabaseContext();
            var cartRepository= new CartRepo(dbContext);
            //Act
            var cartfind=await dbContext.Carts.FindAsync(id);
            dbContext.Entry<Carts>(cartfind).State = EntityState.Detached;//has to be used only on xUnittesting

            var result = await cartRepository.EditCart(id,cart);
            //Assert
            result.Should().BeEquivalentTo(cart);
            4.Should().Be(dbContext.Carts.Count());
        }
    }
}
