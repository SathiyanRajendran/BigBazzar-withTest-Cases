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
    public class AdminControllerTest
    {
        private readonly IAdminRepo _repository;
        public AdminControllerTest()
        {
            _repository = A.Fake<IAdminRepo>();
        }
        [Fact]
        public async Task AdminController_PostAdmin_ReturnOk()
        {
            //Arrange
            var admin = new Admin()
            {
                AdminId = 1,
                AdminName = "abcd",
                AdminEmail = "abcd@gmail.com",
                AdminPassword = "12345678",
                ConfirmPassword = "12345678",
            };
            A.CallTo(() => _repository.AddNewAdmin(admin)).Returns(admin);
            var controller = new AdminController(_repository);
            //Act
            var result = await controller.PostAdmin(admin);
            //Assert
            result.Value.Should().BeEquivalentTo(admin);

        }
        [Fact]
        public async Task AdminController_AdminDelete_ReturnOk()
        {
            //Arrange
            var adminId = 1000;
            A.CallTo(() => _repository.DeleteAdmin(adminId)).Returns(true);
            var controller = new AdminController(_repository);
            //Act
            var result = await controller.AdminDelete(adminId);
            //Assert
            result.Should().BeOfType(typeof(OkResult));
        }
        [Fact]
        public async Task AdminController_AdminLogin_ReturnOk()
        {
            //Arrange
            var adminId=1000;
            var admin = new Admin()
            {
                AdminId = adminId,
                AdminName = "admin",
                AdminEmail = "admin@gmail.com",
                AdminPassword = "12345678",
                ConfirmPassword = "12345678",
            };
            A.CallTo(() => _repository.AdminLogin(admin)).Returns(admin);
            var controller=new AdminController(_repository);

            //Act
            var result = await controller.Login(admin);
            //Assert
            result.Value.Should().BeEquivalentTo(admin);
        }
        [Fact]
        public void AdminController_GetAllCategories_ReturnOk()
        {
            //Arrange
            var categoryList = A.Fake<List<Categories>>();
            A.CallTo(() => _repository.GetAllCategories()).Returns(categoryList);
            var controller = new AdminController(_repository);
            //Act
            var result= controller.GetAllCategories();
            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<List<Categories>>>();
        }
        [Fact]
        public async Task AdminController_PostCategory_ReturnOk()
        {
            //Arrange
            var id=1000;
            var category = new Categories()
            {
                CategoryId = id,
                CategoryName = "Electronics",
            };
            A.CallTo(() => _repository.AddNewCategory(category)).Returns(category);
            var controller = new AdminController(_repository);
            //Act
            var result = await controller.PostCategory(category);
            //Assert
            result.Value.Should().BeEquivalentTo(category);
        }
        [Fact]
        public async Task AdminController_DeleteCategory_ReturnOk()
        {
            //Arrange
            var categoryId = 1000;
            A.CallTo(() => _repository.DeleteCategory(categoryId)).Returns(true);
            var controller= new AdminController(_repository);
            //Act
            var result= await controller.DeleteCategory(categoryId);
            //Assert
            result.Should().BeOfType(typeof(OkResult));
        }

    }
}
