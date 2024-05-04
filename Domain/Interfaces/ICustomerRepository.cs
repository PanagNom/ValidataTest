using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int id);
        Task AddAsync(Customer customer);
        Task Update(Customer customer);
        Task DeleteAsync(int id);
        Task<IEnumerable<Order>> GetCustomerOrdersByDateOrderAsync(int id);
    }
}
