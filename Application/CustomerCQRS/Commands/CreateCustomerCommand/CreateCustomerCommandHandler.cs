using Domain.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
//using Microsoft.AspNetCore.Mvc;

namespace Application.CustomerCQRS.Commands.CreateCustomerCommand
{
    public class CreateCustomerCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
        }

        public async Task Handle(CreateCustomerCommand command)
        {
            await _customerRepository.AddAsync(command.Customer);
            await _unitOfWork.saveChanges();
        }
    }
}
