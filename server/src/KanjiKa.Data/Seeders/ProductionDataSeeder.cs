using KanjiKa.Core;
using KanjiKa.Core.Entities.Grammar;
using KanjiKa.Core.Entities.Path;
using KanjiKa.Core.Entities.Reading;
using KanjiKa.Core.Entities.Users;
using KanjiKa.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Data.Seeders;

public class ProductionDataSeeder : IDataSeeder
{
    protected readonly KanjiKaDbContext Context;
    private readonly IHashService _hashService;

    public ProductionDataSeeder(KanjiKaDbContext context, IHashService hashService)
    {
        Context = context;
        _hashService = hashService;
    }

    public virtual async Task SeedAsync()
    {
        await SeedCharacters();
        await SeedKanjis();
        await SeedGrammar();
        await SeedReadingPassages();
        await SeedLearningPath();
        await SeedAdminUser();
    }

    private async Task SeedCharacters()
    {
        if (await Context.Characters.AnyAsync())
            return;

        var characters = TestData.GetKanaCharacters();
        await Context.Characters.AddRangeAsync(characters);
        await Context.SaveChangesAsync();
    }

    private async Task SeedKanjis()
    {
        if (await Context.Kanjis.AnyAsync())
            return;

        var kanjis = Kanjidic2Parser.Parse();
        await Context.Kanjis.AddRangeAsync(kanjis);
        await Context.SaveChangesAsync();
    }

    private async Task SeedGrammar()
    {
        if (await Context.GrammarPoints.AnyAsync())
            return;

        var grammarPoints = GrammarDataParser.Parse();
        await Context.GrammarPoints.AddRangeAsync(grammarPoints);
        await Context.SaveChangesAsync();
    }

