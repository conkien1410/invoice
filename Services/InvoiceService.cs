using ManagementApi.Models;
using ManagementApi.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace ManagementApi.Services
{

    public interface IInvoice {
        List<Customer> GetAllCustomer();

        double CalculateOrderForCustomer(int customerId);

    }
    public class InvoiceService : IInvoice
    {
        private readonly PostgreSqlContext _dbContext;
  
        public InvoiceService(PostgreSqlContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public List<Customer> GetAllCustomer() 
        {
        
            List<Customer> customerList = new List<Customer>();
            var customers = _dbContext.Customer;
            foreach(Customer customer in customers) {
                customerList.Add(customer);
            }
            return customerList;

            // // Console.WriteLine($"MyDependency.WriteMessage Message: {message}");
            // return new List<Customer>();
        }

        public double CalculateOrderForCustomer(int customerId) 
        {
            // _dbContext.Order.Join<Customer>.


            var customer = _dbContext.Customer
                                .Where(customer => customer.Id == customerId)
                                .Include(customer => customer.Orders)
                                .First();  

            List<int> orderIds = new List<int>();
            foreach(var order in customer.Orders)
            {
                orderIds.Add(order.Id);
            }
            var orders = _dbContext.Order
                                .Where(order => orderIds.Contains(order.Id))
                                .Include(order => order.Package)
                                // .Include(order => order.Customer)
                                .ToList();

            var discounts = _dbContext.Discount
                                .Where(discount => discount.Customer.Contains(customer))
                                .ToList();

            Parser conditonParser = new Parser();
            double total = 0;

            foreach(var order in orders) 
            {
                Dictionary<string, object> orderContext = new Dictionary<string, object>();
                orderContext.Add("package.name", order.Package.Name);
                orderContext.Add("order.number", order.Number);
                orderContext.Add("order.date", order.Date);
                orderContext.Add("package.price", order.Package.Price);
                orderContext.Add("package.number", order.Package.Number);
                
                double orderTotal = 0;

                foreach(var discount in discounts) 
                {
                    var conditionTest = discount.Configuration.condition;
                    Expression conditionExpression = conditonParser.BuildCondition(discount.Configuration.condition, orderContext);
                    Console.WriteLine(conditionExpression.ToString());

                    var conditionResult = Expression.Lambda<Func<bool>>(conditionExpression).Compile()();
                    if(conditionResult) 
                    {
                        var assignment = discount.Configuration.assignment;
                        string packagePriceAssignment = assignment.GetProperty("package.price").GetString();
                        // var scriptOptions = ScriptOptions.Default;

                        // var options =  scriptOptions.AddReferences(typeof(int).Assembly);
                        // Expression<int, int> packagePriceExpression = CSharpScript.Create<Expression<int, int>>(packagePriceAssignment, options);
                        Expression assignmentExpression = conditonParser.BuildAssignment(0, packagePriceAssignment.Split(" ").ToList(), orderContext);
                        double newPackagePrice = Expression.Lambda<Func<double>>(assignmentExpression).Compile()();
                        orderTotal = newPackagePrice * order.Package.Number * order.Number;
                        Console.WriteLine(assignmentExpression.ToString());
                    }
                    else 
                    {
                         orderTotal = order.Package.Price * order.Package.Number * order.Number;
                    }
                }
                total += orderTotal;


            }


            return total;
        }
    }
}