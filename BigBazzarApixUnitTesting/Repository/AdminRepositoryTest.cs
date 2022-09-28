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
    public class AdminRepositoryTest
    {
        [Fact]
        public async Task AdminRepo_AddNewAdmin_ReturnOk()
        {
            //Arrange
            var admin = new Admin()
            {
                AdminName = "Sadan",
                AdminEmail = "sadan@gmail.com",
                AdminPassword = "12345678",
                ConfirmPassword = "12345678",
            };
            var useinmemorytest = new DbContextTest();
            var dbContext = await useinmemorytest.GetDatabaseContext();
            var adminRepo = new AdminRepo(dbContext);

            //Act
            var result = await adminRepo.AddNewAdmin(admin);

            //Assert
            dbContext.Admins.Should().HaveCount(5);
            result.Should().BeEquivalentTo(admin);
        }

        [Fact]
        public async Task AdminRepo_AddNewCategory_ReturnOk()
        {
            //Arrange
            var category = new Categories()
            {
                CategoryName = "Grocery",
            };
            var useinmemorytest = new DbContextTest();
            var dbContext = await useinmemorytest.GetDatabaseContext();
            var adminRepo = new AdminRepo(dbContext);
            //Act
            var result = await adminRepo.AddNewCategory(category);
            //Assert
            dbContext.Categories.Should().HaveCount(5);
            result.Should().BeEquivalentTo(category);
            var id = 4004;
            result.CategoryId.Should().Be(id);

        }
        [Fact]
        public async Task AdminRepo_AdminLogin_ReturnOk()
        {
            //Arrange
            var admin = new Admin()
            {
                AdminEmail = "admin0@gmail.com",
                AdminPassword = "12345678",
            };
            var useinmemorytest = new DbContextTest();
            var dbContext = await useinmemorytest.GetDatabaseContext();
            var adminRepo = new AdminRepo(dbContext);
            //Act
            var result = await adminRepo.AdminLogin(admin);
            //Assert
            var name = "admin0";
            result.AdminName.Should().Be(name);
        }

        [Fact]
        public async Task AdminRepo_DeleteAdmin_ReturnOk()
        {
            //Arrange
            var useinmemorytest = new DbContextTest();
            var dbContext = await useinmemorytest.GetDatabaseContext();
            var adminRepo = new AdminRepo(dbContext);
            //Act
            var result = await adminRepo.DeleteAdmin(3000);
            //Assert
            dbContext.Admins.Should().HaveCount(3);
            result.Should().BeTrue();

        }
        [Fact]
        public async Task AdminRepo_DeleteCategory_ReturnOk()
        {
            //Arrange
            var useinmemorytest = new DbContextTest();
            var dbContext = await useinmemorytest.GetDatabaseContext();
            var adminRepo = new AdminRepo(dbContext);
            //Act
            var result = await adminRepo.DeleteCategory(4000);
            //Assert
            dbContext.Categories.Should().HaveCount(3);
            result.Should().BeTrue();

        }

        [Fact]
        public async Task AdminRepo_GetAllCategories_ReturnOk()
        {
            //Arrange
            var useinmemorytest = new DbContextTest();
            var dbContext = await useinmemorytest.GetDatabaseContext();
            var adminRepo = new AdminRepo(dbContext);
            //Act
            var result = await adminRepo.GetAllCategories();
            //Assert
            var count = result.Count();
            dbContext.Categories.Should().HaveCount(count);
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task AdminRepo_EditAdmin_ReturnOk()
        {
            //Arrange
            var adminId = 3000;
            var admin = new Admin()
            {
                AdminId = adminId,
                AdminName = "Sathiyan",
                AdminEmail = "sathiyan@gmail.com",
                AdminPassword = "12345678",
                ConfirmPassword = "12345678",
            };
            var useinmemorytest = new DbContextTest();
            var dbContext = await useinmemorytest.GetDatabaseContext();
            var adminRepo = new AdminRepo(dbContext);

            //Act
            var adminFind = await dbContext.Admins.FindAsync(adminId);
            dbContext.Entry<Admin>(adminFind).State = EntityState.Detached;//has to be used only on xUnittesting
            var result=await adminRepo.EditAdmin(adminId,admin);

            //Assert

            dbContext.Admins.Should().HaveCount(4);
            result.Should().BeEquivalentTo(admin);

        }

    }
}
