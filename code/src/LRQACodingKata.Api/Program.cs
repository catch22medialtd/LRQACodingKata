using LRQACodingKata.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
builder.Configuration.AddApplicationSettings(builder.Environment, args);

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