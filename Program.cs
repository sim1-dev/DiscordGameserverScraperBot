using GametrackerServerScraper.Functional;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Services.ApplicationCommands;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Quartz;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("Database") ?? string.Empty;

builder.Services.AddDbContext<BaseContext>(options =>
    options.UseNpgsql(connectionString)
);


builder.Services
    .AddDiscordGateway(options =>
        {
            options.Token = builder.Configuration["Discord:Token"];
            options.Intents = GatewayIntents.Guilds | GatewayIntents.GuildMessages;
        })
    .AddApplicationCommands<ApplicationCommandInteraction, ApplicationCommandContext>();
;


builder.Services.AddQuartz(q =>
{
    q.AddJob<TrackingJob>(opts => opts.WithIdentity("TrackingJob"));
    q.AddTrigger(opts => opts
        .ForJob("TrackingJob")
        .WithIdentity("TrackingJobTrigger")
        .StartNow()
        .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(builder.Configuration.GetValue<int>("Scraper:IntervalSeconds"))
            .RepeatForever()));
});

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

builder.Services
    .AddSingleton<IScraper, GametrackerScraper>()
    .AddScoped<TrackingService>()
;

builder.Services.AddTransient<IWebDriver, ChromeDriver>();

IHost host = builder.Build();

host.AddSlashCommand("ping", "Ping!", () => "Pong!");

host.AddModules(typeof(Program).Assembly);

host.UseGatewayEventHandlers();

await host.RunAsync();