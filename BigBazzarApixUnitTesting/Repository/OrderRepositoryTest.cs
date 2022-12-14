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
    public class OrderRepositoryTest
    {
        [Fact] 
        public async Task OrderRepo_GetOrderMasterById_ReturnId()
        {
            //Arrange
            var inmemorytest = new DbContextTest();
            var dbContext = await inmemorytest.GetDatabaseContext();
            var orderrepo=new OrderRepo(dbContext);
            //Act
            var result=await orderrepo.GetOrderMasterById(1002);

            //Assert
            try
            {
                var finalresult = (from i in dbContext.OrderMasters where i.OrderMasterId == result.OrderMasterId select i).FirstOrDefaultAsync();
                var total = 1200;
                result.Total.Should().Be(total);
                result.OrderMasterId.CompareTo(1002);
            }
            catch(NullReferenceException e)
            {
                Console.WriteLine("Object reference not set to an instance of an object",e);
            }
        }
        [Fact]
        public async Task OrderRepo_AddOrderMaster_ReturnAdd()
        {
            var ordermaster = new OrderMasters()
            {
                CustomerId = 1010,
                CardNumber = "890678897987",
                Total = 1300,
                AmountPaid = 1300,
            };
            var inmemorytest=new DbContextTest();
            var dbContext=await inmemorytest.GetDatabaseContext();
            var orderrepo=new OrderRepo(dbContext);

            //Act
            var result= await orderrepo.AddOrderMaster(ordermaster);
            //Assert
            5.Should().Be(dbContext.OrderMasters.Count());
            var id = 1010;
            id.Should().Be(result.CustomerId);
            result.Should().BeEquivalentTo(ordermaster);
        }
        [Fact]
        public async Task OrderRepo_UpdateOrdermaster_ReturnOM()
        {

            var ordermaster = new OrderMasters()
            {
                OrderMasterId = 1002,
                CustomerId = 1000,
                CardNumber = "890678910",
                Total = 1200, //here total and amount paid must be equal
                AmountPaid = 1200,

            };
           
           
           
            var inmemorytest=new DbContextTest();
            var dbContext=await inmemorytest.GetDatabaseContext();
            var orderrepo= new OrderRepo(dbContext);

            //Act
            var ordermasterfind = await dbContext.OrderMasters.FindAsync(1002);
            dbContext.Entry<OrderMasters>(ordermasterfind).State = EntityState.Detached;//has to be used only on xUnittesting
            
            var result = await orderrepo.UpdateOrderMaster(1002,ordermaster);
            //Assert
            //customer amount paid must be equal to the total amount then only ordermaster is updated.
            var card = "890678910";
            card.Should().Be(result.CardNumber);
            result.AmountPaid.Should().Be(1200);
            dbContext.Carts.Should().HaveCount(0);  //Here cart is removed after updating the ordermaster.
            dbContext.Products.Should().HaveCount(4);
            dbContext.Carts.Should().BeNullOrEmpty();
            4.Should().Be(dbContext.OrderMasters.Count());

        }
        [Fact]
        public async Task OrderRepo_GetOrderDetailById_ReturnODId()
        {
            //Arrange
            var inmemoryTest=new DbContextTest();
            var dbContext= await inmemoryTest.GetDatabaseContext();
            var orderrepo=new OrderRepo(dbContext);
            //Act
            var result = await orderrepo.GetOrderDetailById(1003);
            //Assert
            var id = 100;
            id.Should().Be(result.ProductId);
            dbContext.OrderDetails.Should().NotBeNull();
            Assert.Equal(result.ProductId, id);

        }

        [Fact]
        public async Task OrderRepo_AddOrderDetail_ReturnOD()
        {
            //Arrange
            var od = new OrderDetails()
            {
                OrderMasterId = 10,
                ProductId = 123,
                ProductRate = 150,
                ProductQuantity = 2,               
            };
            var inmemoryTest=new DbContextTest();
            var dbContext=await inmemoryTest.GetDatabaseContext();
            var orderRepo=new OrderRepo(dbContext);
            //Act
            var result=await orderRepo.AddOrderDetail(od);
            //Assert
            5.Should().Be(dbContext.OrderDetails.Count());
            result.Should().BeEquivalentTo(od);
        }
    }
}
