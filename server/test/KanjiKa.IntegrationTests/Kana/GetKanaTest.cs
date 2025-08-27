#if INTEGRATION_TESTS_ENABLED
namespace KanjiKa.IntegrationTests.Kana;

[Collection("TestContainer")]
public class GetKanaTest(CustomWebApplicationFactory factory) : IAsyncLifetime
{
    public async Task InitializeAsync() => await Task.CompletedTask;
    //await factory.SetUpDatabaseAsync();

    public async Task DisposeAsync() => await Task.CompletedTask;

    [Fact(Skip = "Integration tests are temporarily disabled until TestContainers is ready")]
    public async Task GetAllKana_Hiragana_ShouldReturnOk()
    {
        // Arrange & Act
        var response = await factory.HttpClient.GetAsync("/api/characters/hiragana?userId=1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact(Skip = "Integration tests are temporarily disabled until TestContainers is ready")]
    public async Task GetAllKana_Hiragana_ShouldReturnExpectedCharacters()
    {
        // Arrange
        string[] expectedHiragana = ["あ", "い", "う", "え", "お"];

        // Act
        var response = await factory.HttpClient.GetFromJsonAsync<IEnumerable<KanaCharacterDto>>("/api/characters/hiragana?userId=1");
        var result = response?.ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(69, result.Count);
        Assert.All(expectedHiragana, expected =>
            Assert.Contains(expected, result.Select(dto => dto.Character)));
    }
}

public class KanaCharacterDto
{
    public string Character { get; set; } = string.Empty;
    public string Romaji { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public int UserId { get; set; }
}

#endif
