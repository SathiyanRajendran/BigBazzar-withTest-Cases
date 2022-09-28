using BigBazzar.Data;
using BigBazzar.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBazzarApixUnitTesting.DatabaseContext
{
    public class DbContextTest
    {
        public async Task<BigBazzarContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<BigBazzarContext>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .Options;
            var databaseContext = new BigBazzarContext(options);
            databaseContext.Database.EnsureCreated();
            int idno = 1000;
            int feedbackid = 2000;
            int adminId = 3000;
            int categoryId = 4000;



                for (int i = 0; i < 4; i++)
                {
                    databaseContext.Traders.Add(
                        new Traders()
                        {
                            TraderId = idno++,
                            TraderEmail = "abcd" + i + "@gmail.com",
                            TraderName = "abc" + i,
                            Password = "12345678",
                            ConfirmPassword = "12345678",
                        }

                        );
                    databaseContext.Products.Add(
                        new Products()
                        {
                            TraderId=1000,
                            ProductId = 1000+i,
                            ProductName = "Boost" + i,
                            UnitPrice = 150 + i,
                            ProductQuantity = 12,
                        }

                        );
                databaseContext.Carts.Add(
                    new Carts()
                    {
                        CustomerId = 1000,
                        CartId = idno++,
                        ProductQuantity = 1+i,
                        ProductId = 1000+i,
                    }
                   );
                databaseContext.OrderMasters.Add(
                     new OrderMasters()
                     {
                         OrderMasterId = idno++,
                         CustomerId = 1000+i,
                         CardNumber="890678"+i,
                         Total=1200+i,
                         AmountPaid=1200 + i,
                         
                     }
                    );
                databaseContext.OrderDetails.Add(
                    new OrderDetails()
                    {
                        OrderDetailId = idno++,
                        OrderMasterId = 10 + i,
                        ProductId = 100 + i,
                        ProductRate = 150 + i,
                        ProductQuantity = 2 + i,
                    }
                    );
                databaseContext.Feedbacks.Add(
                    new Feedback()
                    {
                        FeedbackId = feedbackid++,
                        Comment =i + "Product is Nice",
                        ProductId = 1000 + i,
                        CustomerId = 1000 + i,
                    }
                    );
                databaseContext.Admins.Add(
                    new Admin()
                    {
                        AdminId = adminId++,
                        AdminName = "admin" + i,
                        AdminEmail = "admin" + i + "@gmail.com",
                        AdminPassword = "12345678",
                        ConfirmPassword = "12345678",
                    }
                    );
                databaseContext.Categories.Add(
                    new Categories()
                    {
                        CategoryId = categoryId++,
                        CategoryName = "Electronics" + i,
                    }
                    );



                    await databaseContext.SaveChangesAsync();


                }
            
            return databaseContext;


        }
    }
}
