using System.Collections.Concurrent;
using KanjiKa.Application.Interfaces;
using KanjiKa.Application.Shiritori;

namespace KanjiKa.Application.Services;

public class ShiritoriService : IShiritoriService
{
    private static readonly string[] WordList =
    [
        // あ行
        "あり", "あひる", "あさがお", "あざらし",
        "いぬ", "いるか", "いちご", "いかり",
        "うし", "うさぎ", "うぐいす", "うなぎ",
        "えび", "えんぴつ", "えがお",
        "おに", "おかし", "おたまじゃくし", "おむすび",
        // か行
        "かに", "かめ", "かわうそ", "かぼちゃ", "かえる",
        "きつね", "きのこ", "きりん",
        "くじら", "くも", "くり",
        "けもの", "けしゴム",
        "こいぬ", "こたつ", "こうもり", "こぶた",
        // さ行
        "さかな", "さくら", "さる",
        "しまうま", "しか",
        "すずめ", "すいか",
        "せみ", "せなか",
        "そら", "そりー",
        // た行
        "たこ", "たぬき", "たわし",
        "ちょうちょ", "ちず",
        "つる", "つき", "つくし",
        "てんぐ", "てぶくろ",
        "とり", "とかげ", "とびうお",
        // な行
        "なす", "なまず", "なまこ",
        "にわとり", "にじ",
        "ぬいぐるみ", "ぬか",
        "ねこ", "ねずみ",
        "のり", "のら",
        // は行
        "はな", "はち", "はくちょう",
        "ひつじ", "ひよこ",
        "ふね", "ふくろう",
        "へび", "へら",
        "ほし", "ほたる", "ほぼ",
        // ま行
        "まつ", "まぐろ", "まり",
        "みかん", "みず", "みつばち",
        "むし", "むすび",
        "めだか", "めがね",
        "もり", "もぐら",
        // や行
        "やぎ", "やもり",
        "ゆき", "ゆびわ",
        "よる", "よしきり",
        // ら行
        "らくだ", "らっぱ",
        "りす", "りゅう",
        "るびー",
        "れもの",
        "ろばた",
        // わ行
        "わに", "わた", "わし",
    ];

    private static readonly Random Rng = new();
    private readonly ConcurrentDictionary<string, ShiritoriGameState> _games = new();

    public ShiritoriMoveResult StartGame(string connectionId)
    {
        string startWord = WordList[Rng.Next(WordList.Length)];
        var state = new ShiritoriGameState
        {
            LastChar = startWord[^1]
        };
        state.UsedWords.Add(startWord);
        _games[connectionId] = state;

        return new ShiritoriMoveResult
        {
            IsValid = true,
            ComputerWord = startWord,
            NextChar = startWord[^1],
            GameOver = false
        };
    }

    public ShiritoriMoveResult SubmitWord(string connectionId, string word)
    {
        if (!_games.TryGetValue(connectionId, out ShiritoriGameState? state))
            return new ShiritoriMoveResult { IsValid = false, Reason = "ゲームが開始されていません" };

        if (word[^1] == 'ん')
            return new ShiritoriMoveResult { IsValid = false, Reason = "んで終わる言葉は使えません", GameOver = true, Winner = "computer" };

        if (word[0] != state.LastChar)
            return new ShiritoriMoveResult { IsValid = false, Reason = $"「{state.LastChar}」で始まる言葉を入力してください" };

        if (state.UsedWords.Contains(word))
            return new ShiritoriMoveResult { IsValid = false, Reason = "その言葉はすでに使われました" };

        state.UsedWords.Add(word);

        string[] candidates = WordList
            .Where(w => w[0] == word[^1] && !state.UsedWords.Contains(w))
            .ToArray();

        if (candidates.Length == 0)
            return new ShiritoriMoveResult { IsValid = true, NextChar = word[^1], GameOver = true, Winner = "player" };

        string computerWord = candidates[Rng.Next(candidates.Length)];
        state.UsedWords.Add(computerWord);
        state.LastChar = computerWord[^1];

        return new ShiritoriMoveResult
        {
            IsValid = true,
            NextChar = computerWord[^1],
            ComputerWord = computerWord,
            GameOver = false
        };
    }

    public void AbandonGame(string connectionId)
    {
        _games.TryRemove(connectionId, out _);
    }
}
