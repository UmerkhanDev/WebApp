using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.DataContext
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Payment> payments { get; set; }
        public DbSet<Status> status { get; set; }
        public DbSet<PaymentStatus> paymentStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentStatus>()
                .HasKey(bc => new { bc.PaymentId, bc.StatusId });
            modelBuilder.Entity<PaymentStatus>()
                .HasOne(bc => bc.payment)
                .WithMany(b => b.paymentStatus)
                .HasForeignKey(bc => bc.StatusId);
            modelBuilder.Entity<PaymentStatus>()
                .HasOne(bc => bc.status)
                .WithMany(c => c.paymentStatus)
                .HasForeignKey(bc => bc.PaymentId);
        }
    }
}
