using Application.CustomerCQRS.Queries.GetCustomerQuery;
using Application.OrderCQRS.Commands.CreateOrderCommand;
using Application.OrderCQRS.Commands.DeleteCustomerOrdersCommand;
using Application.OrderCQRS.Commands.DeleteOrderCommand;
using Application.OrderCQRS.Commands.UpdateOrderCommand;
using Application.OrderCQRS.Queries.GetAllOrdersQuery;
using Application.OrderCQRS.Queries.GetCustomerOrdersQuery;
using Application.OrderCQRS.Queries.GetOrderQuery;
using Application.ProductCQRS.Queries.GetProductQuery;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Controllers;

namespace ValidataTests
{
    public class OrderControllerGetAllOkTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ICustomerRepository> _customerRepository;
        private readonly Mock<IOrderRepository> _orderRepository;
        private readonly Mock<IProductRepository> _productRepository;

        private readonly GetProductQueryHandler _getProductQueryHandler;

        private readonly GetCustomerQueryHandler _getCustomerQueryHandler;

        private readonly GetOrderQueryHandler _getOrderQueryHandler;
        private readonly GetAllOrdersQueryHandler _getAllOrderQueryHandler;
        private readonly GetCustomerOrdersQueryHandler _getCustomerOrdersQueryHandler;

        private readonly CreateOrderCommandHandler _createOrderCommandHandler;
        private readonly UpdateOrderCommandHandler _updateOrderCommandHandler;
        private readonly DeleteOrderCommandHandler _deleteOrderCommandHandler;
        private readonly DeleteCustomerOrdersCommandHandler _deleteCustomerOrdersCommandHandler;

        private readonly OrderController _orderController;
        
        public OrderControllerGetAllOkTest()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _customerRepository = new Mock<ICustomerRepository>();
            _orderRepository = new Mock<IOrderRepository>();
            _productRepository = new Mock<IProductRepository>();

            _getProductQueryHandler = new GetProductQueryHandler(_productRepository.Object);

            _getCustomerQueryHandler = new GetCustomerQueryHandler(_customerRepository.Object);

            _getOrderQueryHandler = new GetOrderQueryHandler(_orderRepository.Object);
            _getAllOrderQueryHandler = new GetAllOrdersQueryHandler(_orderRepository.Object);
            _getCustomerOrdersQueryHandler = new GetCustomerOrdersQueryHandler(_orderRepository.Object);

            _createOrderCommandHandler = new CreateOrderCommandHandler(_unitOfWork.Object, 
                _orderRepository.Object, _customerRepository.Object, _productRepository.Object);
            _updateOrderCommandHandler = new UpdateOrderCommandHandler(_unitOfWork.Object, _orderRepository.Object, _productRepository.Object);
            _deleteOrderCommandHandler = new DeleteOrderCommandHandler(_unitOfWork.Object, _orderRepository.Object);
            _deleteCustomerOrdersCommandHandler = new DeleteCustomerOrdersCommandHandler(_unitOfWork.Object, _orderRepository.Object);

            _orderController = new OrderController(
                _getProductQueryHandler,
                _getOrderQueryHandler,
                _createOrderCommandHandler,
                _getAllOrderQueryHandler,
                _updateOrderCommandHandler,
                _deleteOrderCommandHandler,
                _deleteCustomerOrdersCommandHandler);
            
        }

        [Test]
        [TestCase("Panagiotis", "Nomikos", "Kamares", "25009")]
        public async Task GetOrders_ShouldReturnOkResponse_WhenDataFound(string name, string surname, string address, string postalcode)
        {
            // Arrange
            List<Order> orders = new List<Order>();
            orders.Add(new Order ());
            orders.Add(new Order ());
            
            _orderRepository.Setup(o => o.GetAllOrders()).ReturnsAsync(orders);

            // Act
            var result = await _orderController.GetOrders().ConfigureAwait(false);

            // Assess
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<IEnumerable<Order>>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
                .Should().NotBeNull()
                .And.BeOfType(orders.GetType());
            _orderRepository.Verify(c => c.GetAllOrders(), Times.Once());
        }
    }
}