using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Employees> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Accounts Configuration
            modelBuilder.Entity<Accounts>()
                .Property(u => u.AccountID)
                .ValueGeneratedOnAdd();

            // Departments Configuration
            modelBuilder.Entity<Departments>()
                .Property(d => d.DepartmentID)
                .ValueGeneratedOnAdd();

            // Employees Configuration
            modelBuilder.Entity<Employees>()
                .Property(e => e.EmployeeID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Employees>()
                .HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
