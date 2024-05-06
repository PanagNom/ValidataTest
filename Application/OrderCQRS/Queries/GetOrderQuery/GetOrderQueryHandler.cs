using Domain.Interfaces;
using Domain.Entities;

namespace Application.OrderCQRS.Queries.GetOrderQuery
{
    public class GetOrderQueryHandler
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order?> Handle(GetOrderQuery query)
        {
            return await _orderRepository.GetOrderById(query.OrderId);
        }
    }
}
