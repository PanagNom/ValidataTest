using Application.CustomerCQRS.Commands.DeleteCustomerCommand;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderCQRS.Commands.DeleteOrderCommand
{
    public class DeleteOrderCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        public DeleteOrderCommandHandler(
            IUnitOfWork unitOfWork,
            IOrderRepository orderRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
        }

        public async Task<int> Handle(DeleteOrderCommand query)
        {
            var existingOrder = await _orderRepository.GetOrderById(query.OrderID);

            if (existingOrder == null)
            {
                return 0;
            }

            await _orderRepository.Delete(query.OrderID);
            await _unitOfWork.saveChanges();

            return 1;
        }
    }
}
