using Microsoft.EntityFrameworkCore;

namespace customerChallenge.models
{
    public class CustomerContext:DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
