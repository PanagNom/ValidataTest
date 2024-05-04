using Domain.Entities;

namespace Application.CustomerCQRS.Commands.CreateCustomerCommand
{
    public class CreateCustomerCommand
    {
        public Customer Customer { get; set; }
    }
}