    private async Task SeedReadingPassages()
    {
        if (await Context.ReadingPassages.AnyAsync())
            return;

        var passages = new List<ReadingPassage>
        {
            new()
            {
                Title = "Self-Introduction",
                Content = "わたしは田中たろうです。にほんのとうきょうからきました。にじゅうさんさいです。だいがくせいです。えいごとにほんごをべんきょうしています。よろしくおねがいします。",
                JlptLevel = 5,
                Source = "Original",
                SortOrder = 1,
                Questions =
                [
                    new ComprehensionQuestion
                    {
                        QuestionText = "田中たろうさんはどこからきましたか？",
                        OptionA = "おおさか",
                        OptionB = "とうきょう",
                        OptionC = "きょうと",
                        OptionD = "なごや",
                        CorrectOption = 'B',
                        SortOrder = 1
                    },
                    new ComprehensionQuestion
                    {
                        QuestionText = "田中たろうさんはなんさいですか？",
                        OptionA = "にじゅういちさい",
                        OptionB = "にじゅうごさい",
                        OptionC = "にじゅうさんさい",
                        OptionD = "にじゅうはっさい",
                        CorrectOption = 'C',
                        SortOrder = 2
                    },
                    new ComprehensionQuestion
                    {
                        QuestionText = "田中たろうさんはなにをべんきょうしていますか？",
                        OptionA = "すうがくとかがく",
                        OptionB = "えいごとにほんご",
                        OptionC = "れきしとちり",
                        OptionD = "おんがくとびじゅつ",
                        CorrectOption = 'B',
                        SortOrder = 3
                    }
                ]
            },
            new()
            {
                Title = "Daily Routine",
                Content = "まいあさ、しちじにおきます。シャワーをあびて、あさごはんをたべます。はちじにうちをでます。でんしゃでがっこうにいきます。ごごよじにかえります。よるははちじにねます。",
                JlptLevel = 5,
                Source = "Original",
                SortOrder = 2,
                Questions =
                [
                    new ComprehensionQuestion
                    {
                        QuestionText = "まいあさなんじにおきますか？",
                        OptionA = "ろくじ",
                        OptionB = "しちじ",
                        OptionC = "はちじ",
                        OptionD = "くじ",
                        CorrectOption = 'B',
                        SortOrder = 1
                    },
                    new ComprehensionQuestion
                    {
                        QuestionText = "がっこうにどうやっていきますか？",
                        OptionA = "バスで",
                        OptionB = "じてんしゃで",
                        OptionC = "でんしゃで",
                        OptionD = "あるいて",
                        CorrectOption = 'C',
                        SortOrder = 2
                    },
                    new ComprehensionQuestion
                    {
                        QuestionText = "よるなんじにねますか？",
                        OptionA = "しちじ",
                        OptionB = "くじ",
                        OptionC = "じゅうじ",
                        OptionD = "はちじ",
                        CorrectOption = 'D',
                        SortOrder = 3
                    }
                ]
            },
            new()
            {
                Title = "At the Convenience Store",
                Content = "コンビニにはいりました。おにぎりとジュースをかいたいです。おにぎりはひゃくごじゅうえんです。ジュースはひゃくえんです。あわせてにひゃくごじゅうえんはらいました。ありがとうございます。",
                JlptLevel = 5,
                Source = "Original",
                SortOrder = 3,
                Questions =
                [
                    new ComprehensionQuestion
                    {
                        QuestionText = "おにぎりはいくらですか？",
                        OptionA = "ひゃくえん",
                        OptionB = "にひゃくえん",
                        OptionC = "ひゃくごじゅうえん",
                        OptionD = "さんびゃくえん",
                        CorrectOption = 'C',
                        SortOrder = 1
                    },
                    new ComprehensionQuestion
                    {
                        QuestionText = "ぜんぶでいくらはらいましたか？",
                        OptionA = "ひゃくえん",
                        OptionB = "にひゃくごじゅうえん",
                        OptionC = "さんびゃくえん",
                        OptionD = "よんひゃくえん",
                        CorrectOption = 'B',
                        SortOrder = 2
                    },
                    new ComprehensionQuestion
                    {
                        QuestionText = "なにをかいましたか？",
                        OptionA = "パンとみず",
                        OptionB = "すしとお茶",
                        OptionC = "おにぎりとジュース",
                        OptionD = "ケーキとコーヒー",
                        CorrectOption = 'C',
                        SortOrder = 3
                    }
                ]
            },
            new()
            {
                Title = "Asking for Directions",
                Content = "すみません、えきはどこですか。まっすぐいって、みぎにまがってください。こうさてんをひだりにまがると、えきがみえます。あるいてごふんぐらいです。ありがとうございます。",
                JlptLevel = 5,
                Source = "Original",
                SortOrder = 4,
                Questions =
                [
                    new ComprehensionQuestion
                    {
                        QuestionText = "えきまでどのくらいかかりますか？",
                        OptionA = "じゅっぷん",
                        OptionB = "いちじかん",
                        OptionC = "ごふん",
                        OptionD = "さんじゅっぷん",
                        CorrectOption = 'C',
                        SortOrder = 1
                    },
                    new ComprehensionQuestion
                    {
                        QuestionText = "こうさてんでどちらにまがりますか？",
                        OptionA = "まっすぐ",
                        OptionB = "ひだり",
                        OptionC = "みぎ",
                        OptionD = "もどる",
                        CorrectOption = 'B',
                        SortOrder = 2
                    },
                    new ComprehensionQuestion
                    {
                        QuestionText = "さいしょにどちらにいきますか？",
                        OptionA = "ひだりにまがる",
                        OptionB = "みぎにまがる",
                        OptionC = "まっすぐいく",
                        OptionD = "もどる",
                        CorrectOption = 'C',
                        SortOrder = 3
                    }
                ]
            },
            new()
            {
                Title = "The Weather Today",
                Content = "きょうはてんきがいいです。そらはあおくて、くもがすくないです。きおんはにじゅうごどです。かぜがすこしふいています。こうえんにいきたいですね。あしたはあめがふるかもしれません。",
                JlptLevel = 5,
                Source = "Original",
                SortOrder = 5,
                Questions =
                [
                    new ComprehensionQuestion
                    {
                        QuestionText = "きょうのきおんはなんどですか？",
                        OptionA = "じゅうごど",
                        OptionB = "さんじゅうど",
                        OptionC = "にじゅうど",
                        OptionD = "にじゅうごど",
                        CorrectOption = 'D',
                        SortOrder = 1
                    },
                    new ComprehensionQuestion
                    {
                        QuestionText = "きょうのそらはどんなですか？",
                        OptionA = "くもっている",
                        OptionB = "あおい",
                        OptionC = "あめがふっている",
                        OptionD = "ゆきがふっている",
                        CorrectOption = 'B',
                        SortOrder = 2
                    },
                    new ComprehensionQuestion
                    {
                        QuestionText = "あしたのてんきはどうですか？",
                        OptionA = "はれ",
                        OptionB = "くもり",
                        OptionC = "あめかもしれない",
                        OptionD = "ゆきかもしれない",
                        CorrectOption = 'C',
                        SortOrder = 3
                    }
                ]
            },
            new()
            {
                Title = "A School Day",
                Content = "きょうはがっこうでにほんごのじゅぎょうがありました。せんせいはやさしくておもしろかったです。ともだちといっしょにひるごはんをたべました。ごごはたいいくがありました。たのしいいちにちでした。",
                JlptLevel = 5,
                Source = "Original",
                SortOrder = 6,
                Questions =
                [
                    new ComprehensionQuestion
                    {
                        QuestionText = "きょうはなんのじゅぎょうがありましたか？",
                        OptionA = "えいご",
                        OptionB = "すうがく",
                        OptionC = "にほんご",
                        OptionD = "れきし",
                        CorrectOption = 'C',
                        SortOrder = 1
                    },
                    new ComprehensionQuestion
                    {
                        QuestionText = "ひるごはんはだれとたべましたか？",
                        OptionA = "ひとりで",
                        OptionB = "せんせいと",
                        OptionC = "かぞくと",
                        OptionD = "ともだちと",
                        CorrectOption = 'D',
                        SortOrder = 2
                    },
                    new ComprehensionQuestion
                    {
                        QuestionText = "ごごはなにがありましたか？",
                        OptionA = "おんがく",
                        OptionB = "びじゅつ",
                        OptionC = "たいいく",
                        OptionD = "かがく",
                        CorrectOption = 'C',
                        SortOrder = 3
                    }
                ]
            }
        };

        await Context.ReadingPassages.AddRangeAsync(passages);
        await Context.SaveChangesAsync();
    }

