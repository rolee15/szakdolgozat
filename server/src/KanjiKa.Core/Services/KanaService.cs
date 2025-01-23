using KanjiKa.Core.Dtos;
using KanjiKa.Core.Entities;
using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Interfaces;

namespace KanjiKa.Core.Services;

    public class KanaService : IKanaService
    {
        private readonly List<Character> _characters;
        private readonly List<User> _users;

        public KanaService()
        {
            // Load test data
            _characters = TestData.GetKanaCharacters();
            _users = TestData.GetUsers();
        }

        public async Task<IEnumerable<KanaCharacterDto>> GetKanaCharacters(KanaType type, string userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);

            return _characters
                .Where(c => c.Type == type)
                .Select(c => new KanaCharacterDto
                {
                    Character = c.Symbol,
                    Romanization = c.Romanization,
                    Type = c.Type,
                    Proficiency = user?.Proficiencies
                        .FirstOrDefault(p => p.CharacterId == c.Id)
                        ?.Level ?? 0
                });
        }

        public async Task<KanaCharacterDetailDto> GetCharacterDetail(string character, KanaType type, string userId)
        {
            var kanaCharacter = _characters.FirstOrDefault(c =>
                c.Symbol == character && c.Type == type);

            if (kanaCharacter == null)
                throw new KeyNotFoundException("Character not found");

            var user = _users.FirstOrDefault(u => u.Id == userId);
            var proficiency = user?.Proficiencies
                .FirstOrDefault(p => p.CharacterId == kanaCharacter.Id)
                ?.Level ?? 0;

            return new KanaCharacterDetailDto
            {
                Character = kanaCharacter.Symbol,
                Romanization = kanaCharacter.Romanization,
                Type = kanaCharacter.Type,
                Proficiency = proficiency
            };
        }

        public async Task<IEnumerable<string>> GetExamples(string character, KanaType type)
        {
            var @char = _characters.FirstOrDefault(c =>
                c.Symbol == character && c.Type == type);

            if (@char == null)
                return new List<string>();

            return @char.Examples.Select(e =>
                $"{e.Word} ({e.Romanization}) - {e.Meaning}");
        }
    }
