using Application.OrderCQRS.Commands.DeleteOrderCommand;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.OrderCQRS.Commands.DeleteCustomerOrdersCommand
{
    public class DeleteCustomerOrdersCommandHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCustomerOrdersCommandHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteCustomerOrdersComannd command)
        {
            var orders = await _orderRepository.GetCustomerOrders(command.CustomerId);

            foreach (var order in orders)
            {
                var existingOrder = await _orderRepository.GetOrderById(order.Id);

                if (existingOrder == null)
                {
                    return ;
                }

                await _orderRepository.Delete(order.Id);      
            }

            await _unitOfWork.saveChanges();        
        }
    }
}
