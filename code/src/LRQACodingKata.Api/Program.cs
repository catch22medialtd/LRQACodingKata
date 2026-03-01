using LRQACodingKata.Api.Extensions;
using LRQACodingKata.Application.Options;
using LRQACodingKata.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
builder.Configuration.AddApplicationSettings(builder.Environment, args);

// Configure DatabaseOptions with validation
builder.Services.Configure<DatabaseOptions>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException(
            "DefaultConnection connection string is not configured. " +
            "Please ensure 'ConnectionStrings:DefaultConnection' is defined in your configuration.");
    }
    
    options.DefaultConnection = connectionString;
});

// Add Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers(options =>
{
    //options.Filters.Add<GlobalExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();