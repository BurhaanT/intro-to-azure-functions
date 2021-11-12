using System;
using AzureIntro;
using AzureIntro.Payments;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(Startup))]
namespace AzureIntro
{
    public class Startup : FunctionsStartup
    {
        public IConfiguration _configuration { get; set; } = default!;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var context = builder.GetContext();
            _configuration = context.Configuration ?? default;

            _ = builder.Services
                //.AddLogging(context, _configuration) //See gist for code :)
                .Configure<CustomSettings>(_configuration.GetSection(nameof(CustomSettings)))
                .AddSingleton<IPaymentQueueProcessor, PaymentQueueProcessor>()
                .AddSingleton(provider => provider.GetRequiredService<IOptions<CustomSettings>>().Value);
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            //_azureCredential = new DefaultAzureCredential();

            var currentDirectory = builder.GetContext().ApplicationRootPath;
            //var tmpConfig = new ConfigurationBuilder()
            //    .SetBasePath(currentDirectory)
            //    .AddJsonFile("local.appsettings.json", optional: true)
            //    .AddJsonFile("appsettings.json", optional: true)
            //    .AddEnvironmentVariables()
            //    .Build();
            //var environmentName = tmpConfig["Environment"] ?? "Development";
            
            //TODO: Set managed identity of Function app to have access to key vault
            //var appConfigurationName = Environment.GetEnvironmentVariable("APP_CONFIG_NAME");
            //var isAppConfigurationEnabled = !string.IsNullOrEmpty(appConfigurationName);
            var appConfigurationConnectionString = Environment.GetEnvironmentVariable("APP_CONFIG_CONNECTION_STRING");
            var isAppConfigurationEnabled = !string.IsNullOrEmpty(appConfigurationConnectionString);

            builder.ConfigurationBuilder
                .SetBasePath(currentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                // .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
                //.If(isAppConfigurationEnabled, b =>
                //{
                //    b.AddAzureAppConfiguration(
                //        p => {
                //            p.Connect(appConfigurationConnectionString);
                //            //TODO: Set managed identity of Function app to have access to key vault
                //            //p.Connect(new Uri($"https://{appConfigurationName}.azconfig.io"), _azureCredential);
                //            //    .ConfigureKeyVault(kv => {
                //            //        kv.SetCredential(_azureCredential);
                //            //    });
                //        },
                //        optional: false);
                //});
        }
    }

    public class CustomSettings
    {
        public string MySetting { get; set; } = default;
    }
}