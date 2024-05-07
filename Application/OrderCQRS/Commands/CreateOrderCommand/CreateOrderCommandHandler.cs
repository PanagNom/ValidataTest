using Domain.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.OrderCQRS.Commands.CreateOrderCommand
{
    public class CreateOrderCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;  
        private readonly IProductRepository _productRepository;

        public CreateOrderCommandHandler(
            IUnitOfWork unitOfWork, 
            IOrderRepository orderRepository, 
            ICustomerRepository customerRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async Task Handle(CreateOrderCommand command)
        {
            var customer = await _customerRepository.GetByIdAsync(command.CustomerId);

            if (customer == null)
            {
                throw new InvalidOperationException("Customer not found");
            }
            
            if(command.Order.Items == null)
            {
                throw new InvalidOperationException("The order can not be placed without any items.");
            }

            command.Order.OrderDate = DateTime.Now;
            command.Order.TotalPrice = await CalculateTotalPrice(command.Order.Items);
            command.Order.CustomerId = customer.Id;

            await _orderRepository.Add(command.Order);
            await _unitOfWork.saveChanges();
        }

        private async Task<decimal> CalculateTotalPrice(List<Item> items)
        {
            decimal totalPrice = 0;
            foreach (var item in items)
            {
                var product = await _productRepository.GetProductAsync(item.ProductId);
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
