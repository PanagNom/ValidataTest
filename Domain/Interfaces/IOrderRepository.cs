using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderById(int id);
        Task<IEnumerable<Order>> GetAllOrders();
        Task<IEnumerable<Order>> GetCustomerOrders(int id);
        Task Delete(int id);
        void Update(Order order);
        Task Add(Order order);
    }
}
