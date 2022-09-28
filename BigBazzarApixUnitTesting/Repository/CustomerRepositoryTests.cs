using BigBazzar.Data;
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
    public class CustomerRepositoryTests
    {
        private async Task<BigBazzarContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<BigBazzarContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;
            var databaseContext = new BigBazzarContext(options);
            databaseContext.Database.EnsureCreated();
            int idno = 10;
            if (await databaseContext.Traders.CountAsync() <= 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    var customer = new Customers()
                    {
                        CustomerId = idno++,
                        CustomerEmail = "abcd" + i + "@gmail.com",
                        CustomerName = "abc" + i,
                        Password = "12345678",
                        ConfirmPassword = "12345678",
                    };
                    databaseContext.Customers.Add(customer);
                    await databaseContext.SaveChangesAsync();

                }
            }
            return databaseContext;
        }
        [Fact]
        public async Task CustomerRepo_GetAllCustomers_ReturnCustomers()
        {
            //Arrange
            var dbContext=await GetDatabaseContext(); //This one calls the inmemory database
            var customersRepository=new CustomerRepo(dbContext); //repo layer object calling

            //Act
            var result=await customersRepository.GetAllCustomers(); //calling the methods of repository
            //Assert
            var count=result.Count();
            result.Should().HaveCount(count);
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task CustomerRepo_GetCustomerById_ReturnCustomerId()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var customersRepository=new CustomerRepo(dbContext);

            //Act
            var result = await customersRepository.GetCustomerById(10);
            //Assert
            var name = "abc0";
            name.Should().Be(result.CustomerName);

        }
        [Fact]
        public async Task CustomerRepo_AddCustomer_ReturnPost()
        {
            //Arrange
            var customer = new Customers()
            {
                CustomerName = "abcd",
                CustomerEmail="abc5@gmail.com",
                CustomerCity="madhapur",
                Password="1234",
                ConfirmPassword="1234",

            };
            var dbContext=await GetDatabaseContext();
            var customerRepository=new CustomerRepo(dbContext);
            //Act
            var result = await customerRepository.AddCustomer(customer);
            //Assert
            result.Should().BeEquivalentTo(customer);
            dbContext.Customers.Should().HaveCount(5);
        }
        [Fact]
        public async Task CustomerRepo_UpdateCustomer_ReturnEdit()
        {
            //Arrange
            var id = 10;
            var customer = new Customers()
            {
                CustomerId = id,
                CustomerName = "abcdefgh",
                CustomerEmail = "abcdefgh@gmail.com",
                CustomerCity = "asdfgh",
                Password="12345",
                ConfirmPassword="12345",
            };
            var dbContext= await GetDatabaseContext();
            var customersRepository=new CustomerRepo(dbContext);

            //Act
            //---------------------------------------------------------
            var customerfind = await dbContext.Customers.FindAsync(customer.CustomerId);
            dbContext.Entry<Customers>(customerfind).State = EntityState.Detached;//has to be used only on xUnittesting
            //----------------------------------------------------------
            var result = await customersRepository.UpdateCustomer(id,customer);
            //Assert
            result.Should().BeEquivalentTo(customer);
            dbContext.Customers.Should().HaveCount(4);

        }
        [Fact]
        public async Task CustomerRepo_CustomerLogin_ReturnOk()
        {
            //Arrange
            var customer = new Customers()
            {
                CustomerEmail = "abcd0@gmail.com",
                Password = "12345678",
                ConfirmPassword = "12345678",
            };
            var dbContext=await GetDatabaseContext();
            var customerRepo=new CustomerRepo(dbContext);

            //Act
            var result = await customerRepo.CustomerLogin(customer);
            //Assert
            var name = "abc0";
            var id = 10;
            result.CustomerName.Should().Be(name);
            result.CustomerId.Should().Be(id);
        }

        [Fact]
        public async Task CustomerRepo_AddFeedback_ReturnOk()
        {
            //Arrange
            var feedback = new Feedback()
            {
                CustomerId = 1000,
                ProductId = 1000,
                Comment = "Your website is too good",
            };
            var inmemorytest = new DbContextTest();
            var dbContext=await inmemorytest.GetDatabaseContext();
            var customerRepo = new CustomerRepo(dbContext);
            //Act
            var result=await customerRepo.AddFeedback(feedback);

            //Assert
            dbContext.Feedbacks.Should().HaveCount(5);
            result.FeedbackId.Should().Be(2004);
        }
    }
}
