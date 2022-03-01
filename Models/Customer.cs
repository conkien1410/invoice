namespace ManagementApi.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public List<Order> Orders { get; set; }

        public ICollection<Discount> Discount { get; set; }
    }


    public class Order
    {
        public int Id { get; set; }

        public Customer Customer { get; set; }

        public Package Package {get; set;}

        public int Number {get; set;}

        public DateOnly Date {get; set;}

        public String? Status {get; set;}

    }
}