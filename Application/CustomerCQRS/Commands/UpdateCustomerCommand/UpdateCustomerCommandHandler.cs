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
            await _customerRepository.Update(query.Customer);

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
