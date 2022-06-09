using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;
using TBotFinal.Controllers;
using TBotFinal.Configuration;
using TBotFinal.Services;

namespace TBotFinal
{
    static class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("Starting Service");
            await host.RunAsync();
            Console.WriteLine("Service stopped");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings());
            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddTransient<DefaultMessageController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            services.AddHostedService<Bot>();
        }
        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "5384319470:AAEu74oy_RJP7WS-5LgnJrPG93RISS-EV2U"
            };
        }
    }
}