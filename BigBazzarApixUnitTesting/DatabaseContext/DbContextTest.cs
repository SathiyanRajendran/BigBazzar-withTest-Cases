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
                            ProductId = idno++,
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
                        ProductQuantity = 12+i,
                        ProductId = 101+i,
                    }
                   );
                databaseContext.OrderMasters.Add(
                     new OrderMasters()
                     {
                         OrderMasterId = idno++,
                         CustomerId = 1001+i,
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


                    await databaseContext.SaveChangesAsync();


                }
            
            return databaseContext;


        }
    }
}
