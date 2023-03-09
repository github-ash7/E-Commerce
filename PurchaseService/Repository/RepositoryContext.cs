using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) { }

        // Creates table for Cart, Order, OrderItem, WishList, WishListItem

        public DbSet<Cart> Cart { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<OrderItem> OrderItem { get; set; }

        public DbSet<WishList> WishList { get; set; }

        public DbSet<WishListItem> WishListItem { get; set; }

    }
}
