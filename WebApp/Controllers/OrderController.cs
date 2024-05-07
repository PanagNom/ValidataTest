using Domain.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.OrderCQRS.Commands.CreateOrderCommand;
using Application.OrderCQRS.Queries.GetAllOrdersQuery;
using Application.OrderCQRS.Queries.GetOrderQuery;
using Application.ProductCQRS.Queries.GetProductQuery;
using Application.CustomerCQRS.Queries.GetCustomerQuery;
using Application.CustomerCQRS.Commands.UpdateCustomerCommand;
using Application.OrderCQRS.Commands.UpdateOrderCommand;
using Application.OrderCQRS.Commands.DeleteOrderCommand;
using Application.OrderCQRS.Commands.DeleteCustomerOrdersCommand;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly GetProductQueryHandler _getProductQueryHandler;
        private readonly GetOrderQueryHandler _getOrderHandler;
        private readonly CreateOrderCommandHandler _createOrderCommandHandler;
        private readonly GetAllOrdersQueryHandler _getAllOrdersQueryHandler;
        private readonly UpdateOrderCommandHandler _updateOrderCommandHandler;
        private readonly DeleteOrderCommandHandler _deleteOrderCommandHandler;
        private readonly DeleteCustomerOrdersCommandHandler _deleteCustomerOrdersCommandHandler;

        public OrderController(
            GetProductQueryHandler getProductQueryHandler,
            GetOrderQueryHandler getOrderHandler,
            CreateOrderCommandHandler createOrderCommandHandler,
            GetAllOrdersQueryHandler getAllOrdersQueryHandler,
            UpdateOrderCommandHandler updateOrderCommandHandler,
            DeleteOrderCommandHandler deleteOrderCommandHandler,
            DeleteCustomerOrdersCommandHandler deleteCustomerOrdersCommandHandler)
            {
                _getProductQueryHandler = getProductQueryHandler;
                _getOrderHandler = getOrderHandler;
                _createOrderCommandHandler = createOrderCommandHandler;
                _getAllOrdersQueryHandler = getAllOrdersQueryHandler;
                _updateOrderCommandHandler = updateOrderCommandHandler;
                _deleteOrderCommandHandler = deleteOrderCommandHandler;
                _deleteCustomerOrdersCommandHandler = deleteCustomerOrdersCommandHandler;
            }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var query = new GetAllOrdersQuery();
            var orders = await _getAllOrdersQueryHandler.Handle(query);

            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var query = new GetOrderQuery { OrderId = id };
            var order = await _getOrderHandler.Handle(query);

            if(order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(int CustomerId, Order order)
        {
            var command = new CreateOrderCommand { CustomerId = CustomerId, Order = order };
            try
            {
                await _createOrderCommandHandler.Handle(command);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(int Id, Order order)
        {
            if (Id != order.Id)
            {
                return BadRequest();
            }

            var query = new GetOrderQuery { OrderId = Id };
            var databaseOrder = await _getOrderHandler.Handle(query);

            if (databaseOrder == null)
            {
                return NotFound("Not found");
            }

            databaseOrder.CustomerId = order.CustomerId;
            databaseOrder.Items = order.Items;

            var command = new UpdateOrderCommand { OrderID = Id, Order = databaseOrder };
            try
            {
                await _updateOrderCommandHandler.Handle(command);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Database concurrency issue.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand { OrderID= id };

            if(await _deleteOrderCommandHandler.Handle(command)==0)
            {
                return BadRequest("Order does not exist.");
            }

            return NoContent();
        }

        [HttpDelete("/customer/{id}")]
        public async Task<IActionResult> DeleteCustomerOrders(int id)
        {
            var command = new DeleteCustomerOrdersComannd { CustomerId = id };

            await _deleteCustomerOrdersCommandHandler.Handle(command);
            
            return NoContent();
        }

        private async Task<decimal> CalculateTotalPrice(List<Item> items)
        {
            decimal totalPrice = 0;
            foreach (var item in items)
            {
                var query = new GetProductQuery {Id = item.ProductId };

                var product =  await _getProductQueryHandler.Handle(query);

                if (product == null)
                {
                    throw new InvalidOperationException("Product not found");
                }
                totalPrice += item.Quantity * product.Price;
            }
            return totalPrice;
        }
    }
}
