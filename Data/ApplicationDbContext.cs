using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkingWithDatabase.Model;

namespace WorkingWithDatabase.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
    }
}
