using LRQACodingKata.Api.Extensions;
using LRQACodingKata.Api.Filters;
using LRQACodingKata.Application;
using LRQACodingKata.Application.Behaviours;
using LRQACodingKata.Infrastructure;
using MediatR;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Load configuration
        builder.Configuration.AddApplicationSettings(builder.Environment, args);

        // Add Application and Infrastructure services
        builder.Services
            .AddApplication(builder.Configuration)
            .AddInfrastructure(builder.Configuration);

        // MediatR
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        });

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<GlobalExceptionFilter>();
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
    }
}