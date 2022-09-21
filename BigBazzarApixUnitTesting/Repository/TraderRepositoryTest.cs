using BigBazzar.Data;
using BigBazzar.Models;
using BigBazzar.Repository;
using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBazzarApixUnitTesting.Repository
{
    public class TraderRepositoryTest
    {
        private async Task<BigBazzarContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<BigBazzarContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;
            var databaseContext=new BigBazzarContext(options);
            databaseContext.Database.EnsureCreated();
            int idno = 10;
            if(await databaseContext.Traders.CountAsync()<=0)
            {
                for(int i=0;i<5;i++)
                {
                    var trader = new Traders()
                    {
                        TraderId = idno++,
                        TraderEmail = "abcd" + i + "@gmail.com",
                        TraderName = "abc" + i,
                        Password = "12345678",
                        ConfirmPassword = "12345678",
                    };
                    databaseContext.Traders.Add(trader);
                    await databaseContext.SaveChangesAsync();

                }
            }
            return databaseContext;


        }
        [Fact]
        public async Task TraderRepo_GetAllTraders_ReturnsTraders()
        {
            //Arrange
            
            var dbContext=await GetDatabaseContext();
            var tradersRepository = new TraderRepo(dbContext);
            //Act
            var result = await tradersRepository.GetAllTraders();

            //Assert
            var count=result.Count();
           result.Should().NotBeNull();
            result.Should().HaveCount(count);
        }
        [Fact]
        public async Task TraderRepo_GetTraderbyId_ReturnTraderById()
        {
            var id = 11;
            Traders traders1 = new Traders()
            {
                TraderId = id,
                TraderEmail = "abcd1@gmail.com",
                TraderName = "abc1",
                Password = "12345678",
                ConfirmPassword = "12345678",
            };
            //Arrange
            var dbContext=await GetDatabaseContext();
            var tradersRepository=new TraderRepo(dbContext);
            //Act
            var result = await tradersRepository.GetTraderbyId(id);
            //Assert
            var name = result.TraderName;
            result.Should().BeEquivalentTo(traders1);
        }
        [Fact]
        public async Task TraderRepo_AddNewTraders_ReturnPostTrader()
        {
            Traders traders1 = new Traders()
            {
                TraderEmail = "abcd3@gmail.com",
                TraderName = "abc3",
                Password = "12345678",
                ConfirmPassword = "12345678",
            };
            //Arrange
            var dbContext = await GetDatabaseContext();
            var traderRepository = new TraderRepo(dbContext);
            //Act
            var result=await traderRepository.AddNewTraders(traders1);
            //Assert
            result.Should().BeEquivalentTo(traders1);
            dbContext.Traders.Count().Should().Be(6);

        }
        [Fact]
        public async Task TraderRepo_DeleteTraders_ReturnDelete()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var traderRepository = new TraderRepo(dbContext);
            //Act
            var result = await traderRepository.DeleteTraders(10);

            //Assert
            result.Should().BeTrue();          
            dbContext.Traders.Should().HaveCount(4);
        }
        [Fact]
        public async Task TraderRepo_UpdateTraders_ReturnEdit()
        {
            //Arrange
            var id = 11;
            Traders traders = new Traders()
            {
                TraderId = id,
                TraderEmail = "abc1@gmail.com",
                TraderName = "abcrdrrd",
                Password = "12345678",
                ConfirmPassword = "12345678",
            };
            
            var dbContext=await GetDatabaseContext();
            var traderRepository=new TraderRepo(dbContext);
            //Act
            var trader = await dbContext.Traders.FindAsync(traders.TraderId);
            dbContext.Entry<Traders>(trader).State = EntityState.Detached;//has to be used only on xUnittesting
            var result = await traderRepository.UpdateTraders(id,traders);
            //Assert
            result.Should().BeEquivalentTo(traders);
            dbContext.Traders.Should().HaveCount(5);
        }
        [Fact]
        public async Task TraderRepo_GetProductByTraderId_ReturnsProduct()
        {
            
        }
    }
}
