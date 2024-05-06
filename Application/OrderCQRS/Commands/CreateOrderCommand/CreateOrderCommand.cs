using Domain.Entities;

namespace Application.OrderCQRS.Commands.CreateOrderCommand
{
    public class CreateOrderCommand
    {
        public int CustomerId { get; set; }
        public required Order Order { get; set; }
    }
}
