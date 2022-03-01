using Microsoft.EntityFrameworkCore;  
using ManagementApi.Models;


namespace ManagementApi.Configuration {
    public class PostgreSqlContext: DbContext  
    {  
        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options)  
        {  
        } 

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //     => optionsBuilder.UseNpgsql("Host=127.0.0.1:55432;Database=management;Username=postgres;Password=password"); 
  
        public DbSet<Customer> Customer { get; set; }  

        public DbSet<Order> Order{ get; set; }

        public DbSet<Package> Package { get; set; }

        public DbSet<Discount> Discount { get; set; }
  
        protected override void OnModelCreating(ModelBuilder modelBuilder)  
        {  
            // base.OnModelCreating(builder);  


            // modelBuilder.Entity<Customer>()
            //     .HasMany(b => b.Orders)
            //     .WithOne();

            modelBuilder.Entity<Order>()
                .HasOne(order => order.Customer)
                .WithMany(Customer => Customer.Orders);
            // modelBuilder.Entity<Customer>()
            //     .HasOne(p => p.Customer)
            //     .WithMany(b => b.Orders);

            modelBuilder.Entity<Customer>()
                .HasMany(customer => customer.Discount)
                .WithMany(discount => discount.Customer)
                .UsingEntity(entity => entity.ToTable("promotion"));

             modelBuilder.Entity<Discount>()
                .Property(discount => discount.Configuration)
                .HasColumnType("jsonb");
        }  
  
        public override int SaveChanges()  
        {  
            ChangeTracker.DetectChanges();  
            return base.SaveChanges();  
        }  
    }  
}