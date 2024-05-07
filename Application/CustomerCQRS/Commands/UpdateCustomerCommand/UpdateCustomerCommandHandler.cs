using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.CustomerCQRS.Commands.UpdateCustomerCommand
{
    public class UpdateCustomerCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;
        public UpdateCustomerCommandHandler(
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
        }

        public async Task Handle(UpdateCustomerCommand command)
        {
            var databaseCustomer = await _customerRepository.GetByIdAsync(command.CustomerId);

            if (databaseCustomer == null)
            {
                return ;
            }

            databaseCustomer.FirstName = command.Customer.FirstName;
            databaseCustomer.LastName = command.Customer.LastName;
            databaseCustomer.Address = command.Customer.Address;
            databaseCustomer.PostalCode = command.Customer.PostalCode;

            _customerRepository.Update(databaseCustomer);

            try
            {
                await _unitOfWork.saveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
    }
}
