using Domain.Entities;

namespace Application.CustomerCQRS.Commands.UpdateCustomerCommand
{
    public class UpdateCustomerCommand
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
