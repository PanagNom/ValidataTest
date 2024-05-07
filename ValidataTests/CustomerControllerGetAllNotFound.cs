using Application.CustomerCQRS.Commands.CreateCustomerCommand;
using Application.CustomerCQRS.Commands.DeleteCustomerCommand;
using Application.CustomerCQRS.Commands.UpdateCustomerCommand;
using Application.CustomerCQRS.Queries.GetCustomerQuery;
using Application.CustomerCQRS.Queries.GetCustomersQuery;
using Application.CustomerCQRS.Queries.GetOrdersByDate;
using Application.OrderCQRS.Commands.DeleteCustomerOrdersCommand;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Controllers;

namespace ValidataTests
{
    public class CustomerControllerGetAllNotFoundTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ICustomerRepository> _customerRepository;
        private readonly Mock<IOrderRepository> _orderRepository;

        private readonly GetCustomerQueryHandler _getCustomerQueryHandler;
        private readonly GetCustomersQueryHandler _getCustomersQueryHandler;
        private readonly GetOrdersByDateHandler _getOrdersByDateHandler;

        private readonly CreateCustomerCommandHandler _createCustomerCommandHandler;
        private readonly UpdateCustomerCommandHandler _updateCustomerCommandHandler;
        private readonly DeleteCustomerCommandHandler _deleteCustomerCommandHandler;
        private readonly DeleteCustomerOrdersCommandHandler _deleteCustomerOrdersCommandHandler;

        private readonly CustomerController _customerController;
        
        public CustomerControllerGetAllNotFoundTest()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _customerRepository = new Mock<ICustomerRepository>();
            _orderRepository = new Mock<IOrderRepository>();

            _unitOfWork.Setup(x => x.saveChanges()).ReturnsAsync(1);

            _getCustomerQueryHandler = new GetCustomerQueryHandler(_customerRepository.Object);
            _getCustomersQueryHandler = new GetCustomersQueryHandler(_customerRepository.Object);
            _getOrdersByDateHandler = new GetOrdersByDateHandler(_customerRepository.Object);

            _createCustomerCommandHandler = new CreateCustomerCommandHandler(_unitOfWork.Object, _customerRepository.Object);
            _updateCustomerCommandHandler = new UpdateCustomerCommandHandler(_unitOfWork.Object, _customerRepository.Object);
            _deleteCustomerCommandHandler = new DeleteCustomerCommandHandler(_unitOfWork.Object, _customerRepository.Object);
            _deleteCustomerOrdersCommandHandler = new DeleteCustomerOrdersCommandHandler(_unitOfWork.Object, _orderRepository.Object);

            _customerController = new CustomerController(
                _createCustomerCommandHandler,
                _updateCustomerCommandHandler,
                _deleteCustomerCommandHandler,
                _getCustomerQueryHandler, 
                _getCustomersQueryHandler, 
                _getOrdersByDateHandler,
                _deleteCustomerOrdersCommandHandler);
            
        }

        [Test]
        public async Task GetCustomers_ShouldReturnNotFound_WhenDataNotFound()
        {
            // Arrange
            List<Customer> customers = null;

            _customerRepository.Setup(c => c.GetAllAsync()).ReturnsAsync(customers);

            // Act
            var result = await _customerController.GetCustomers().ConfigureAwait(false);

            // Assess
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            _customerRepository.Verify(c => c.GetAllAsync(), Times.Once());
        }
    }
}