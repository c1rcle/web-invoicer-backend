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

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }

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

            builder.Entity<Invoice>(entity =>
            {
                entity.HasIndex(x => new { x.Number, x.UserId }).IsUnique();
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Invoices)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Invoice_User");

                entity.HasOne(e => e.Employee)
                    .WithMany(e => e.Invoices)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Invoice_Employee");

                entity.HasOne(e => e.Counterparty)
                    .WithMany(e => e.Invoices)
                    .HasForeignKey(e => e.CounterpartyId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Invoice_Counterparty");
            });

            builder.Entity<InvoiceItem>(entity =>
            {
                entity.HasOne(e => e.Invoice)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => x.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_InvoiceItem_Invoice");

                entity.HasOne(e => e.Product)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_InvoiceItem_Product");
            });
        }

        public async Task<TaskResult> SaveContextChanges(CancellationToken token)
        {
            return await SaveChangesAsync(token) > 0
                ? new TaskResult()
                : new TaskResult(new[] { "Data could not be processed" });
        }

        public async Task<TaskResult<T>> SaveContextChanges<T>(CancellationToken token, T payload)
        {
            return await SaveChangesAsync(token) > 0
                ? new TaskResult<T>(payload)
                : new TaskResult<T>(new[] { "Data could not be processed" });
        }
    }
}
