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
    public class OrderControllerTest
    {
        private readonly IOrderRepo _orderrepository;
        private readonly ICartRepo _cartrepository;
         public OrderControllerTest()
        {
            _orderrepository = A.Fake<IOrderRepo>();
            _cartrepository = A.Fake<ICartRepo>();
        }
        [Fact]
        public async Task OrderController_GetOrderMasterById_ReturnOmId()
        {
            //Arrange
            var id = 1022;
            var ordermaster = new OrderMasters()
            {
                
                OrderMasterId = id,
                CardNumber = "234567890986",
                Total = 1000,
                AmountPaid = 1000,
                CustomerId=100,
                OrderDate=2022,
            };
            A.CallTo(()=>_orderrepository.GetOrderMasterById(id)).Returns(ordermaster);
            var ordermasterController = new OrderController(_orderrepository,_cartrepository);
            //Act
            var tempResult = await ordermasterController.GetOrderMaster(id);
            var result=(tempResult.Result as OkObjectResult).Value as OrderMasters;
            //Assert
            var card = "234567890986";
            card.Should().Be(ordermaster.CardNumber);
            result.Should().As<OrderMasters>();
        }
        [Fact]
        public async Task OrderController_PutOrderMaster_ReturnPut()
        {
            //Arrange
            var id = 1022;
            var ordermaster = new OrderMasters()
            {
                OrderMasterId = id,
                CardNumber = "234567890986",
                Total = 1000,
                AmountPaid = 1000,
                CustomerId = 100,
                OrderDate = 2022,
            };
            A.CallTo(()=>_orderrepository.UpdateOrderMaster(id,ordermaster)).Returns(ordermaster);
            var controller=new OrderController(_orderrepository,_cartrepository);
            //Act
            var result= await controller.PutOrderMaster(id,ordermaster);
            //Assert
            result.Value.Should().BeEquivalentTo(ordermaster);
        }
        [Fact]
        public async Task OrderController_PostOM_ReturnPostOM()
        {
            //Arrange
            var id = 1022;
            var ordermaster = new OrderMasters()
            {
                OrderMasterId = id,
                CardNumber = "234567890986",
                Total = 1000,
                AmountPaid = 1000,
                CustomerId = 100,
                OrderDate = 2022,
            };
            A.CallTo(() => _orderrepository.AddOrderMaster(ordermaster)).Returns(ordermaster);
            var controller = new OrderController(_orderrepository, _cartrepository);
            //Act
            var tempresult=await controller.PostOrderMaster(ordermaster);
            var result = (tempresult.Result as OkObjectResult).Value as OrderMasters;

            //Assert
            result.OrderDate.Should().Be(2022);
            result.Should().BeEquivalentTo(ordermaster);
        }
        [Fact]
        public async Task  OrderController_PostOrderDetail_ReturnOD()
        {
            //Arrange
            var id = 121;
            var orderdetail = new OrderDetails()
            {
                ProductId = 100,
                OrderDetailId = id,
                OrderMasterId = id + 9,
                ProductRate = 300,
                ProductQuantity = 3,

            };
            A.CallTo(() => _orderrepository.AddOrderDetail(orderdetail)).Returns(orderdetail);
            var controller= new OrderController(_orderrepository, _cartrepository);

            //Act
            var tempresult= await controller.PostOrderDetail(orderdetail);
            var result = (tempresult.Result as OkObjectResult).Value as OrderDetails;

            //Assert
            result.Should().BeEquivalentTo(orderdetail);
            result.OrderMasterId.Should().Be(130);
        }
        [Fact]
        public async Task OrderController_GetOrderDetailById_ReturnODId()
        {
            //Arrange
            var id = 121;
            var orderdetail = new OrderDetails()
            {
                ProductId = 100,
                OrderDetailId = id,
                OrderMasterId = id + 9,
                ProductRate = 300,
                ProductQuantity = 3,

            };
            A.CallTo(() => _orderrepository.GetOrderDetailById(id)).Returns(orderdetail);
            var controller = new OrderController(_orderrepository, _cartrepository);
            //Act
            var tempresult = await controller.GetOrderDetail(id);
            var result = (tempresult.Result as OkObjectResult).Value as OrderDetails;

            //Assert
            result.Should().BeEquivalentTo(orderdetail);
            result.ProductQuantity.Should().Be(3);
        }
        [Fact]
        public async Task OrderController_Buy_ReturnOk()
        {
            //Arrange
            var tempCart = new List<Carts>();
            tempCart.Add( new Carts()
            {
                CartId = 11,
                ProductQuantity = 2,
                ProductId = 101,
                CustomerId = 1000,
                Products =new Products()
                {
                    ProductId=101,
                    UnitPrice=10,
                }
                           
            });
            var ordermaster = new OrderMasters()
            {
                OrderMasterId = 1,
                CustomerId = 1000,
                OrderDate = 2022,
                Total = 20,
                CardNumber = "12345",
            };
            var orderDetail = new List<OrderDetails>();
             orderDetail.Add( new OrderDetails()
            {
                OrderDetailId = 11,
                OrderMasterId = 1,
                ProductRate = 10,
                ProductQuantity = 2,
                ProductId = 101,

            });
            A.CallTo(() => _cartrepository.GetAllCart(1000)).Returns(tempCart);
            A.CallTo(() => _orderrepository.AddOrderMaster(ordermaster)).Returns(ordermaster);
            A.CallTo(() => _orderrepository.AddOrderDetail(orderDetail[0])).Returns(orderDetail[0]);
            var controller = new OrderController(_orderrepository,_cartrepository);

            //Act
            var result = await controller.Buy(1000);

            //Assert
            result.Total.Should().Be(20);
            result.CustomerId.Should().Be(ordermaster.CustomerId);

          
        }
    }
}
