using Application.CustomerCQRS.Commands.CreateCustomerCommand;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace ValidataTests
{
    public class Tests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ICustomerRepository> _customerRepository;
        private CreateCustomerCommandHandler _createCustomerCommandHandler;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _customerRepository = new Mock<ICustomerRepository>();

            _unitOfWork.Setup(x => x.saveChanges()).ReturnsAsync(1);

            _customerRepository.Setup(c => c.AddAsync(It.IsAny<Customer>()));

            _createCustomerCommandHandler = new CreateCustomerCommandHandler(_unitOfWork.Object, _customerRepository.Object);
        }

        [Test]
        [TestCase("Panagiotis", "Nomikos", "Kamares", "25009")]
        public async Task CreateCustomerTest(string firtsName,
            string lastName, string address, string postalCode)
        {
            // Arrange
            Customer testCustomer = new Customer(firtsName, lastName, address, postalCode);
            var command = new CreateCustomerCommand();
            command.Customer = testCustomer;
            // Act
            await _createCustomerCommandHandler.Handle(command);      
            // Assess
            Assert.Pass();
        }
    }
}