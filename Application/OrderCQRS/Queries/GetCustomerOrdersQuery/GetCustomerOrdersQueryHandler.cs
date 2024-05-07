using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderCQRS.Queries.GetCustomerOrdersQuery
{
    public class GetCustomerOrdersQueryHandler
    {
        private readonly IOrderRepository _orderRepository;

        public GetCustomerOrdersQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> Handle(GetCustomerOrdersQuery query)
        {
            return await _orderRepository.GetCustomerOrders(query.CustomerId);
        }
    }
}
