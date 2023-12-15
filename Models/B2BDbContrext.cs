using Microsoft.EntityFrameworkCore;

namespace POSWebsite.Models
{
    public class B2BDbContrext : DbContext
    {
        public B2BDbContrext(DbContextOptions<B2BDbContrext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>().Property(account => account.Roles)
                .HasConversion(
                    role => string.Join(",", role),
                    role => role.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList()
                ).HasColumnName("Roles");

            modelBuilder.Entity<OrderDetail>().HasKey(sc => new { sc.OrderId, sc.ProductId });

            modelBuilder.Entity<OrderDetail>()
                .HasOne<Order>(orderDetail => orderDetail.Order)
                .WithMany(order => order.OrderDetails)
                .HasForeignKey(orderDetail => orderDetail.OrderId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<OrderDetail>()
                .HasOne<Product>(orderDetail => orderDetail.Product)
                .WithMany(product => product.OrderDetails)
                .HasForeignKey(orderDetail => orderDetail.OrderId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Account> Account { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Staff> Staff { get; set; }
    }
}
