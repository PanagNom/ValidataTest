﻿using Domain.Interfaces;

namespace Application.CustomerCQRS.Commands.DeleteCustomerCommand
{
    public class DeleteCustomerCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;
        public DeleteCustomerCommandHandler(
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
        }

        public async Task<int> Handle(DeleteCustomerCommand command)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(command.CustomerID);

            if (existingCustomer == null)
            {
                return 0;
            }

            await _customerRepository.DeleteAsync(command.CustomerID);
            await _unitOfWork.saveChanges();

            return 1;
        }
    }
}
