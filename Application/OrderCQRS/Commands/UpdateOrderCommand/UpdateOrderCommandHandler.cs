using Application.CustomerCQRS.Commands.UpdateCustomerCommand;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderCQRS.Commands.UpdateOrderCommand
{
    public class UpdateOrderCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public UpdateOrderCommandHandler(
            IUnitOfWork unitOfWork,
            IOrderRepository orderRepository,
            IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task Handle(UpdateOrderCommand command)
        {
            var databaseOrder = await _orderRepository.GetOrderById(command.OrderID);

            if (databaseOrder == null)
            {
                return;
            }

            databaseOrder.CustomerId = command.Order.CustomerId;
            databaseOrder.Items = command.Order.Items;
            databaseOrder.TotalPrice = await CalculateTotalPrice(command.Order.Items);

            _orderRepository.Update(databaseOrder);

            try
            {
                await _unitOfWork.saveChanges();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        private async Task<decimal> CalculateTotalPrice(List<Item>? items)
        {
            decimal totalPrice = 0;

            if (items == null)
            {
                return 0;
            }

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
