using FluentValidation;
using LRQACodingKata.Application.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LRQACodingKata.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration) => services
            .AddValidatorsFromAssembly(typeof(ApplicationAssemblyMarker).Assembly)
            .AddOptions(configuration);

        private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseOptions>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException(
                        "DefaultConnection connection string is not configured. " +
                        "Please ensure 'ConnectionStrings:DefaultConnection' is defined in your configuration.");
                }

                options.DefaultConnection = connectionString;
            });

            return services;
        }
    }
}