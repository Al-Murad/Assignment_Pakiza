using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required, StringLength(100),Display(Name ="Customer Name")]
        public string CustomerName { get; set; } = default!;
        [Required, StringLength(30)]
        public string Phone { get; set; } = default!;
        [Required, StringLength(100)]
        public string Address { get; set; } = default!;
        public virtual ICollection<Order> Orders { get; set; }=new List<Order>();
    }
    public class Product
    {
        public int ProductId { get; set; }
        [Required, StringLength(50),Display(Name ="Product Name")]
        public string ProductName { get; set; } = default!;
        [Required, Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; } = new List<OrderItem>();

    }
    public class Order
    {
        public int OrderId { get; set; }
        [Required, ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [Required]
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();


    }
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required, ForeignKey("Order")]
        public int OrderId { get; set; }
        [Required, ForeignKey("Product")]
        public int ProductId { get; set; }
        [Required]
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
    public class AssignmentDbContext : DbContext
    {
        public AssignmentDbContext(DbContextOptions<AssignmentDbContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                new Customer {CustomerId=1, CustomerName="Kamal",Phone="0198653738",Address="khulna"},
                new Customer { CustomerId = 2, CustomerName = "Jamal", Phone = "0198653738", Address = "Dhanmondi" },
               new Customer { CustomerId = 3, CustomerName = "Hafiz", Phone = "0198653738", Address = "Dhanmondi" }

                );
            modelBuilder.Entity<Product>().HasData(
               new Product { ProductId=1,ProductName="Computer",UnitPrice=90000M},
               new Product {ProductId=2, ProductName = "Keyboard", UnitPrice = 500M },
               new Product { ProductId = 3, ProductName = "mouse", UnitPrice = 500M }

               );
            modelBuilder.Entity<Order>().HasData(
            new Order {OrderId=1,CustomerId=1 },
            new Order { OrderId=2,CustomerId=2},
            new Order { OrderId = 3, CustomerId = 3 }
            );
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { OrderItemId=1,Quantity=20,UnitPrice=10,OrderId=1,ProductId=1},
                new OrderItem { OrderItemId = 2, Quantity = 10, UnitPrice=30,OrderId = 2, ProductId = 2 },
                new OrderItem { OrderItemId = 3, Quantity = 10, UnitPrice = 30, OrderId = 2, ProductId = 2 }
                );
        }
    }
   
}
