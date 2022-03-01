using System.Text.Json;

namespace ManagementApi.Models
{
    public class Package
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Price { get; set; }

        public int Number { get; set; }
    }

    public class Discount
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // public JsonDocument Rule { get; set; }
        // [Column(TypeName = "jsonb")]

        public Configuration Configuration { get; set; }

        public ICollection<Customer> Customer { get; set; }

    }

    public class Configuration {
        public Rule condition { get; set; }
        public JsonElement assignment { get; set; }
    }
    

    public class Rule
    {
        public string field { get; set; }
        public string oper { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string join { get; set; }
        public List<Rule> rules { get; set; }
    }
    public class Condition
    {

        public string join { get; set; }
        public List<Rule> rules { get; set; }

    }


}