using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

namespace PayrollProcessor.Data.Persistence.Features.Employees
{
    public class EmployeesContext : DbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        /*
         * See https://docs.microsoft.com/en-us/ef/core/miscellaneous/nullable-reference-types
         */
        public DbSet<EmployeeRecord> Employees { get; set; } = null!;

        public EmployeesContext(DbContextOptions<EmployeesContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeesContext).Assembly);
    }

    public class EmployeeRecordEntityConfiguration : IEntityTypeConfiguration<EmployeeRecord>
    {
        public void Configure(EntityTypeBuilder<EmployeeRecord> builder)
        {
            builder
                .ToContainer("Employees")
                .HasDiscriminator(e => e.Type)
                .HasValue("EmployeeEntity");
            builder
                .HasPartitionKey(e => e.PartitionKey);
        }

    }
}
