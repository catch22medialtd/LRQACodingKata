using LRQACodingKata.Application.Options;
using LRQACodingKata.Core.Repositories;
using LRQACodingKata.Infrastructure.Data.Context;
using LRQACodingKata.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LRQACodingKata.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
            => services
            .AddDatabase()
            .AddRepositories();

        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var databaseOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;

                options.UseNpgsql(databaseOptions.DefaultConnection);
                options.UseSnakeCaseNamingConvention();
            });

            services.AddScoped<DbContext>(sp => sp.GetRequiredService<AppDbContext>());

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));

            return services;
        }
    }
}