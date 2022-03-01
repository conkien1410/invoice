using Microsoft.Extensions.Configuration;
using ManagementApi.Services;
using ManagementApi.Configuration;
using Microsoft.EntityFrameworkCore;  



var builder = WebApplication.CreateBuilder(args);


// Connect to PostgreSQL Database
var connectionString = builder.Configuration.GetConnectionString("ManagementContext");
builder.Services.AddDbContext<PostgreSqlContext>(options =>
    options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddDbContext<PostgreSqlContext>(options =>
//             options.LogTo(new Action<string>()));

//  services.AddTransient<ITransientOperation, DefaultOperation>()
//             .AddScoped<IScopedOperation, DefaultOperation>()
//             .AddSingleton<ISingletonOperation, DefaultOperation>()
//             .AddTransient<OperationLogger>())

// builder.Services.AddSingleton<ICustomer, CustomerService>();
builder.Services.AddScoped<IInvoice, InvoiceService>();



var app = builder.Build();
//... rest of the code omitted for brevitybuilder.Services.AddDbContext<PostgreSqlContext>(dbOptions);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
