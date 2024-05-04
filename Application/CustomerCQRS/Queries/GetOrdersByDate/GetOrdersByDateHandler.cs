using Domain.Interfaces;
using Domain.Entities;

namespace Application.CustomerCQRS.Queries.GetOrdersByDate
{
    public class GetOrdersByDateHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public GetOrdersByDateHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Order>> Handle(GetOrdersByDateQuery query)
        {
            return await _customerRepository.GetCustomerOrdersByDateOrderAsync(query.CustomerId);
        }
    }
}
