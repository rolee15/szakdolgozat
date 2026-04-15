// [7] T. Coil, "Hanabira.org: Japanese Grammar and Vocabulary Learning Resource" — https://github.com/tristcoil/hanabira.org (accessed 2026-04-04)
using System.Reflection;
using System.Text.Json;
using KanjiKa.Domain.Entities.Grammar;

namespace KanjiKa.Data;

// Todo: Optimize, and make it readable and robust
public static class GrammarDataParser
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static List<GrammarPoint> Parse()
    {
        Assembly assembly = typeof(GrammarDataParser).Assembly;
        using Stream stream = assembly.GetManifestResourceStream("KanjiKa.Data.Data.grammar-n5.json")!;

        List<GrammarPointRecord> records = JsonSerializer.Deserialize<List<GrammarPointRecord>>(stream, JsonOptions)
                                           ?? throw new InvalidOperationException("Failed to deserialize grammar-n5.json");

        return records.Select(r => new GrammarPoint
        {
            Title = r.Title,
            Pattern = r.Pattern,
            Explanation = r.Explanation,
            JlptLevel = r.JlptLevel,
            SortOrder = r.SortOrder,
            Examples = r.Examples.Select(e => new GrammarExample
            {
                Japanese = e.Japanese,
                Reading = e.Reading,
                English = e.English
            }).ToList(),
            Exercises = r.Exercises.Select(ex => new GrammarExercise
            {
                Sentence = ex.Sentence,
                CorrectAnswer = ex.CorrectAnswer,
                Distractor1 = ex.Distractor1,
                Distractor2 = ex.Distractor2,
                Distractor3 = ex.Distractor3
            }).ToList()
        }).ToList();
    }

    private sealed record GrammarPointRecord(
        string Title,
        string Pattern,
        string Explanation,
        int JlptLevel,
        int SortOrder,
        List<GrammarExampleRecord> Examples,
        List<GrammarExerciseRecord> Exercises);

    private sealed record GrammarExampleRecord(
        string Japanese,
        string Reading,
        string English);

    private sealed record GrammarExerciseRecord(
        string Sentence,
        string CorrectAnswer,
        string Distractor1,
        string Distractor2,
        string Distractor3);
}
