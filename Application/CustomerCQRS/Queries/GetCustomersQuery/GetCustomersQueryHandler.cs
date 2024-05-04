using Domain.Interfaces;
using Domain.Entities;

namespace Application.CustomerCQRS.Queries.GetCustomersQuery
{
    public class GetCustomersQueryHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomersQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> Handle(GetCustomersQuery query)
        {
            return await _customerRepository.GetAllAsync();
        }
    }
}
