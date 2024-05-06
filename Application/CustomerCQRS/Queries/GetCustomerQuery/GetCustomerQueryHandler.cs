using Domain.Interfaces;
using Domain.Entities;

namespace Application.CustomerCQRS.Queries.GetCustomerQuery
{
    public class GetCustomerQueryHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer?> Handle(GetCustomerQuery query)
        {
            return await _customerRepository.GetByIdAsync(query.CustomerId);
        }
    }
}
