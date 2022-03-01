using Microsoft.AspNetCore.Mvc;
using ManagementApi.Models;
using ManagementApi.Services;

namespace ManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<InvoiceController> _logger;

    private readonly IInvoice _invoiceService;

    public InvoiceController(ILogger<InvoiceController> logger, IInvoice invoiceService)
    {
        _logger = logger;
        _invoiceService = invoiceService;
    }

    [HttpGet(Name = "GetCustomer")]
    public IEnumerable<Customer> Get()
    {
        // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        // {
        //     Date = DateTime.Now.AddDays(index),
        //     TemperatureC = Random.Shared.Next(-20, 55),
        //     Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        // })
        // .ToArray();
        return _invoiceService.GetAllCustomer();
    }

    [HttpGet("CustomerID/{customerId}")]
    public double GetInvoiceByCustomer(int customerId) 
    {
        return _invoiceService.CalculateOrderForCustomer(customerId);
    }
}
