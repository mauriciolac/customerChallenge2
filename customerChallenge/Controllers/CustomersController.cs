
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using customerChallenge.models;

namespace customerChallenge.Controllers
{
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : Controller
    {
        private readonly CustomerContext _context;

        public CustomersController(CustomerContext context)
        {
            _context = context;
            
        }

        [HttpGet]
        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.SingleOrDefaultAsync(m => m.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }        

        [HttpPost]
        public IActionResult PostCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            Customer oCustomer = _context.Customers.SingleOrDefault(c => c.email == customer.email);
            if (oCustomer != null)
            {
                oCustomer.email = customer.email;
                oCustomer.name = customer.name;
            } else
            {
                oCustomer = new Customer();
                oCustomer.email = customer.email;
                oCustomer.name = customer.name;
                _context.Customers.Add(customer);                
            }

            _context.SaveChanges();

            oCustomer = _context.Customers.SingleOrDefault(c => c.email == customer.email);

            return CreatedAtAction("GetCustomer", new { id = oCustomer.Id }, oCustomer);
        }       

    }

}