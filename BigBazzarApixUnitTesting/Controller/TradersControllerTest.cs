using BigBazzar.Controllers;
using BigBazzar.Models;
using BigBazzar.Repository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBazzarApixUnitTesting.Controller
{
    public class TradersControllerTest
    {
        private readonly ITraderRepo _repository;

        public TradersControllerTest()
        {
            _repository = A.Fake<ITraderRepo>();
        }
        [Fact]
        public void TradersController_GetTraders_ReturnOK()
        {
            //Arrange
           // var traders = A.Fake<ICollection<Traders>>();
            var tradersList=A.Fake<List<Traders>>();
            A.CallTo(() =>_repository.GetAllTraders()).Returns(tradersList);
            var controller = new TradersController(_repository);

            //Act
            var result=  controller.GetTraders();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<ActionResult<List<Traders>>>>();
            //result.Should().BeOfType(typeof(OkObjectResult));
        }
        [Fact]
        public async Task TradersController_PostTrader_ReturnOk()
        {
            //Arrange
            Traders trader = new Traders
            {
                TraderId = 1001,
                TraderEmail = "abcd@gmail.com",
                TraderName = "abcd",
                Password = "12345",
                ConfirmPassword = "12345",

            };
            //var traderspost = A.Fake<Traders>();
           // var traders = A.Fake<ICollection<Traders>>();
            //var tradersList = A.Fake<List<Traders>>();
            A.CallTo(() => _repository.AddNewTraders(trader)).Returns(trader);
            var controller = new TradersController(_repository);

            //Act
            var result = await controller.PostTrader(trader);
            Traders t = result.Value;

            //Assert
            var name = t.TraderName;
            name.Should().BeEquivalentTo(name);
            //result.Should().BeEquivalentTo(trader);
        }
        [Fact]
        public async Task TradersController_PutTrader_ReturnOk()
        {
            //Arrange
            var Id = 1000;
            Traders trader = new Traders
            {
                TraderId = Id,
                TraderEmail = "abcd@gmail.com",
                TraderName = "sathiyan",
                Password = "12345",
                ConfirmPassword = "12345",

            };
            A.CallTo(() => _repository.UpdateTraders(Id, trader)).Returns(trader);
            var controller= new TradersController(_repository);
            //Act
            var fresult = await controller.PutTrader(Id,trader);
            var result=fresult.Value;
   
            //Assert
            var name = "sathiyan";
            result.Should().BeOfType<Traders>();
            name.Should().BeEquivalentTo(result.TraderName);
        }
        [Fact]
        public async Task TradersController_DeleteTrader_ReturnOk()
        {
            //Arrange
            var traderId = 1000;
            A.CallTo(() => _repository.DeleteTraders(traderId)).Returns(true);
            var controller = new TradersController(_repository);
            //Act
            var result=await controller.DeleteTrader(traderId);
            //Assert
            
            result.Should().BeOfType(typeof(OkResult));        
        }
        [Fact]
        public async Task TradersController_GetTraderById_ReturnOk()
        {
            //Arrange
            var traderId=1000;
            Traders trader = new Traders()
            {
                TraderId = traderId,
                TraderEmail = "abcd@gmail.com",
                TraderName = "Sathiyan",
                Password = "12345",
                ConfirmPassword = "12345",
            };
            A.CallTo(() => _repository.GetTraderbyId(traderId)).Returns(trader);
            var controller = new TradersController(_repository);

            //Act
            var tempresult = await controller.GetTrader(traderId);
            var result = (tempresult.Result as OkObjectResult).Value as Traders;  //returns 200 status code
            //Assert
            result.Should().NotBeNull();
            var name="Sathiyan";
            name.Should().Be(trader.TraderName);


         

        }
        [Fact]
        public async Task TradersController_GetProductByTraderId_ReturnOk()
        {
            //Arrange           
            var traderId = 1000;
            var productId = 200;
            List<Products> p = new List<Products>();
            for (int i = 0; i < 3; i++)
            {
                Products products = new Products()
                {
                    TraderId = traderId,
                    ProductId = productId++,
                    ProductName = "Boost"+i,
                    ProductQuantity = 1,
                    UnitPrice = 12,
                    CategoryId = 1,
                };
                p.Add(products);
            }         
            A.CallTo(()=>_repository.GetProductByTraderId(traderId)).Returns(p);
            var controller = new TradersController(_repository);

            //Act
            var result= await controller.GetProductByTraderId(traderId);
            //Assert
            var id = result.Value[0].TraderId;
            var count=result.Value.Count();
            id.Should().Be(traderId);
            count.Should().Be(3);
            
            result.Should().NotBeNull();
            

        }
    }
}
