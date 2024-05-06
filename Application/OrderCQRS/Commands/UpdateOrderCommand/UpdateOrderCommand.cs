using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderCQRS.Commands.UpdateOrderCommand
{
    public class UpdateOrderCommand
    {
        public int OrderID { get; set; }
        public Order Order { get; set; }
    }
}
