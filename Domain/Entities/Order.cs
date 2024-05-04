using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice {  get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public List<Item>? Items { get; set; }

    }
}
