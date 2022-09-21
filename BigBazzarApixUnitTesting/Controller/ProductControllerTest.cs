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
    public class ProductControllerTest
    {
        private readonly IProductRepository _repository;
        public ProductControllerTest()
        {
            _repository = A.Fake<IProductRepository>();
        }
        [Fact]
        public async Task ProductsController_GetProducts_ReturnOk()
        {
            //Arrange
            var productslist = A.Fake<List<Products>>();
            A.CallTo(() => _repository.GetAllProduct()).Returns(productslist);
            var controller = new ProductsController(_repository);

            //Act
            var result = await controller.GetProducts();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<List<Products>>>();

        }
        [Fact]
        public async Task ProductsController_GetProductById_ReturnOk()
        {
            //Arrange
            var id = 1000;
           
           
                var product = new Products()
                {
                    ProductId = id,
                    ProductName = "Boost" ,
                    ProductQuantity = 12 ,
                    UnitPrice = 120 ,
                };
            
            A.CallTo(() => _repository.GetProductbyId(1000)).Returns(product);
            var controller = new ProductsController(_repository);
            //Act
            var result=await controller.GetProductbyId(product.ProductId);
            //Assert
            var name = "Boost";
            name.Should().BeSameAs(product.ProductName);
        }
        [Fact]
        public async Task ProductsController_PostProducts_ReturnOk()
        {
            //Arrange
            var id = 1000;
            var product = new Products()
            {
                ProductId = id,
                ProductName = "Complan",
                ProductQuantity = 12,
                UnitPrice = 124,
            };
            A.CallTo(() => _repository.AddProduct(product)).Returns(product);
            var controller=new ProductsController(_repository);
            //Act
            var result = await controller.PostProduct(product);
            //Assert
            result.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(product);
            
        }
        [Fact]
        public async Task ProductsController_PutProducts_ReturnOk()
        {
            //Arrange
            var id=1000;
            var product = new Products()
            {
                ProductId = id,
                ProductName = "Bournvita",
                ProductQuantity = 12,
                UnitPrice = 230,
            };
            A.CallTo(()=>_repository.EditProduct(id,product)).Returns(product);
            var controller= new ProductsController(_repository);
            //Act
            var result = await controller.PutProduct(id,product);
            //Assert
            var qty = 12;
            qty.Should().BeGreaterThan(10);
            result.Value.Should().BeEquivalentTo(product);              
        }
        [Fact]
        public async Task ProductsController_DeleteProduct_ReturnOk()
        {

        }
    }
}
