using Domain.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.CustomerCQRS.Commands.CreateCustomerCommand;
using Application.CustomerCQRS.Commands.DeleteCustomerCommand;
using Application.CustomerCQRS.Commands.UpdateCustomerCommand;
using Application.CustomerCQRS.Queries.GetCustomerQuery;
using Application.CustomerCQRS.Queries.GetCustomersQuery;
using Application.CustomerCQRS.Queries.GetOrdersByDate;
using Application.OrderCQRS.Queries.GetAllOrdersQuery;
using Application.OrderCQRS.Queries.GetOrderQuery;
using Application.OrderCQRS.Commands.DeleteCustomerOrdersCommand;

namespace WebApp.Controllers
{
    /// <summary>
    /// Customer Controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CreateCustomerCommandHandler _createCustomerCommandHandler;
        private readonly UpdateCustomerCommandHandler _updateCustomerCommandHandler;
        private readonly DeleteCustomerCommandHandler _deleteCustomerCommandHandler;
        private readonly GetCustomerQueryHandler _getCustomerQueryHandler;
        private readonly GetCustomersQueryHandler _getCustomersQueryHandler;
        private readonly GetOrdersByDateHandler _getOrdersByDateHandler;
        private readonly DeleteCustomerOrdersCommandHandler _deleteCustomerOrdersCommandHandler;
        
        /// <summary>
        /// Customer Controller Constructor.
        /// </summary>
        /// <param name="createCustomerCommandHandler"></param>
        /// <param name="updateCustomerCommandHandler"></param>
        /// <param name="deleteCustomerCommandHandler"></param>
        /// <param name="getCustomerQueryHandler"></param>
        /// <param name="getCustomersQueryHandler"></param>
        /// <param name="getOrdersByDateHandler"></param>
        /// <param name="deleteCustomerOrdersCommandHandler"></param>
        public CustomerController(
            CreateCustomerCommandHandler createCustomerCommandHandler,
            UpdateCustomerCommandHandler updateCustomerCommandHandler,
            DeleteCustomerCommandHandler deleteCustomerCommandHandler,
            GetCustomerQueryHandler getCustomerQueryHandler,
            GetCustomersQueryHandler getCustomersQueryHandler,
            GetOrdersByDateHandler getOrdersByDateHandler,
            DeleteCustomerOrdersCommandHandler deleteCustomerOrdersCommandHandler)
        {
            _createCustomerCommandHandler = createCustomerCommandHandler;
            _updateCustomerCommandHandler = updateCustomerCommandHandler;
            _deleteCustomerCommandHandler = deleteCustomerCommandHandler;
            _getCustomerQueryHandler = getCustomerQueryHandler;
            _getCustomersQueryHandler = getCustomersQueryHandler;
            _getOrdersByDateHandler = getOrdersByDateHandler;
            _deleteCustomerOrdersCommandHandler = deleteCustomerOrdersCommandHandler;
        }

        /// <summary>
        /// You can retrieve all the Customers.
        /// </summary>
        /// <returns> This endpoint returns a list of all Customers. </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var query = new GetCustomersQuery();
            var customers = await _getCustomersQueryHandler.Handle(query);

            if (customers == null)
            {
                return NotFound("The are no orders saved.");
            }
            return Ok(customers);
        }

        /// <summary>
        /// You can retrieve a Customer by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns> This endpoint returns a Customer. </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            //var customer = await _customerRepository.GetByIdAsync(id);
            var query = new GetCustomerQuery { CustomerId = id };
            var customer = await _getCustomerQueryHandler.Handle(query);
            
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        /// <summary>
        /// You can retrieve a Customers orders ordered by date. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns> This endpoint returns the list of orders of a Customer ordered by date. </returns>
        [HttpGet("orders/{id}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetCustomerOrders(int id)
        {
            var query = new GetOrdersByDateQuery { CustomerId = id };

            var orders = await _getOrdersByDateHandler.Handle(query);
            /*
            if (customer == null)
            {
                return NotFound();
            }
            await _customerRepository.GetCustomerOrdersByDateOrderAsync(id)
            */
            return Ok(orders);
        }

        /// <summary>
        /// You can create a Customer here.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns> This endpoint return the created Customer. </returns>
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            var command = new CreateCustomerCommand { Customer = customer };
            
            await _createCustomerCommandHandler.Handle(command);
            /*
            if (customer==null)
            {
                return BadRequest();
            }

            await _customerRepository.AddAsync(customer);
            await _unitOfWork.saveChanges();
            */
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        /// <summary>
        /// You can update a Customer.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customer"></param>
        /// <returns> This endpoint doesn't return anything. </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            if(id != customer.Id)
            {
                return BadRequest();
            }

            var query = new GetCustomerQuery {CustomerId = id };
            var databaseCustomer = await _getCustomerQueryHandler.Handle(query);
            
            if(databaseCustomer == null)
            {
                return NotFound("Not found");
            }

            databaseCustomer.FirstName = customer.FirstName;
            databaseCustomer.LastName = customer.LastName;
            databaseCustomer.Address = customer.Address;
            databaseCustomer.PostalCode = customer.PostalCode;

            var command = new UpdateCustomerCommand { CustomerId = id, Customer = databaseCustomer };
            try
            {
                await _updateCustomerCommandHandler.Handle(command);
            }
            catch(DbUpdateConcurrencyException)
            {
                return BadRequest("Database concurrency issue.");
            }
            
            return NoContent();
        }

        /// <summary>
        /// You can delete a Customer here. (It also deletes its orders.)
        /// </summary>
        /// <param name="id"></param>
        /// <returns> This endpoint doesn't return anything. </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var command_1 = new DeleteCustomerOrdersComannd { CustomerId = id };
            
            await _deleteCustomerOrdersCommandHandler.Handle(command_1);

            var command = new DeleteCustomerCommand { CustomerID = id };

            await _deleteCustomerCommandHandler.Handle(command);
            
            return NoContent();
        }
    }
}