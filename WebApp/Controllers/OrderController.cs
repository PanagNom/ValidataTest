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

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        //private readonly IUnitOfWork _unitOfWork;
        //private readonly IOrderRepository _orderRepository;
        //private readonly ICustomerRepository _customerRepository;
        //private readonly DbContextClass _dbContextClass;
        private readonly GetProductQueryHandler _getProductQueryHandler;
        private readonly GetOrderQueryHandler _getOrderHandler;
        private readonly CreateOrderCommandHandler _createOrderCommandHandler;
        private readonly GetAllOrdersQueryHandler _getAllOrdersQueryHandler;
        public OrderController(
            //IUnitOfWork unitOfWork, 
            //IOrderRepository orderRepository, 
            //ICustomerRepository customerRepository, 
            //DbContextClass dbContextClass,
            GetProductQueryHandler getProductQueryHandler,
            GetOrderQueryHandler getOrderHandler,
            CreateOrderCommandHandler createOrderCommandHandler,
            GetAllOrdersQueryHandler getAllOrdersQueryHandler)
            {
                //_unitOfWork = unitOfWork;
                //_orderRepository = orderRepository;
                //_customerRepository = customerRepository;
                //_dbContextClass = dbContextClass;
                _getProductQueryHandler = getProductQueryHandler;
                _getOrderHandler = getOrderHandler;
                _createOrderCommandHandler = createOrderCommandHandler;
                _getAllOrdersQueryHandler = getAllOrdersQueryHandler;
            }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var query = new GetAllOrdersQuery();
            var orders = await _getAllOrdersQueryHandler.Handle(query);

            //var orders = await _orderRepository.GetAllOrders();

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
            /*
            var order = await _orderRepository.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
            */
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(int CustomerId, Order order)
        {
            var command = new CreateOrderCommand { CustomerId = CustomerId, Order = order };

            await _createOrderCommandHandler.Handle(command);

            return Ok();
            /*
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Customer customer = await _customerRepository.GetByIdAsync(order.CustomerId);

            if ( customer == null)
            {
                return BadRequest();
            }

            order.OrderDate = DateTime.Now;
            order.TotalPrice = CalculateTotalPrice(order.Items);

            await _orderRepository.Add(order);
            await _unitOfWork.saveChanges();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            */
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
