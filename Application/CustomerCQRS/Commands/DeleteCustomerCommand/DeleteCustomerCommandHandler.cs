using Domain.Interfaces;

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

        public async Task Handle(DeleteCustomerCommand query)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(query.CustomerID);

            if (existingCustomer != null)
            {
                await _customerRepository.DeleteAsync(query.CustomerID);
                await _unitOfWork.saveChanges();
            }
        }
    }
}
