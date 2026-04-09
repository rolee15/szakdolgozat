using KanjiKa.Application.Shiritori;

namespace KanjiKa.Application.Interfaces;

public interface IShiritoriService
{
    ShiritoriMoveResult StartGame(string connectionId);
    ShiritoriMoveResult SubmitWord(string connectionId, string word);
    void AbandonGame(string connectionId);
}
