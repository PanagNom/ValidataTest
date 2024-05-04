using Domain.Interfaces;
using Domain.Entities;

namespace Application.OrderCQRS.Queries.GetAllOrdersQuery
{
    public class GetAllOrdersQueryHandler
    {
        private readonly IOrderRepository _orderRepository;

        public GetAllOrdersQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> Handle(GetAllOrdersQuery query)
        {
            return await _orderRepository.GetAllOrders();
        }
    }
}
