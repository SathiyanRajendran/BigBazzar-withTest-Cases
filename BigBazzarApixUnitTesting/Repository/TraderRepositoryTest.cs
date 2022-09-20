using BigBazzar.Data;
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
    public class TraderRepositoryTest
    {
        private async Task<BigBazzarContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<BigBazzarContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext=new BigBazzarContext(options);
            databaseContext.Database.EnsureCreated();
            int idno = 10;
            if(await databaseContext.Traders.CountAsync()<=0)
            {
                for(int i=0;i<3;i++)
                {
                    databaseContext.Traders.Add(
                        new BigBazzar.Models.Traders()
                        {
                            TraderId = idno++,
                            TraderEmail = "abcd" + i + "@gmail.com",
                            TraderName = "abc" +i,
                            Password = "12345678",
                            ConfirmPassword = "12345678",
                        }) ;
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
           result.Should().NotBeNull();
        }
        [Fact]
        public async Task TraderRepo_GetTraderbyId_ReturnTraderById()
        {
            //Arrange
             var dbContext=await GetDatabaseContext();
            var tradersRepository=new TraderRepo(dbContext);
            //Act
            var traders = await tradersRepository.GetAllTraders();
        }
    }
}
