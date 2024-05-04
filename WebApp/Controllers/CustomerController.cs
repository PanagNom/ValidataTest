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

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        //private readonly IUnitOfWork _unitOfWork;
        //private readonly ICustomerRepository _customerRepository;
        private readonly CreateCustomerCommandHandler _createCustomerCommandHandler;
        private readonly UpdateCustomerCommandHandler _updateCustomerCommandHandler;
        private readonly DeleteCustomerCommandHandler _deleteCustomerCommandHandler;
        private readonly GetCustomerQueryHandler _getCustomerQueryHandler;
        private readonly GetCustomersQueryHandler _getCustomersQueryHandler;
        private readonly GetOrdersByDateHandler _getOrdersByDateHandler;
        public CustomerController(
            //IUnitOfWork unitOfWork, 
            //ICustomerRepository customerRepository,
            CreateCustomerCommandHandler createCustomerCommandHandler,
            UpdateCustomerCommandHandler updateCustomerCommandHandler,
            DeleteCustomerCommandHandler deleteCustomerCommandHandler,
            GetCustomerQueryHandler getCustomerQueryHandler,
            GetCustomersQueryHandler getCustomersQueryHandler,
            GetOrdersByDateHandler getOrdersByDateHandler)
        {
            //_unitOfWork = unitOfWork;
            //_customerRepository = customerRepository;
            _createCustomerCommandHandler = createCustomerCommandHandler;
            _updateCustomerCommandHandler = updateCustomerCommandHandler;
            _deleteCustomerCommandHandler = deleteCustomerCommandHandler;
            _getCustomerQueryHandler = getCustomerQueryHandler;
            _getCustomersQueryHandler = getCustomersQueryHandler;
            _getOrdersByDateHandler = getOrdersByDateHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            //var customers = await _customerRepository.GetAllAsync();
            var query = new GetCustomersQuery();
            var customers = await _getCustomersQueryHandler.Handle(query);
            
            return Ok(customers);
        }
       
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
                return BadRequest("Database concurency issue.");
            }
            /*
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _customerRepository.Update(customer);

            try
            {
                await _unitOfWork.saveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            */
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var command = new DeleteCustomerCommand { CustomerID = id };

            await _deleteCustomerCommandHandler.Handle(command);
            /*
            var existingCustomer = await _customerRepository.GetByIdAsync(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            await _customerRepository.DeleteAsync(id);
            await _unitOfWork.saveChanges();
            */
            return NoContent();
        }
    }
}