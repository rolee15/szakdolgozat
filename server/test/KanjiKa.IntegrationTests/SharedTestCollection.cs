namespace KanjiKa.IntegrationTests;

[CollectionDefinition("TestContainer")]
public class SharedTestCollection : ICollectionFixture<CustomWebApplicationFactory>;
