using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextClass _dbContext;
        public OrderRepository(DbContextClass dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Delete(int id)
        {
            var entity = await _dbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);

            if (entity != null)
            {
                _dbContext.Orders.Remove(entity);
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _dbContext.Orders.Include(o=>o.Items).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetCustomerOrders(int id)
        {
            return await _dbContext.Orders.Include(o => o.Items).Where(o=>o.CustomerId==id).ToListAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _dbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o=>o.Id==id);
        }

        public void Update(Order order)
        {
            _dbContext.Entry(order).State = EntityState.Modified;
        }

        public async Task Add(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
        }
    }
}
