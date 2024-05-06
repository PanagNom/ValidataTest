using Application.CustomerCQRS.Commands.CreateCustomerCommand;
using Application.CustomerCQRS.Commands.DeleteCustomerCommand;
using Application.CustomerCQRS.Commands.UpdateCustomerCommand;
using Application.CustomerCQRS.Queries.GetCustomersQuery;
using Application.CustomerCQRS.Queries.GetOrdersByDate;
using Application.OrderCQRS.Commands.CreateOrderCommand;
using Application.OrderCQRS.Queries.GetAllOrdersQuery;
using Application.OrderCQRS.Queries.GetOrderQuery;
using Application.ProductCQRS.Queries.GetProductQuery;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.CustomerCQRS.Queries.GetCustomerQuery;
using Application.OrderCQRS.Commands.UpdateOrderCommand;

namespace Application
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddTransient<CreateOrderCommandHandler>()
                .AddTransient<CreateCustomerCommandHandler>()
                .AddTransient<UpdateCustomerCommandHandler>()
                .AddTransient<DeleteCustomerCommandHandler>()
                .AddTransient<GetOrderQueryHandler>()
                .AddTransient<GetAllOrdersQueryHandler>()
                .AddTransient<GetCustomerQueryHandler>()
                .AddTransient<GetCustomersQueryHandler>()
                .AddTransient<GetProductQueryHandler>()
                .AddTransient<GetOrdersByDateHandler>()
                .AddTransient<UpdateOrderCommandHandler>();
            
            return services;
        }
    }
}
