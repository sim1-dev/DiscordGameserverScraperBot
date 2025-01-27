using System.Net;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

public class TrackingModule : ApplicationCommandModule<ApplicationCommandContext>
{
    private readonly TrackingService _trackingService;

    public TrackingModule(TrackingService trackingService)
    {
        _trackingService = trackingService;
    }


    [SlashCommand("track", "Start tracking a server")]
    public async Task TrackAsync(string serverIp)
    {
        if (!IPEndPoint.TryParse(serverIp, out _))
        {
            await Context.Interaction.SendResponseAsync(InteractionCallback.Message("Invalid server IP."));
            return;
        }
        

        bool isTracked = await _trackingService.IsTracked(serverIp, Context.Interaction.GuildId);
        if (isTracked)
        {
            await Context.Interaction.SendResponseAsync(InteractionCallback.Message("This server is already being tracked."));
            return;
        }


        await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage(MessageFlags.Loading));

        RestMessage? followupMessage = await Context.Interaction.SendFollowupMessageAsync(new()
        {
            Embeds = [new ServerInfoEmbed(serverIp)]
        });

        if (followupMessage is null)
            return;



        Tracking tracking = new()
        {
            GuildId = Context.Interaction.GuildId!.Value,
            ChannelId = Context.Interaction.Channel.Id,
            MessageId = followupMessage.Id,

            ServerIp = serverIp,

            IssuerId = Context.Interaction.User.Id,
            IssuerUsername = Context.Interaction.User.Username
        };

        await _trackingService.CreateTracking(tracking);


        await Context.Interaction.SendResponseAsync(InteractionCallback.Message($"Started tracking {serverIp} for this channel."));
    }




    [SlashCommand("untrack", "Stop tracking a server")]
    public async Task UntrackAsync(string serverIp)
    {
        if (!IPEndPoint.TryParse(serverIp, out _))
        {
            await Context.Interaction.SendResponseAsync(InteractionCallback.Message("Invalid server IP."));
            return;
        }

        Tracking? tracking = await _trackingService.FindTracking(serverIp, Context.Interaction.GuildId);
        if (tracking == null)
        {
            await Context.Interaction.SendResponseAsync(InteractionCallback.Message("This server is not being tracked."));
            return;
        }


        await _trackingService.DeleteTracking(tracking);


        await Context.Interaction.SendResponseAsync(InteractionCallback.Message($"Stopped tracking {serverIp} for this channel."));
    }
}