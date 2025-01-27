using GametrackerServerScraper.Functional;
using GametrackerServerScraper.Models;
using Microsoft.EntityFrameworkCore;
using NetCord;
using NetCord.Rest;
using OpenQA.Selenium;
using Quartz;

public class TrackingJob : IJob
{
    private readonly RestClient _client;
    private readonly IScraper _scraper;
    private readonly TrackingService _trackingService;

    public TrackingJob(RestClient client, IScraper scraper, TrackingService trackingService)
    {
        _client = client;
        _scraper = scraper;
        _trackingService = trackingService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<Tracking> trackings = await _trackingService.GetTrackings();
        
        foreach (Tracking tracking in trackings)
        {
            Console.WriteLine($"Updating tracking for server: {tracking.ServerIp}");

            Channel channel = await _client.GetChannelAsync(tracking.ChannelId);

            ServerInfoEmbed embed;
            try 
            {
                ServerInfo info = _scraper.GetServerInfo(tracking.ServerIp);

                embed = new ServerInfoEmbed(info);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error updating tracking for server {tracking.ServerIp}: {ex.Message}");

                embed = new ServerInfoEmbed();
            }


            try 
            {
                await _client.ModifyMessageAsync(
                    channelId: tracking.ChannelId, 
                    messageId: tracking.MessageId,
                    action: message => message.Embeds = [embed]
                );
            }
            catch(RestException ex)
            {
                if(ex.ReasonPhrase == "Unknown Message.")
                    await _trackingService.DeleteTracking(tracking);

                Console.WriteLine($"Deleting tracking for server {tracking.ServerIp}: {ex.Message}: {ex.ReasonPhrase}");
            }
        }
    }
}
