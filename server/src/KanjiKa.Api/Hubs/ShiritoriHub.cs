using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using KanjiKa.Application.Interfaces;

namespace KanjiKa.Api.Hubs;

[Authorize]
public class ShiritoriHub : Hub
{
    private readonly IShiritoriService _shiritoriService;

    public ShiritoriHub(IShiritoriService shiritoriService)
    {
        _shiritoriService = shiritoriService;
    }

    public async Task StartGame()
    {
        var result = _shiritoriService.StartGame(Context.ConnectionId);
        await Clients.Caller.SendAsync("GameStarted", result);
    }

    public async Task SubmitWord(string word)
    {
        var result = _shiritoriService.SubmitWord(Context.ConnectionId, word);
        await Clients.Caller.SendAsync("MoveResult", result);
    }

    public async Task Abandon()
    {
        _shiritoriService.AbandonGame(Context.ConnectionId);
        await Clients.Caller.SendAsync("GameAbandoned");
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _shiritoriService.AbandonGame(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}
