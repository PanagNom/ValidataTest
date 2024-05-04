using Domain.Entities;

namespace Application.OrderCQRS.Commands.CreateOrderCommand
{
    public class CreateOrderCommand
    {
        public int CustomerId { get; set; }
        public Order Order { get; set; }
    }
}
