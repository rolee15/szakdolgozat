using KanjiKa.Api.Hubs;
using KanjiKa.Application.Interfaces;
using KanjiKa.Application.Shiritori;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace KanjiKa.UnitTests.Api.Hubs;

public class ShiritoriHubTests
{
    private static ShiritoriHub CreateHub(
        IShiritoriService service,
        out Mock<IHubCallerClients> mockClients,
        out Mock<ISingleClientProxy> mockCaller,
        string connectionId = "test-conn")
    {
        mockClients = new Mock<IHubCallerClients>();
        mockCaller = new Mock<ISingleClientProxy>();
        mockClients.Setup(c => c.Caller).Returns(mockCaller.Object);

        var mockContext = new Mock<HubCallerContext>();
        mockContext.Setup(c => c.ConnectionId).Returns(connectionId);

        var hub = new ShiritoriHub(service);
        hub.Clients = mockClients.Object;
        hub.Context = mockContext.Object;

        return hub;
    }

    [Fact]
    public async Task StartGame_CallsServiceAndSendsGameStarted()
    {
        var mockService = new Mock<IShiritoriService>();
        var expectedResult = new ShiritoriMoveResult { IsValid = true, ComputerWord = "いぬ", NextChar = 'ぬ' };
        mockService.Setup(s => s.StartGame("test-conn")).Returns(expectedResult);

        ShiritoriHub hub = CreateHub(mockService.Object, out _, out Mock<ISingleClientProxy> mockCaller);

        await hub.StartGame();

        mockService.Verify(s => s.StartGame("test-conn"), Times.Once);
        mockCaller.Verify(c => c.SendCoreAsync("GameStarted",
            It.Is<object?[]>(args => args.Length == 1 && args[0] == expectedResult),
            default), Times.Once);
    }

    [Fact]
    public async Task SubmitWord_CallsServiceAndSendsMoveResult()
    {
        var mockService = new Mock<IShiritoriService>();
        var expectedResult = new ShiritoriMoveResult { IsValid = true, NextChar = 'か' };
        mockService.Setup(s => s.SubmitWord("test-conn", "ぬいぐるみ")).Returns(expectedResult);

        ShiritoriHub hub = CreateHub(mockService.Object, out _, out Mock<ISingleClientProxy> mockCaller);

        await hub.SubmitWord("ぬいぐるみ");

        mockService.Verify(s => s.SubmitWord("test-conn", "ぬいぐるみ"), Times.Once);
        mockCaller.Verify(c => c.SendCoreAsync("MoveResult",
            It.Is<object?[]>(args => args.Length == 1 && args[0] == expectedResult),
            default), Times.Once);
    }

    [Fact]
    public async Task OnDisconnectedAsync_CallsAbandonGame()
    {
        var mockService = new Mock<IShiritoriService>();

        ShiritoriHub hub = CreateHub(mockService.Object, out _, out _);

        await hub.OnDisconnectedAsync(null);

        mockService.Verify(s => s.AbandonGame("test-conn"), Times.Once);
    }
}
