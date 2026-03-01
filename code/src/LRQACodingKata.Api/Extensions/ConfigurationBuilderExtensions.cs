namespace LRQACodingKata.Api.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddApplicationSettings(this IConfigurationBuilder config, IHostEnvironment env, string[]? args = null)
        {
            config.SetBasePath(env.ContentRootPath);
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            config.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);
            config.AddEnvironmentVariables();

            if (args is not null)
            {
                config.AddCommandLine(args);
            }

            return config;
        }
    }
}