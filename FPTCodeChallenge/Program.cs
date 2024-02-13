// See https://aka.ms/new-console-template for more information
using AdviseService;
using IAdviseService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherService;
using WeatherServiceInterface;


public class Program
{
    public static async Task Main(string[] args)
    {
        // Setup DI
        //var serviceProvider = new ServiceCollection()
        //    .AddSingleton<IWeatherForecast, WeatherForecast>()
        //    .BuildServiceProvider();


        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Environment.ContentRootPath = Directory.GetCurrentDirectory();
        builder.Configuration.AddJsonFile("appsettings.json", optional: false);
        builder.Configuration.AddEnvironmentVariables(prefix: "PREFIX_");
        builder.Configuration.AddCommandLine(args);

        builder.Services.AddSingleton<IWeatherForecast, WeatherForecast>();
        builder.Services.AddSingleton<IAdviceService, AdviceService>();

        using IHost host = builder.Build();

        await ShowQuery(host.Services);

        await host.RunAsync();
    }

    public static async Task ShowQuery(IServiceProvider serviceProvider)
    {
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
}
