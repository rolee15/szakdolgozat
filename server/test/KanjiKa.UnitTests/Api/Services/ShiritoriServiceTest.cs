using KanjiKa.Application.Services;
using KanjiKa.Application.Shiritori;

namespace KanjiKa.UnitTests.Api.Services;

public class ShiritoriServiceTests
{
    [Fact]
    public void StartGame_ReturnsWordFromWordList()
    {
        var service = new ShiritoriService();

        ShiritoriMoveResult result = service.StartGame("conn1");

        Assert.NotNull(result.ComputerWord);
        Assert.NotEmpty(result.ComputerWord);
    }

    [Fact]
    public void StartGame_SetsLastCharToLastCharOfStartWord()
    {
        var service = new ShiritoriService();

        ShiritoriMoveResult result = service.StartGame("conn1");

        Assert.Equal(result.ComputerWord![^1], result.NextChar);
    }

    [Fact]
    public void SubmitWord_ValidWord_ReturnsIsValidTrue()
    {
        var service = new ShiritoriService();
        ShiritoriMoveResult start = service.StartGame("conn1");
        char required = start.NextChar!.Value;
        string validWord = required + "なまず";

        ShiritoriMoveResult result = service.SubmitWord("conn1", validWord);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void SubmitWord_WrongStartChar_ReturnsIsValidFalse()
    {
        var service = new ShiritoriService();
        ShiritoriMoveResult start = service.StartGame("conn1");
        char required = start.NextChar!.Value;
        char wrong = required == 'あ' ? 'か' : 'あ';
        string badWord = wrong + "いぬ";

        ShiritoriMoveResult result = service.SubmitWord("conn1", badWord);

        Assert.False(result.IsValid);
        Assert.Contains(required.ToString(), result.Reason);
    }

    [Fact]
    public void SubmitWord_RepeatedWord_ReturnsIsValidFalse()
    {
        var service = new ShiritoriService();
        ShiritoriMoveResult start = service.StartGame("conn1");
        char required = start.NextChar!.Value;
        string word = required + "なまず";

        service.SubmitWord("conn1", word);
        ShiritoriMoveResult second = service.SubmitWord("conn1", word);

        Assert.False(second.IsValid);
        Assert.Equal("その言葉はすでに使われました", second.Reason);
    }

    [Fact]
    public void SubmitWord_WordEndingInN_PlayerLoses()
    {
        var service = new ShiritoriService();
        ShiritoriMoveResult start = service.StartGame("conn1");
        char required = start.NextChar!.Value;
        string wordEndingInN = required + "ん";

        ShiritoriMoveResult result = service.SubmitWord("conn1", wordEndingInN);

        Assert.False(result.IsValid);
        Assert.True(result.GameOver);
        Assert.Equal("computer", result.Winner);
    }

    [Fact]
    public void SubmitWord_WithoutActiveGame_ReturnsIsValidFalse()
    {
        var service = new ShiritoriService();

        ShiritoriMoveResult result = service.SubmitWord("no-game", "あいうえお");

        Assert.False(result.IsValid);
        Assert.Equal("ゲームが開始されていません", result.Reason);
    }

    [Fact]
    public void AbandonGame_ClearsState()
    {
        var service = new ShiritoriService();
        service.StartGame("conn1");
        service.AbandonGame("conn1");

        ShiritoriMoveResult result = service.SubmitWord("conn1", "あいうえお");

        Assert.False(result.IsValid);
        Assert.Equal("ゲームが開始されていません", result.Reason);
    }

    [Fact]
    public void SubmitWord_WhenComputerHasNoReply_PlayerWins()
    {
        var service = new ShiritoriService();
        ShiritoriMoveResult start = service.StartGame("conn1");
        char required = start.NextChar!.Value;
        string wordEndingWithUnusedChar = required + "びー";

        ShiritoriMoveResult result = service.SubmitWord("conn1", wordEndingWithUnusedChar);

        if (result.GameOver)
            Assert.Equal("player", result.Winner);
        else
            Assert.True(result.IsValid);
    }
}
