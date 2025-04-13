using System.Net.Http.Json;

namespace KanjiKa.IntegrationTests.Kana;

[Collection("IntegrationTests")]
public class GetKanaTest(CustomWebApplicationFactory factory) : IAsyncLifetime
{
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await factory.ResetDatabaseAsync();

    [Fact]
    public async Task GetAllKana_ShouldReturnOk()
    {
        // Arrange & Act
        var response = factory.HttpClient.GetFromJsonAsAsyncEnumerable<KanaCharacterDto>("/api/characters/hiragana?userId=1");

        // Assert
        await foreach (var character in response)
        {
            Assert.NotNull(character);
            Assert.NotEmpty(character.Character);
            Assert.NotEmpty(character.Romaji);
            Assert.NotEmpty(character.Meaning);
            Assert.True(character.UserId > 0);
        }
    }
}

public class KanaCharacterDto
{
    public string Character { get; set; } = string.Empty;
    public string Romaji { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public int UserId { get; set; }
}
