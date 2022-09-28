using BigBazzar.Controllers;
using BigBazzar.Models;
using BigBazzar.Repository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBazzarApixUnitTesting.Controller
{
    public class CustomerControllerTests
    {
        private readonly ICustomerRepo _repository;
        public CustomerControllerTests()
        {
            _repository = A.Fake<ICustomerRepo>();
        }
        [Fact]
        public async Task CustomersController_GetCustomers_ReturnOk()
        {
            //Arrange
            var customerlist = A.Fake<List<Customers>>();
            A.CallTo(()=>_repository.GetAllCustomers()).Returns(customerlist);
            var controller = new CustomersController(_repository);

            //Act
            var result= await controller.GetCustomers();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<IEnumerable<Customers>>>();

        }
        [Fact]
        public async Task CustomersController_GetCustomerById_ReturnOk()
        {
            //Arrange
            var id = 1000;
            Customers customers = new Customers()
            {
                CustomerId= id,
                CustomerName = "abcd",
                CustomerEmail = "abc@gmail.com",
                CustomerCity = "xxxxx",
                Password = "12345678",
                ConfirmPassword = "12345678",

            };
            A.CallTo(()=>_repository.GetCustomerById(id)).Returns(customers);
            var controller = new CustomersController(_repository);

            //Act
            var result =await controller.GetCustomer(id);
            //Assert
            var name = "abcd";
            name.Should().BeSameAs(customers.CustomerName);
        }
        [Fact]
        public async Task CustomersController_PostCustomer_ReturnOk()
        {
            //Arrange
            Customers customers = new Customers()
            {
                CustomerId = 1000,
                CustomerName = "abcd",
                CustomerEmail = "abc@gmail.com",
                CustomerCity = "swswsw",
                Password = "1234",
                ConfirmPassword = "1234",
            };
            A.CallTo(() => _repository.AddCustomer(customers)).Returns(customers);
            var controller= new CustomersController(_repository);

            //Act
            var result=await controller.PostCustomer(customers);
            //Assert
            result.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(customers);
        }
        [Fact]
        public async Task CustomersController_PutCustomer_ReturnOk()
        {
            //Arrange
            var id = 1000;
            var customers = new Customers()
            {
                CustomerId= id,
                CustomerName="abcd",
                CustomerEmail="abc@gmail.com",
                CustomerCity="xsxsx",
                Password="12345",
                ConfirmPassword="12345",
            };
            A.CallTo(() => _repository.UpdateCustomer(id, customers)).Returns(customers);
            var controller=new CustomersController(_repository);
            //Act
            var result = await controller.PutCustomer(id,customers);
            //Assert
            var name = "abcd";
            name.Should().BeSameAs(customers.CustomerName);
            result.Value.Should().BeEquivalentTo(customers);
        }

        [Fact]
        public async Task CustomersController_CustomerLogin_ReturnOk()
        {
            //Arrange
            var id = 1000;
            var customer = new Customers()
            {
                CustomerId = id,
                CustomerEmail = "abcd@gmail.com",
                CustomerName = "abcd",
                Password = "12345678",
                ConfirmPassword = "12345678",
            };

            A.CallTo(() => _repository.CustomerLogin(customer)).Returns(customer);
            var controller=new CustomersController(_repository);

            //Act
            var result = await controller.CustomerLogin(customer);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]

        public async Task CustomersController_PostFeedback_ReturnOk()
        {
            //Arrange
            var id = 2000;
            var feedback = new Feedback()
            {
                FeedbackId = id,
                Comment = "Try to Improve your website.",
                CustomerId = 1000,
                ProductId = 1000,
            };
            A.CallTo(()=>_repository.AddFeedback(feedback)).Returns(feedback);
            var controller=new CustomersController(_repository);

            //Act
            var result = await controller.PostFeedback(feedback);
            //Assert
            result.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(feedback);
        }
    }
}
