using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) { }

        //Creates table for User, User Secret, Address, Payment

        public DbSet<User> User { get; set; }

        public DbSet<UserSecret> UserSecret { get; set; }

        public DbSet<Address> Address { get; set; }

        public DbSet<Payment> Payment { get; set; }

    }
}
