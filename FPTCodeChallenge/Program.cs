// See https://aka.ms/new-console-template for more information
using AdviseService;
using IAdviseService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Polly;
using Polly.Extensions;
using Polly.Extensions.Http;
using Polly.Registry;
using Polly.Retry;
using WeatherService;
using WeatherServiceInterface;

public class Program
{
    public static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        // Add console log provider
        builder.Logging.AddConsole();

        builder.Environment.ContentRootPath = Directory.GetCurrentDirectory();
        builder.Configuration.AddJsonFile("appsettings.json", optional: false);
        builder.Configuration.AddEnvironmentVariables(prefix: "PREFIX_");
        builder.Configuration.AddCommandLine(args);

        builder.Services.AddSingleton<IWeatherForecast, WeatherForecast>(
            (IServiceProvider provider) => new WeatherForecast(provider, builder.Configuration)
        );
        builder.Services.AddSingleton<IAdviceService, AdviceService>();

        // Register resilence pipeline
        RegisterResilencePipeline(builder.Services);

        using IHost host = builder.Build();

        await ShowAdvice(host.Services);

        await host.RunAsync();
    }

    public static async Task ShowAdvice(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        // For log demonstration only
        logger.LogWarning("Start program (log demo) .......");

        Console.Write("Please enter your zip code: ");
        var zipcode = Console.ReadLine();
        //var zipcode = "L5G";

        var adviceService = serviceProvider.GetService<IAdviceService>();
        var advice = await adviceService.GetAdvice(zipcode);

        var goOut = advice.GoOutside ? "Yes" : "No";
        var wearSunscreen = advice.WearSunscreen ? "Yes" : "No";
        var flyKite = advice.FlyKite ? "Yes" : "No";

        Console.WriteLine("========================================");
        Console.WriteLine($"Should I go outside? {goOut} (Precip: {advice.Precip})");
        Console.WriteLine($"Should I wear sunscreen? {wearSunscreen} (UvIndex: {advice.UvIndex})");
        Console.WriteLine($"Can I fly my kite? {flyKite} (WindSpeed: {advice.WindSpeed})");
        Console.WriteLine("========================================\n\n\n");
    }

    static void RegisterResilencePipeline(IServiceCollection services)
    {
        // Retry with back-off
        // Define a resilience pipeline with the name "weather-pipeline"
        services.AddResiliencePipeline("weather-pipeline", builder =>
        {
            var retryOptions = new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>(),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,  // Adds a random factor to the delay
                MaxRetryAttempts = 4,
                Delay = TimeSpan.FromSeconds(2),
            };

            builder
                .AddRetry(retryOptions)
                .AddTimeout(TimeSpan.FromSeconds(10));
        });
    }
}
