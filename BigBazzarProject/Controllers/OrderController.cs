using BigBazzar.Models;
using BigBazzar.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BigBazzar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        private readonly IOrderRepo _orderrepository;
        private readonly ICartRepo _cartrepository;
        public OrderController(IOrderRepo orderrepository, ICartRepo cartrepository)
        {
            _orderrepository = orderrepository;
            _cartrepository = cartrepository;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderMasters>> GetOrderMaster(int id)//HERE WE GET THE ORDERMASTER TABLE OF ID WE GIVE(OM ID) 
                                                                                 //(API/ORDER/ID)
        {
            return await _orderrepository.GetOrderMasterById(id);
        }
        [HttpPut]
        public async Task<ActionResult<OrderMasters>> PutOrderMaster(int id, OrderMasters om)     //Here I update the order master by the total amount=amount paid by the 
                                                                                     //customer then the order master updated here.
                                                                                     //(API/ORDER)
        {
            return await _orderrepository.UpdateOrderMaster(id, om);
        }
        [HttpPost("orderMaster")]
        public async Task<ActionResult<OrderMasters>> PostOrderMaster(OrderMasters om)
        {
            return await _orderrepository.AddOrderMaster(om);
        }
        [HttpPost("orderDetail")]
        public async Task<ActionResult<OrderDetails>> PostOrderDetail(OrderDetails od)
        {
            return await _orderrepository.AddOrderDetail(od);
        }
        [HttpGet("orderDetail")]
        public async Task<ActionResult<OrderDetails>> GetOrderDetail(int id)
        {
            return await _orderrepository.GetOrderDetailById(id);
        }
        [HttpPost("{customerid}")]
        public async Task<OrderMasters> Buy([FromRoute]int customerid) //THIS WILL SHOWS THE ORDER DETAILS AND ORDER MASTER OF THE DB 
                                                                       //IT CALCULATE THE TOTAL AMOUNT OF THE PRODUCTS.
                                                                       //WHEN I GIVE BUY,ORDERDETAIL AND ORDERMASTER AUTOMATICALLY FILLED.
                                                                       //(API/ORDER/CUSTOMERID)
        {
            List<Carts> c = await _cartrepository.GetAllCart(customerid);
                                    //IT CAN SHOW ALL THE CARTS OF THE PARTICULAR CUSTOMERID AND ORDERMASTER IS CREATED FOR THIS ORDER
            OrderMasters om = new OrderMasters();
            om.OrderDate = 0;  //instead of datetime I used integer for date.
            om.CustomerId = customerid;
            om.Total = 0;
            if(c!=null)  //Whenever he have a cart he is ready to buy the carts and total is calculated here
            {
                foreach(var cart in c)
                {
                    om.Total += (cart.ProductQuantity * Convert.ToInt32(cart.Products.UnitPrice));

                }
            }
            await _orderrepository.AddOrderMaster(om);
      //--------------------------------------------------------------------------
            foreach(var item in c)
            {                                                //ORDERDETAIL TO BE FILLED AFTER CUSTOMER CLICKS THE BUY BUTTON.
                                                             //IF THERE IS MORETHAN ONE PRODUCT ON CART SEPARATE ORDERDETAIL ID SHOULD BE GIVEN.
                                                             //ORDERDETAIL ID IS LIKE S.NO IN THE PARTICULARS(INVOICE BILL)
                OrderDetails od = new OrderDetails();
                od.ProductId= item.ProductId;
                od.ProductQuantity= item.ProductQuantity;
                od.ProductRate = item.Products.UnitPrice;
                od.OrderMasterId = om.OrderMasterId;
                await _orderrepository.AddOrderDetail(od);
            }
            return om;
        }

    }
}
