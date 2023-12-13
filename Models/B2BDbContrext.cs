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
        }

        public DbSet<Staff> Staff { get; set; }
        public DbSet<Account> Account { get; set; }
    }
}
