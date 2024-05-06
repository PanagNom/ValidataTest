using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderCQRS.Commands.DeleteOrderCommand
{
    public class DeleteOrderCommand
    {
        public int OrderID { get; set; }
    }
}
