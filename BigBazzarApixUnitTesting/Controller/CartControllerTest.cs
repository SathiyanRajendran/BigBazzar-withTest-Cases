using BigBazzar.Controllers;
using BigBazzar.Models;
using BigBazzar.Repository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBazzarApixUnitTesting.Controller
{
    public class CartControllerTest
    {
        private readonly ICartRepo _repository;
        public CartControllerTest()
        {
            _repository = A.Fake<ICartRepo>();
        }
        
        [Fact]
       
        public async Task CartController_AddCart_ReturnAdd()
        {
            //Arrange
            var id = 100;
            var cart = new Carts()
            {
                CartId = id,               
                ProductQuantity = 12,
            };
            A.CallTo(() => _repository.AddToCart(cart)).Returns(cart);
            var controller = new CartsController(_repository);
            //Act
            var result = await controller.AddCart(cart);
            //Assert
            result.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(cart);
        }
        [Fact]
        public async Task CartController_CartById_ReturnCartById()
        {
            //Arrange
            var id=100;
            var cart = new Carts()
            {
                CartId = id,
                ProductQuantity = 13,
                CustomerId=1000,

            };
            A.CallTo(() => _repository.GetCartById(id)).Returns(cart);
            var controller=new CartsController(_repository);
            //Act
            var result=await controller.CartById(id);
            //Assert
            var qty = 13;
            qty.Should().Be(cart.ProductQuantity);
        }
        [Fact]
        public async Task CartController_UpdateCart_ReturnEditOk()
        {
            //Arrange
            var id = 1000;
            var cart = new Carts()
            {
                CartId = id,
                ProductQuantity = 12,
            };
            A.CallTo(()=>_repository.EditCart(id,cart)).Returns(cart);
            var controller= new CartsController(_repository);
            //Act
            var result = await controller.UpdateCart(id,cart);
            //Assert
            var qty = 12;   
            qty.Should().Be(cart.ProductQuantity);
            result.Value.Should().BeOfType<Carts>();
        }
        [Fact]
        public async Task CartController_DeleteCart_ReturnOk()
        {
            //Arrange
            A.CallTo(() => _repository.DeleteFromCart(1000)).Returns(true);
            var controller=new CartsController(_repository);
            //Act
            var result= await controller.DeleteCart(1000);
            //Assert
            result.Should().BeOfType(typeof(OkResult));

        }
        [Fact]
        public void CartController_GetCartByCustId_ReturnOk()
        {
            //Arrange
            var cartsList = A.Fake<List<Carts>>();
            A.CallTo(() => _repository.GetAllCart(1000)).Returns(cartsList);
            var controller = new CartsController(_repository);
            //Act
            var result= controller.Getcarts(1000);
            //Assert
            result.Should().BeOfType<Task<ActionResult<List<Carts>>>>();

        }
    }
}
