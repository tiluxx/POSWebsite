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

            modelBuilder.Entity<Staff>()
                .HasOne(staff => staff.BranchStore)
                .WithMany(branch => branch.Staff)
                .HasForeignKey(staff => staff.BranchStoreId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.CreationLocation)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.CreationLocationId);

            modelBuilder.Entity<ProductBranch>().HasKey(sc => new { sc.BranchId, sc.ProductId });

            modelBuilder.Entity<ProductBranch>()
                .HasOne(productBranch => productBranch.BranchStore)
                .WithMany(branchStore => branchStore.ProductBranches)
                .HasForeignKey(productBranch => productBranch.BranchId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<ProductBranch>()
                .HasOne(productBranch => productBranch.Product)
                .WithMany(product => product.ProductBranches)
                .HasForeignKey(productBranch => productBranch.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderDetail>().HasKey(sc => new { sc.OrderId, sc.ProductId });

            modelBuilder.Entity<OrderDetail>()
                .HasOne(orderDetail => orderDetail.Order)
                .WithMany(order => order.OrderDetails)
                .HasForeignKey(orderDetail => orderDetail.OrderId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<OrderDetail>()
                .HasOne(orderDetail => orderDetail.Product)
                .WithMany(product => product.OrderDetails)
                .HasForeignKey(orderDetail => orderDetail.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Account> Account { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<BranchStore> BranchStore { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductBranch> ProductBranch { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Staff> Staff { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
    }
}
