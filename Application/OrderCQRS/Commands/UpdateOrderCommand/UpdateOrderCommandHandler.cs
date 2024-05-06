using Application.CustomerCQRS.Commands.UpdateCustomerCommand;
using Domain.Interfaces;
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
        public UpdateOrderCommandHandler(
            IUnitOfWork unitOfWork,
            IOrderRepository orderRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
        }

        public async Task Handle(UpdateOrderCommand query)
        {
            _orderRepository.Update(query.Order);

            try
            {
                await _unitOfWork.saveChanges();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }
        }
    }
}
