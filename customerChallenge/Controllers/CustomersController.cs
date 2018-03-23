using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

            if (_context.Customers.Count() == 0)
            {
                _context.Customers.Add(new Customer { name = "nomeTeste", email = "email@teste.com" });
                _context.SaveChanges();
            }

        }

        // GET: api/Customers
        [HttpGet]
        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers;
        }

        // GET: api/Customers/5
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

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] int id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
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
            } else {
                oCustomer = new Customer();
                oCustomer.email = customer.email;
                oCustomer.name = customer.name;
                _context.Customers.Add(customer);                
            }

            _context.SaveChanges();

            oCustomer = _context.Customers.SingleOrDefault(c => c.email == customer.email);

            return CreatedAtAction("GetCustomer", new { id = oCustomer.Id }, oCustomer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
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

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(c => c.Id == id);
        }

        private bool CustomerExists(string email)
        {
            return _context.Customers.Any(c => c.email == email);
        }

        private Customer getCustomerByEmail(string email)
        {
            var customer =   _context.Customers.SingleOrDefault(c => c.email == email);
            return customer;
        }        

    }

}