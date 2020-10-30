using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebInvoicer.Core.Models;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Counterparty> Counterparties { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Counterparty>(entity =>
            {
                entity.HasIndex(x => x.Nip).IsUnique();
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Counterparties)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Counterparty_User");
            });

            builder.Entity<Employee>(entity =>
            {
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Employees)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Employee_User");
            });

            builder.Entity<Product>(entity =>
            {
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Product_User");
            });
        }

        public async Task<TaskResult> SaveContextChanges(CancellationToken token)
        {
            return await SaveChangesAsync(token) > 0
                ? new TaskResult()
                : new TaskResult(new[] { "Data could not be processed" });
        }
    }
}
