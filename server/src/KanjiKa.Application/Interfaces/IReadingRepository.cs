using KanjiKa.Domain.Entities.Reading;

namespace KanjiKa.Application.Interfaces;

public interface IReadingRepository
{
    Task<List<ReadingPassage>> GetAllAsync();

    Task<ReadingPassage?> GetByIdAsync(int id);

    Task<Dictionary<int, ReadingProficiency>> GetProficienciesForUserAsync(int userId, List<int> passageIds);

    Task UpsertProficiencyAsync(ReadingProficiency proficiency);

    Task SaveChangesAsync();
}
