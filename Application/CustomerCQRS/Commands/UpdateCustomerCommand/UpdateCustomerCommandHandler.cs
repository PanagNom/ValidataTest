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

        public async Task Handle(UpdateCustomerCommand query)
        {
            var databaseCustomer = await _customerRepository.GetByIdAsync(query.CustomerId);

            if (databaseCustomer == null)
            {
                return ;
            }

            databaseCustomer.FirstName = query.Customer.FirstName;
            databaseCustomer.LastName = query.Customer.LastName;
            databaseCustomer.Address = query.Customer.Address;
            databaseCustomer.PostalCode = query.Customer.PostalCode;

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