    private async Task SeedLearningPath()
    {
        if (await Context.LearningUnits.AnyAsync())
            return;

        var units = new List<LearningUnit>
        {
            new()
            {
                Title = "Hiragana Vowels",
                Description = "Learn the five basic hiragana vowel characters: あ、い、う、え、お.",
                SortOrder = 1,
                Contents =
                [
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 1, SortOrder = 1 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 2, SortOrder = 2 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 3, SortOrder = 3 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 4, SortOrder = 4 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 5, SortOrder = 5 }
                ],
                Tests =
                [
                    new UnitTest { QuestionText = "Which hiragana represents the sound 'a'?", OptionA = "い", OptionB = "う", OptionC = "あ", OptionD = "え", CorrectOption = 'C', ContentType = ContentType.Kana, SortOrder = 1 },
                    new UnitTest { QuestionText = "Which hiragana represents the sound 'i'?", OptionA = "あ", OptionB = "い", OptionC = "え", OptionD = "お", CorrectOption = 'B', ContentType = ContentType.Kana, SortOrder = 2 },
                    new UnitTest { QuestionText = "Which hiragana represents the sound 'u'?", OptionA = "お", OptionB = "え", OptionC = "あ", OptionD = "う", CorrectOption = 'D', ContentType = ContentType.Kana, SortOrder = 3 },
                    new UnitTest { QuestionText = "Which hiragana represents the sound 'e'?", OptionA = "え", OptionB = "い", OptionC = "う", OptionD = "あ", CorrectOption = 'A', ContentType = ContentType.Kana, SortOrder = 4 },
                    new UnitTest { QuestionText = "Which hiragana represents the sound 'o'?", OptionA = "あ", OptionB = "い", OptionC = "え", OptionD = "お", CorrectOption = 'D', ContentType = ContentType.Kana, SortOrder = 5 }
                ]
            },
            new()
            {
                Title = "Hiragana K-row",
                Description = "Learn the hiragana K-row characters: か、き、く、け、こ.",
                SortOrder = 2,
                Contents =
                [
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 6, SortOrder = 1 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 7, SortOrder = 2 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 8, SortOrder = 3 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 9, SortOrder = 4 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 10, SortOrder = 5 }
                ],
                Tests =
                [
                    new UnitTest { QuestionText = "Which hiragana represents 'ka'?", OptionA = "き", OptionB = "く", OptionC = "か", OptionD = "け", CorrectOption = 'C', ContentType = ContentType.Kana, SortOrder = 1 },
                    new UnitTest { QuestionText = "Which hiragana represents 'ki'?", OptionA = "か", OptionB = "き", OptionC = "こ", OptionD = "く", CorrectOption = 'B', ContentType = ContentType.Kana, SortOrder = 2 },
                    new UnitTest { QuestionText = "Which hiragana represents 'ku'?", OptionA = "け", OptionB = "か", OptionC = "く", OptionD = "き", CorrectOption = 'C', ContentType = ContentType.Kana, SortOrder = 3 },
                    new UnitTest { QuestionText = "Which hiragana represents 'ke'?", OptionA = "く", OptionB = "こ", OptionC = "か", OptionD = "け", CorrectOption = 'D', ContentType = ContentType.Kana, SortOrder = 4 },
                    new UnitTest { QuestionText = "Which hiragana represents 'ko'?", OptionA = "か", OptionB = "き", OptionC = "こ", OptionD = "け", CorrectOption = 'C', ContentType = ContentType.Kana, SortOrder = 5 }
                ]
            },
            new()
            {
                Title = "Hiragana S-row",
                Description = "Learn the hiragana S-row characters: さ、し、す、せ、そ.",
                SortOrder = 3,
                Contents =
                [
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 11, SortOrder = 1 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 12, SortOrder = 2 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 13, SortOrder = 3 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 14, SortOrder = 4 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 15, SortOrder = 5 }
                ],
                Tests =
                [
                    new UnitTest { QuestionText = "Which hiragana represents 'sa'?", OptionA = "し", OptionB = "さ", OptionC = "す", OptionD = "せ", CorrectOption = 'B', ContentType = ContentType.Kana, SortOrder = 1 },
                    new UnitTest { QuestionText = "Which hiragana represents 'shi'?", OptionA = "さ", OptionB = "す", OptionC = "し", OptionD = "そ", CorrectOption = 'C', ContentType = ContentType.Kana, SortOrder = 2 },
                    new UnitTest { QuestionText = "Which hiragana represents 'su'?", OptionA = "す", OptionB = "さ", OptionC = "せ", OptionD = "し", CorrectOption = 'A', ContentType = ContentType.Kana, SortOrder = 3 },
                    new UnitTest { QuestionText = "Which hiragana represents 'se'?", OptionA = "そ", OptionB = "し", OptionC = "す", OptionD = "せ", CorrectOption = 'D', ContentType = ContentType.Kana, SortOrder = 4 },
                    new UnitTest { QuestionText = "Which hiragana represents 'so'?", OptionA = "さ", OptionB = "そ", OptionC = "し", OptionD = "す", CorrectOption = 'B', ContentType = ContentType.Kana, SortOrder = 5 }
                ]
            },
            new()
            {
                Title = "Hiragana T & N rows",
                Description = "Learn the hiragana T-row (た、ち、つ、て、と) and N-row (な、に、ぬ、ね、の) characters.",
                SortOrder = 4,
                Contents =
                [
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 16, SortOrder = 1 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 17, SortOrder = 2 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 18, SortOrder = 3 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 19, SortOrder = 4 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 20, SortOrder = 5 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 21, SortOrder = 6 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 22, SortOrder = 7 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 23, SortOrder = 8 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 24, SortOrder = 9 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 25, SortOrder = 10 }
                ],
                Tests =
                [
                    new UnitTest { QuestionText = "Which hiragana represents 'ta'?", OptionA = "ち", OptionB = "つ", OptionC = "た", OptionD = "て", CorrectOption = 'C', ContentType = ContentType.Kana, SortOrder = 1 },
                    new UnitTest { QuestionText = "Which hiragana represents 'chi'?", OptionA = "た", OptionB = "ち", OptionC = "つ", OptionD = "と", CorrectOption = 'B', ContentType = ContentType.Kana, SortOrder = 2 },
                    new UnitTest { QuestionText = "Which hiragana represents 'na'?", OptionA = "に", OptionB = "ぬ", OptionC = "な", OptionD = "ね", CorrectOption = 'C', ContentType = ContentType.Kana, SortOrder = 3 },
                    new UnitTest { QuestionText = "Which hiragana represents 'ni'?", OptionA = "な", OptionB = "に", OptionC = "ぬ", OptionD = "の", CorrectOption = 'B', ContentType = ContentType.Kana, SortOrder = 4 },
                    new UnitTest { QuestionText = "Which hiragana represents 'no'?", OptionA = "な", OptionB = "に", OptionC = "ね", OptionD = "の", CorrectOption = 'D', ContentType = ContentType.Kana, SortOrder = 5 }
                ]
            },
            new()
            {
                Title = "Katakana Basics",
                Description = "Learn the katakana A-row (ア、イ、ウ、エ、オ) and K-row (カ、キ、ク、ケ、コ) characters.",
                SortOrder = 5,
                Contents =
                [
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 47, SortOrder = 1 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 48, SortOrder = 2 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 49, SortOrder = 3 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 50, SortOrder = 4 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 51, SortOrder = 5 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 52, SortOrder = 6 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 53, SortOrder = 7 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 54, SortOrder = 8 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 55, SortOrder = 9 },
                    new UnitContent { ContentType = ContentType.Kana, ContentId = 56, SortOrder = 10 }
                ],
                Tests =
                [
                    new UnitTest { QuestionText = "Which katakana represents 'a'?", OptionA = "イ", OptionB = "ウ", OptionC = "ア", OptionD = "エ", CorrectOption = 'C', ContentType = ContentType.Kana, SortOrder = 1 },
                    new UnitTest { QuestionText = "Which katakana represents 'i'?", OptionA = "ア", OptionB = "イ", OptionC = "エ", OptionD = "オ", CorrectOption = 'B', ContentType = ContentType.Kana, SortOrder = 2 },
                    new UnitTest { QuestionText = "Which katakana represents 'ka'?", OptionA = "キ", OptionB = "ク", OptionC = "カ", OptionD = "ケ", CorrectOption = 'C', ContentType = ContentType.Kana, SortOrder = 3 },
                    new UnitTest { QuestionText = "Which katakana represents 'ki'?", OptionA = "カ", OptionB = "キ", OptionC = "コ", OptionD = "ク", CorrectOption = 'B', ContentType = ContentType.Kana, SortOrder = 4 },
                    new UnitTest { QuestionText = "Which katakana represents 'ko'?", OptionA = "カ", OptionB = "キ", OptionC = "ケ", OptionD = "コ", CorrectOption = 'D', ContentType = ContentType.Kana, SortOrder = 5 }
                ]
            },
            new()
            {
                Title = "Mixed N5 Review",
                Description = "Review mix of N5 grammar and reading comprehension to consolidate your learning.",
                SortOrder = 6,
                Contents =
                [
                    new UnitContent { ContentType = ContentType.Grammar, ContentId = 1, SortOrder = 1 },
                    new UnitContent { ContentType = ContentType.Grammar, ContentId = 2, SortOrder = 2 },
                    new UnitContent { ContentType = ContentType.Reading, ContentId = 1, SortOrder = 3 },
                    new UnitContent { ContentType = ContentType.Reading, ContentId = 2, SortOrder = 4 }
                ],
                Tests =
                [
                    new UnitTest { QuestionText = "Which particle marks the topic of a sentence?", OptionA = "が", OptionB = "を", OptionC = "に", OptionD = "は", CorrectOption = 'D', ContentType = ContentType.Grammar, SortOrder = 1 },
                    new UnitTest { QuestionText = "Which particle marks the subject of a sentence?", OptionA = "は", OptionB = "が", OptionC = "を", OptionD = "で", CorrectOption = 'B', ContentType = ContentType.Grammar, SortOrder = 2 },
                    new UnitTest { QuestionText = "How do you say 'This is a pen' in Japanese?", OptionA = "これをペンです。", OptionB = "これはペンです。", OptionC = "これがペンです。", OptionD = "ペンはこれです。", CorrectOption = 'B', ContentType = ContentType.Grammar, SortOrder = 3 },
                    new UnitTest { QuestionText = "What does 'まいあさ' mean?", OptionA = "every evening", OptionB = "every night", OptionC = "every morning", OptionD = "every afternoon", CorrectOption = 'C', ContentType = ContentType.Reading, SortOrder = 4 },
                    new UnitTest { QuestionText = "What does 'てんきがいい' mean?", OptionA = "The weather is bad.", OptionB = "The weather is cold.", OptionC = "The weather is hot.", OptionD = "The weather is good.", CorrectOption = 'D', ContentType = ContentType.Reading, SortOrder = 5 }
                ]
            }
        };

        await Context.LearningUnits.AddRangeAsync(units);
        await Context.SaveChangesAsync();
    }

    private async Task SeedAdminUser()
    {
        if (await Context.Users.AnyAsync(u => u.Role == UserRole.Admin))
            return;

        var (hash, salt) = _hashService.Hash("Admin123!");
        var admin = new User
        {
            Username = "admin@kanjika.com",
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = UserRole.Admin,
            MustChangePassword = true
        };
        await Context.Users.AddAsync(admin);
        await Context.SaveChangesAsync();
    }
}
