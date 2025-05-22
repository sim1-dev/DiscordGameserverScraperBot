# Gameserver Discord Scraper Bot

A small .NET 9 implementation of a Selenium gameserver scraper, which exposes a set of interfaces to build a custom scraper and outputs the scraped data onto a Discord bot.

It currently implements a Gametracker scraper you can reference to as an example. Just implement and inject yours as of your needs:



```csharp
builder.Services
    .AddSingleton<IScraper, GametrackerScraper>()
    .AddScoped<TrackingService>()
;
```
## Environment Variables

To run this project, you will need to add the following environment variables to your appSettings.json file:

```json
{
  "ConnectionStrings": {
    "Database": "Host=localhost;Port=5432;Database=gametracker;Username=postgres;Password=your_password"
  },
  "Discord": {
    "Token": "your_discord_bot_token"
  },
  "Scraper": {
    "IntervalSeconds": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```





Dependencies: NetCord, Entity Framework Core, Quartz, Selenium


## License

[MIT](https://choosealicense.com/licenses/mit/)
