using KanjiKa.Core.Services;

namespace KanjiKa.UnitTests.Core.Services;

public class DummyEmailServiceTest
{
    [Fact]
    public async Task DummyEmailService_SendEmail_Completes()
    {
        var service = new DummyEmailService();

        Task task = service.SendEmail("dummy@test.com", "Test Subject", "Test Body");
        await task;

        Assert.True(task.IsCompletedSuccessfully);
    }
}
