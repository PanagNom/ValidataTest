using Domain.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        protected readonly DbContextClass _dbContext;
        private readonly IOrderRepository _orderRepository;
        public CustomerRepository(DbContextClass dbContext, IOrderRepository orderRepository) 
        {
            _dbContext = dbContext;
            _orderRepository = orderRepository;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _dbContext.Customers.Where(c => c.Id == id).FirstOrDefaultAsync(); ;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        public async Task AddAsync(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity != null)
            {
                _dbContext.Customers.Remove(entity);
            }
        }

        public async Task<IEnumerable<Order>> GetCustomerOrdersByDateOrderAsync(int id)
        {
            IEnumerable<Order> myCustomerOrders = await _orderRepository.GetCustomerOrders(id);

            return myCustomerOrders.OrderBy(o => o.OrderDate);
        }

        public void Update(Customer customer)
        {
            _dbContext.Entry(customer).State = EntityState.Modified;
        }
    }
}
