using KanjiKa.Core.Entities;
using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.Core;

public static class TestData
{
    public static List<Character> GetKanaCharacters()
    {
        return
        [
            new Character
            {
                Id = HiraganaA,
                Symbol = "あ",
                Romanization = "a",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-ame",
                        CharacterId = HiraganaA,
                        Word = "あめ",
                        Romanization = "ame",
                        Meaning = "rain"
                    },

                    new Example
                    {
                        Id = "example-asa",
                        CharacterId = HiraganaA,
                        Word = "あさ",
                        Romanization = "asa",
                        Meaning = "morning"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaI,
                Symbol = "い",
                Romanization = "i",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-inu",
                        CharacterId = HiraganaI,
                        Word = "いぬ",
                        Romanization = "inu",
                        Meaning = "dog"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaU,
                Symbol = "う",
                Romanization = "u",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-ushi",
                        CharacterId = HiraganaU,
                        Word = "うし",
                        Romanization = "ushi",
                        Meaning = "cow"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaE,
                Symbol = "え",
                Romanization = "e",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-eki",
                        CharacterId = HiraganaE,
                        Word = "えき",
                        Romanization = "eki",
                        Meaning = "station"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaO,
                Symbol = "お",
                Romanization = "o",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-omizu",
                        CharacterId = HiraganaO,
                        Word = "おみず",
                        Romanization = "omizu",
                        Meaning = "water"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaKa,
                Symbol = "か",
                Romanization = "ka",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-kasa",
                        CharacterId = HiraganaKa,
                        Word = "かさ",
                        Romanization = "kasa",
                        Meaning = "umbrella"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaKi,
                Symbol = "き",
                Romanization = "ki",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-kiku",
                        CharacterId = HiraganaKi,
                        Word = "きく",
                        Romanization = "kiku",
                        Meaning = "to listen"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaKu,
                Symbol = "く",
                Romanization = "ku",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-kumo",
                        CharacterId = HiraganaKu,
                        Word = "くも",
                        Romanization = "kumo",
                        Meaning = "cloud"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaKe,
                Symbol = "け",
                Romanization = "ke",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-ken",
                        CharacterId = HiraganaKe,
                        Word = "けん",
                        Romanization = "ken",
                        Meaning = "sword"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaKo,
                Symbol = "こ",
                Romanization = "ko",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-koi",
                        CharacterId = HiraganaKo,
                        Word = "こい",
                        Romanization = "koi",
                        Meaning = "love"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaSa,
                Symbol = "さ",
                Romanization = "sa",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-sakana",
                        CharacterId = HiraganaSa,
                        Word = "さかな",
                        Romanization = "sakana",
                        Meaning = "fish"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaShi,
                Symbol = "し",
                Romanization = "shi",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-shiawase",
                        CharacterId = HiraganaShi,
                        Word = "しあわせ",
                        Romanization = "shiawase",
                        Meaning = "happiness"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaSu,
                Symbol = "す",
                Romanization = "su",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-sushi",
                        CharacterId = HiraganaSu,
                        Word = "すし",
                        Romanization = "sushi",
                        Meaning = "sushi"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaSe,
                Symbol = "せ",
                Romanization = "se",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-sen",
                        CharacterId = HiraganaSe,
                        Word = "せん",
                        Romanization = "sen",
                        Meaning = "thousand"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaSo,
                Symbol = "そ",
                Romanization = "so",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-sora",
                        CharacterId = HiraganaSo,
                        Word = "そら",
                        Romanization = "sora",
                        Meaning = "sky"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaTa,
                Symbol = "た",
                Romanization = "ta",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-tako",
                        CharacterId = HiraganaTa,
                        Word = "たこ",
                        Romanization = "tako",
                        Meaning = "octopus"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaChi,
                Symbol = "ち",
                Romanization = "chi",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-chiisai",
                        CharacterId = HiraganaChi,
                        Word = "ちいさい",
                        Romanization = "chiisai",
                        Meaning = "small"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaTsu,
                Symbol = "つ",
                Romanization = "tsu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-tsunami",
                        CharacterId = HiraganaTsu,
                        Word = "つなみ",
                        Romanization = "tsunami",
                        Meaning = "tsunami"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaTe,
                Symbol = "て",
                Romanization = "te",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-tegami",
                        CharacterId = HiraganaTe,
                        Word = "てがみ",
                        Romanization = "tegami",
                        Meaning = "letter"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaTo,
                Symbol = "と",
                Romanization = "to",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-tokyo",
                        CharacterId = HiraganaTo,
                        Word = "とうきょう",
                        Romanization = "toukyou",
                        Meaning = "Tokyo"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaNa,
                Symbol = "な",
                Romanization = "na",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-natsu",
                        CharacterId = HiraganaNa,
                        Word = "なつ",
                        Romanization = "natsu",
                        Meaning = "summer"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaNi,
                Symbol = "に",
                Romanization = "ni",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-nihon",
                        CharacterId = HiraganaNi,
                        Word = "にほん",
                        Romanization = "nihon",
                        Meaning = "Japan"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaNu,
                Symbol = "ぬ",
                Romanization = "nu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-nuki",
                        CharacterId = HiraganaNu,
                        Word = "ぬき",
                        Romanization = "nuki",
                        Meaning = "pull out"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaNe,
                Symbol = "ね",
                Romanization = "ne",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-neko",
                        CharacterId = HiraganaNe,
                        Word = "ねこ",
                        Romanization = "neko",
                        Meaning = "cat"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaNo,
                Symbol = "の",
                Romanization = "no",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-nori",
                        CharacterId = HiraganaNo,
                        Word = "のり",
                        Romanization = "nori",
                        Meaning = "seaweed"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaHa,
                Symbol = "は",
                Romanization = "ha",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-hana",
                        CharacterId = HiraganaHa,
                        Word = "はな",
                        Romanization = "hana",
                        Meaning = "flower"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaHi,
                Symbol = "ひ",
                Romanization = "hi",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-hikari",
                        CharacterId = HiraganaHi,
                        Word = "ひかり",
                        Romanization = "hikari",
                        Meaning = "light"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaFu,
                Symbol = "ふ",
                Romanization = "fu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-fune",
                        CharacterId = HiraganaFu,
                        Word = "ふね",
                        Romanization = "fune",
                        Meaning = "boat"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaHe,
                Symbol = "へ",
                Romanization = "he",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-heya",
                        CharacterId = HiraganaHe,
                        Word = "へや",
                        Romanization = "heya",
                        Meaning = "room"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaHo,
                Symbol = "ほ",
                Romanization = "ho",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-hoshi",
                        CharacterId = HiraganaHo,
                        Word = "ほし",
                        Romanization = "hoshi",
                        Meaning = "star"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaMa,
                Symbol = "ま",
                Romanization = "ma",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-machi",
                        CharacterId = HiraganaMa,
                        Word = "まち",
                        Romanization = "machi",
                        Meaning = "town"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaMi,
                Symbol = "み",
                Romanization = "mi",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-mizu",
                        CharacterId = HiraganaMi,
                        Word = "みず",
                        Romanization = "mizu",
                        Meaning = "water"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaMu,
                Symbol = "む",
                Romanization = "mu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-mushi",
                        CharacterId = HiraganaMu,
                        Word = "むし",
                        Romanization = "mushi",
                        Meaning = "insect"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaMe,
                Symbol = "め",
                Romanization = "me",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-megane",
                        CharacterId = HiraganaMe,
                        Word = "めがね",
                        Romanization = "megane",
                        Meaning = "glasses"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaMo,
                Symbol = "も",
                Romanization = "mo",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-mori",
                        CharacterId = HiraganaMo,
                        Word = "もり",
                        Romanization = "mori",
                        Meaning = "forest"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaYa,
                Symbol = "や",
                Romanization = "ya",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-yama",
                        CharacterId = HiraganaYa,
                        Word = "やま",
                        Romanization = "yama",
                        Meaning = "mountain"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaYu,
                Symbol = "ゆ",
                Romanization = "yu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-yuki",
                        CharacterId = HiraganaYu,
                        Word = "ゆき",
                        Romanization = "yuki",
                        Meaning = "snow"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaYo,
                Symbol = "よ",
                Romanization = "yo",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-yoru",
                        CharacterId = HiraganaYo,
                        Word = "よる",
                        Romanization = "yoru",
                        Meaning = "night"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaRa,
                Symbol = "ら",
                Romanization = "ra",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-rain",
                        CharacterId = HiraganaRa,
                        Word = "らいん",
                        Romanization = "rain",
                        Meaning = "rain"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaRi,
                Symbol = "り",
                Romanization = "ri",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-ring",
                        CharacterId = HiraganaRi,
                        Word = "りんぐ",
                        Romanization = "ringu",
                        Meaning = "ring"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaRu,
                Symbol = "る",
                Romanization = "ru",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-rum",
                        CharacterId = HiraganaRu,
                        Word = "るむ",
                        Romanization = "rumu",
                        Meaning = "room"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaRe,
                Symbol = "れ",
                Romanization = "re",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-rental",
                        CharacterId = HiraganaRe,
                        Word = "れんたる",
                        Romanization = "rentaru",
                        Meaning = "rental"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaRo,
                Symbol = "ろ",
                Romanization = "ro",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-robotto",
                        CharacterId = HiraganaRo,
                        Word = "ろぼっと",
                        Romanization = "robotto",
                        Meaning = "robot"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaWa,
                Symbol = "わ",
                Romanization = "wa",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-wa",
                        CharacterId = HiraganaWa,
                        Word = "わたし",
                        Romanization = "watashi",
                        Meaning = "I"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaWo,
                Symbol = "を",
                Romanization = "wo",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-won",
                        CharacterId = HiraganaWo,
                        Word = "をん",
                        Romanization = "won",
                        Meaning = "won"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaN,
                Symbol = "ん",
                Romanization = "n",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-nihon",
                        CharacterId = HiraganaN,
                        Word = "にほん",
                        Romanization = "nihon",
                        Meaning = "Japan"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaGa,
                Symbol = "が",
                Romanization = "ga",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-gakusei",
                        CharacterId = HiraganaGa,
                        Word = "がくせい",
                        Romanization = "gakusei",
                        Meaning = "student"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaGi,
                Symbol = "ぎ",
                Romanization = "gi",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-gin",
                        CharacterId = HiraganaGi,
                        Word = "ぎん",
                        Romanization = "gin",
                        Meaning = "silver"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaGu,
                Symbol = "ぐ",
                Romanization = "gu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-gumi",
                        CharacterId = HiraganaGu,
                        Word = "ぐみ",
                        Romanization = "gumi",
                        Meaning = "group"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaGe,
                Symbol = "げ",
                Romanization = "ge",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-geemu",
                        CharacterId = HiraganaGe,
                        Word = "げーむ",
                        Romanization = "geemu",
                        Meaning = "game"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaGo,
                Symbol = "ご",
                Romanization = "go",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-gohan",
                        CharacterId = HiraganaGo,
                        Word = "ごはん",
                        Romanization = "gohan",
                        Meaning = "rice"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaZa,
                Symbol = "ざ",
                Romanization = "za",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-zasshi",
                        CharacterId = HiraganaZa,
                        Word = "ざっし",
                        Romanization = "zasshi",
                        Meaning = "magazine"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaJi,
                Symbol = "じ",
                Romanization = "ji",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-jikan",
                        CharacterId = HiraganaJi,
                        Word = "じかん",
                        Romanization = "jikan",
                        Meaning = "time"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaZu,
                Symbol = "ず",
                Romanization = "zu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-zubon",
                        CharacterId = HiraganaZu,
                        Word = "ずぼん",
                        Romanization = "zubon",
                        Meaning = "trousers"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaZe,
                Symbol = "ぜ",
                Romanization = "ze",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-zenbu",
                        CharacterId = HiraganaZe,
                        Word = "ぜんぶ",
                        Romanization = "zenbu",
                        Meaning = "all"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaZo,
                Symbol = "ぞ",
                Romanization = "zo",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-zoo",
                        CharacterId = HiraganaZo,
                        Word = "ぞう",
                        Romanization = "zou",
                        Meaning = "elephant"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaDa,
                Symbol = "だ",
                Romanization = "da",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-dango",
                        CharacterId = HiraganaDa,
                        Word = "だんご",
                        Romanization = "dango",
                        Meaning = "dumpling"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaDe,
                Symbol = "で",
                Romanization = "de",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-denwa",
                        CharacterId = HiraganaDe,
                        Word = "でんわ",
                        Romanization = "denwa",
                        Meaning = "telephone"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaDo,
                Symbol = "ど",
                Romanization = "do",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-doraibu",
                        CharacterId = HiraganaDo,
                        Word = "どらいぶ",
                        Romanization = "doraibu",
                        Meaning = "drive"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaBa,
                Symbol = "ば",
                Romanization = "ba",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-basu",
                        CharacterId = HiraganaBa,
                        Word = "ばす",
                        Romanization = "basu",
                        Meaning = "bus"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaBi,
                Symbol = "び",
                Romanization = "bi",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-biru",
                        CharacterId = HiraganaBi,
                        Word = "びる",
                        Romanization = "biru",
                        Meaning = "beer"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaBu,
                Symbol = "ぶ",
                Romanization = "bu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-budo",
                        CharacterId = HiraganaBu,
                        Word = "ぶどう",
                        Romanization = "budou",
                        Meaning = "grape"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaBe,
                Symbol = "べ",
                Romanization = "be",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-benkyou",
                        CharacterId = HiraganaBe,
                        Word = "べんきょう",
                        Romanization = "benkyou",
                        Meaning = "study"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaBo,
                Symbol = "ぼ",
                Romanization = "bo",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-boku",
                        CharacterId = HiraganaBo,
                        Word = "ぼく",
                        Romanization = "boku",
                        Meaning = "I"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaPa,
                Symbol = "ぱ",
                Romanization = "pa",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-pachinko",
                        CharacterId = HiraganaPa,
                        Word = "ぱちんこ",
                        Romanization = "pachinko",
                        Meaning = "pachinko"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaPi,
                Symbol = "ぴ",
                Romanization = "pi",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-pikachu",
                        CharacterId = HiraganaPi,
                        Word = "ぴかちゅう",
                        Romanization = "pikachuu",
                        Meaning = "Pikachu"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaPu,
                Symbol = "ぷ",
                Romanization = "pu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-purin",
                        CharacterId = HiraganaPu,
                        Word = "ぷりん",
                        Romanization = "purin",
                        Meaning = "pudding"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaPe,
                Symbol = "ぺ",
                Romanization = "pe",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-pen",
                        CharacterId = HiraganaPe,
                        Word = "ぺん",
                        Romanization = "pen",
                        Meaning = "pen"
                    }
                ]
            },

            new Character
            {
                Id = HiraganaPo,
                Symbol = "ぽ",
                Romanization = "po",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-pokemon",
                        CharacterId = HiraganaPo,
                        Word = "ぽけもん",
                        Romanization = "pokemon",
                        Meaning = "Pokemon"
                    }
                ]
            },
            //TODO Add yoon characters


            new Character
            {
                Id = KatakanaA,
                Symbol = "ア",
                Romanization = "a",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-anime",
                        CharacterId = KatakanaA,
                        Word = "アニメ",
                        Romanization = "anime",
                        Meaning = "animation"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaI,
                Symbol = "イ",
                Romanization = "i",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-ikura",
                        CharacterId = KatakanaI,
                        Word = "イクラ",
                        Romanization = "ikura",
                        Meaning = "salmon roe"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaU,
                Symbol = "ウ",
                Romanization = "u",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-uni",
                        CharacterId = KatakanaU,
                        Word = "ウニ",
                        Romanization = "uni",
                        Meaning = "sea urchin"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaE,
                Symbol = "エ",
                Romanization = "e",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-ebi",
                        CharacterId = KatakanaE,
                        Word = "エビ",
                        Romanization = "ebi",
                        Meaning = "shrimp"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaO,
                Symbol = "オ",
                Romanization = "o",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-omuraisu",
                        CharacterId = KatakanaO,
                        Word = "オムライス",
                        Romanization = "omuraisu",
                        Meaning = "omelette rice"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaKa,
                Symbol = "カ",
                Romanization = "ka",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-kamera",
                        CharacterId = KatakanaKa,
                        Word = "カメラ",
                        Romanization = "kamera",
                        Meaning = "camera"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaKi,
                Symbol = "キ",
                Romanization = "ki",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-kiro",
                        CharacterId = KatakanaKi,
                        Word = "キロ",
                        Romanization = "kiro",
                        Meaning = "kilometer"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaKu,
                Symbol = "ク",
                Romanization = "ku",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-kumo",
                        CharacterId = KatakanaKu,
                        Word = "クモ",
                        Romanization = "kumo",
                        Meaning = "spider"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaKe,
                Symbol = "ケ",
                Romanization = "ke",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-ken",
                        CharacterId = KatakanaKe,
                        Word = "ケン",
                        Romanization = "ken",
                        Meaning = "sword"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaKo,
                Symbol = "コ",
                Romanization = "ko",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-koi",
                        CharacterId = KatakanaKo,
                        Word = "コイ",
                        Romanization = "koi",
                        Meaning = "carp"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaSa,
                Symbol = "サ",
                Romanization = "sa",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-sakura",
                        CharacterId = KatakanaSa,
                        Word = "サクラ",
                        Romanization = "sakura",
                        Meaning = "cherry blossom"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaShi,
                Symbol = "シ",
                Romanization = "shi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-shinbun",
                        CharacterId = KatakanaShi,
                        Word = "シンブン",
                        Romanization = "shinbun",
                        Meaning = "newspaper"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaSu,
                Symbol = "ス",
                Romanization = "su",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-sushi",
                        CharacterId = KatakanaSu,
                        Word = "スシ",
                        Romanization = "sushi",
                        Meaning = "sushi"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaSe,
                Symbol = "セ",
                Romanization = "se",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-sen",
                        CharacterId = KatakanaSe,
                        Word = "セン",
                        Romanization = "sen",
                        Meaning = "thousand"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaSo,
                Symbol = "ソ",
                Romanization = "so",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-soba",
                        CharacterId = KatakanaSo,
                        Word = "ソバ",
                        Romanization = "soba",
                        Meaning = "buckwheat noodles"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaTa,
                Symbol = "タ",
                Romanization = "ta",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-takoyaki",
                        CharacterId = KatakanaTa,
                        Word = "タコヤキ",
                        Romanization = "takoyaki",
                        Meaning = "octopus balls"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaChi,
                Symbol = "チ",
                Romanization = "chi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-chiizu",
                        CharacterId = KatakanaChi,
                        Word = "チーズ",
                        Romanization = "chiizu",
                        Meaning = "cheese"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaTsu,
                Symbol = "ツ",
                Romanization = "tsu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-tsuki",
                        CharacterId = KatakanaTsu,
                        Word = "ツキ",
                        Romanization = "tsuki",
                        Meaning = "moon"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaTe,
                Symbol = "テ",
                Romanization = "te",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-tennisu",
                        CharacterId = KatakanaTe,
                        Word = "テニス",
                        Romanization = "tenisu",
                        Meaning = "tennis"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaTo,
                Symbol = "ト",
                Romanization = "to",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-tokyo",
                        CharacterId = KatakanaTo,
                        Word = "トウキョウ",
                        Romanization = "toukyou",
                        Meaning = "Tokyo"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaNa,
                Symbol = "ナ",
                Romanization = "na",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-natsu",
                        CharacterId = KatakanaNa,
                        Word = "ナツ",
                        Romanization = "natsu",
                        Meaning = "summer"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaNi,
                Symbol = "ニ",
                Romanization = "ni",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-nihon",
                        CharacterId = KatakanaNi,
                        Word = "ニホン",
                        Romanization = "nihon",
                        Meaning = "Japan"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaNu,
                Symbol = "ヌ",
                Romanization = "nu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-nuki",
                        CharacterId = KatakanaNu,
                        Word = "ヌキ",
                        Romanization = "nuki",
                        Meaning = "pull out"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaNe,
                Symbol = "ネ",
                Romanization = "ne",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-neko",
                        CharacterId = KatakanaNe,
                        Word = "ネコ",
                        Romanization = "neko",
                        Meaning = "cat"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaNo,
                Symbol = "ノ",
                Romanization = "no",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-nori",
                        CharacterId = KatakanaNo,
                        Word = "ノリ",
                        Romanization = "nori",
                        Meaning = "seaweed"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaHa,
                Symbol = "ハ",
                Romanization = "ha",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-hanabi",
                        CharacterId = KatakanaHa,
                        Word = "ハナビ",
                        Romanization = "hanabi",
                        Meaning = "fireworks"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaHi,
                Symbol = "ヒ",
                Romanization = "hi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-hikouki",
                        CharacterId = KatakanaHi,
                        Word = "ヒコウキ",
                        Romanization = "hikouki",
                        Meaning = "airplane"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaFu,
                Symbol = "フ",
                Romanization = "fu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-fuji",
                        CharacterId = KatakanaFu,
                        Word = "フジ",
                        Romanization = "fuji",
                        Meaning = "Mt. Fuji"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaHe,
                Symbol = "ヘ",
                Romanization = "he",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-henshin",
                        CharacterId = KatakanaHe,
                        Word = "ヘンシン",
                        Romanization = "henshin",
                        Meaning = "transformation"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaHo,
                Symbol = "ホ",
                Romanization = "ho",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-hon",
                        CharacterId = KatakanaHo,
                        Word = "ホン",
                        Romanization = "hon",
                        Meaning = "book"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaMa,
                Symbol = "マ",
                Romanization = "ma",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-maiku",
                        CharacterId = KatakanaMa,
                        Word = "マイク",
                        Romanization = "maiku",
                        Meaning = "microphone"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaMi,
                Symbol = "ミ",
                Romanization = "mi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-miso",
                        CharacterId = KatakanaMi,
                        Word = "ミソ",
                        Romanization = "miso",
                        Meaning = "miso"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaMu,
                Symbol = "ム",
                Romanization = "mu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-mushi",
                        CharacterId = KatakanaMu,
                        Word = "ムシ",
                        Romanization = "mushi",
                        Meaning = "insect"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaMe,
                Symbol = "メ",
                Romanization = "me",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-megane",
                        CharacterId = KatakanaMe,
                        Word = "メガネ",
                        Romanization = "megane",
                        Meaning = "glasses"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaMo,
                Symbol = "モ",
                Romanization = "mo",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-mono",
                        CharacterId = KatakanaMo,
                        Word = "モノ",
                        Romanization = "mono",
                        Meaning = "thing"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaYa,
                Symbol = "ヤ",
                Romanization = "ya",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-yakitori",
                        CharacterId = KatakanaYa,
                        Word = "ヤキトリ",
                        Romanization = "yakitori",
                        Meaning = "grilled chicken"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaYu,
                Symbol = "ユ",
                Romanization = "yu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-yuki",
                        CharacterId = KatakanaYu,
                        Word = "ユキ",
                        Romanization = "yuki",
                        Meaning = "snow"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaYo,
                Symbol = "ヨ",
                Romanization = "yo",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-yoga",
                        CharacterId = KatakanaYo,
                        Word = "ヨガ",
                        Romanization = "yoga",
                        Meaning = "yoga"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaRa,
                Symbol = "ラ",
                Romanization = "ra",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-ramen",
                        CharacterId = KatakanaRa,
                        Word = "ラーメン",
                        Romanization = "raamen",
                        Meaning = "ramen"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaRi,
                Symbol = "リ",
                Romanization = "ri",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-ramune",
                        CharacterId = KatakanaRi,
                        Word = "ラムネ",
                        Romanization = "ramune",
                        Meaning = "ramune"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaRu,
                Symbol = "ル",
                Romanization = "ru",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-ringo",
                        CharacterId = KatakanaRu,
                        Word = "リンゴ",
                        Romanization = "ringo",
                        Meaning = "apple"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaRe,
                Symbol = "レ",
                Romanization = "re",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-reizouko",
                        CharacterId = KatakanaRe,
                        Word = "レイゾウコ",
                        Romanization = "reizouko",
                        Meaning = "refrigerator"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaRo,
                Symbol = "ロ",
                Romanization = "ro",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-robotto",
                        CharacterId = KatakanaRo,
                        Word = "ロボット",
                        Romanization = "robotto",
                        Meaning = "robot"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaWa,
                Symbol = "ワ",
                Romanization = "wa",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-wanpaku",
                        CharacterId = KatakanaWa,
                        Word = "ワンパク",
                        Romanization = "wanpaku",
                        Meaning = "naughty"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaWo,
                Symbol = "ヲ",
                Romanization = "wo",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-wotagei",
                        CharacterId = KatakanaWo,
                        Word = "ヲタゲイ",
                        Romanization = "wotagei",
                        Meaning = "otaku dance"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaN,
                Symbol = "ン",
                Romanization = "n",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-ninja",
                        CharacterId = KatakanaN,
                        Word = "ニンジャ",
                        Romanization = "ninja",
                        Meaning = "ninja"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaGa,
                Symbol = "ガ",
                Romanization = "ga",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-gakkou",
                        CharacterId = KatakanaGa,
                        Word = "ガッコウ",
                        Romanization = "gakkou",
                        Meaning = "school"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaGi,
                Symbol = "ギ",
                Romanization = "gi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-gin",
                        CharacterId = KatakanaGi,
                        Word = "ギン",
                        Romanization = "gin",
                        Meaning = "silver"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaGu,
                Symbol = "グ",
                Romanization = "gu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-gundam",
                        CharacterId = KatakanaGu,
                        Word = "ガンダム",
                        Romanization = "gandamu",
                        Meaning = "Gundam"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaGe,
                Symbol = "ゲ",
                Romanization = "ge",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-geemu",
                        CharacterId = KatakanaGe,
                        Word = "ゲーム",
                        Romanization = "geemu",
                        Meaning = "game"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaGo,
                Symbol = "ゴ",
                Romanization = "go",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-gohan",
                        CharacterId = KatakanaGo,
                        Word = "ゴハン",
                        Romanization = "gohan",
                        Meaning = "rice"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaZa,
                Symbol = "ザ",
                Romanization = "za",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-zasshi",
                        CharacterId = KatakanaZa,
                        Word = "ザッシ",
                        Romanization = "zasshi",
                        Meaning = "magazine"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaJi,
                Symbol = "ジ",
                Romanization = "ji",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-jinrui",
                        CharacterId = KatakanaJi,
                        Word = "ジンルイ",
                        Romanization = "jinrui",
                        Meaning = "humanity"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaZu,
                Symbol = "ズ",
                Romanization = "zu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-zutto",
                        CharacterId = KatakanaZu,
                        Word = "ズット",
                        Romanization = "zutto",
                        Meaning = "always"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaZe,
                Symbol = "ゼ",
                Romanization = "ze",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-zenbu",
                        CharacterId = KatakanaZe,
                        Word = "ゼンブ",
                        Romanization = "zenbu",
                        Meaning = "all"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaZo,
                Symbol = "ゾ",
                Romanization = "zo",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-zou",
                        CharacterId = KatakanaZo,
                        Word = "ゾウ",
                        Romanization = "zou",
                        Meaning = "elephant"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaDa,
                Symbol = "ダ",
                Romanization = "da",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-daisuki",
                        CharacterId = KatakanaDa,
                        Word = "ダイスキ",
                        Romanization = "daisuki",
                        Meaning = "love"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaDe,
                Symbol = "デ",
                Romanization = "de",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-dekai",
                        CharacterId = KatakanaDe,
                        Word = "デカイ",
                        Romanization = "dekai",
                        Meaning = "big"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaDo,
                Symbol = "ド",
                Romanization = "do",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-doraemon",
                        CharacterId = KatakanaDo,
                        Word = "ドラエモン",
                        Romanization = "doraemon",
                        Meaning = "Doraemon"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaBa,
                Symbol = "バ",
                Romanization = "ba",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-banana",
                        CharacterId = KatakanaBa,
                        Word = "バナナ",
                        Romanization = "banana",
                        Meaning = "banana"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaBi,
                Symbol = "ビ",
                Romanization = "bi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-biru",
                        CharacterId = KatakanaBi,
                        Word = "ビール",
                        Romanization = "biiru",
                        Meaning = "beer"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaBu,
                Symbol = "ブ",
                Romanization = "bu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-bus",
                        CharacterId = KatakanaBu,
                        Word = "バス",
                        Romanization = "basu",
                        Meaning = "bus"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaBe,
                Symbol = "ベ",
                Romanization = "be",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-benri",
                        CharacterId = KatakanaBe,
                        Word = "ベンリ",
                        Romanization = "benri",
                        Meaning = "convenient"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaBo,
                Symbol = "ボ",
                Romanization = "bo",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-boku",
                        CharacterId = KatakanaBo,
                        Word = "ボク",
                        Romanization = "boku",
                        Meaning = "I"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaPa,
                Symbol = "パ",
                Romanization = "pa",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-piano",
                        CharacterId = KatakanaPa,
                        Word = "ピアノ",
                        Romanization = "piano",
                        Meaning = "piano"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaPi,
                Symbol = "ピ",
                Romanization = "pi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-pikachu",
                        CharacterId = KatakanaPi,
                        Word = "ピカチュウ",
                        Romanization = "pikachuu",
                        Meaning = "Pikachu"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaPu,
                Symbol = "プ",
                Romanization = "pu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-purin",
                        CharacterId = KatakanaPu,
                        Word = "プリン",
                        Romanization = "purin",
                        Meaning = "pudding"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaPe,
                Symbol = "ペ",
                Romanization = "pe",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-pen",
                        CharacterId = KatakanaPe,
                        Word = "ペン",
                        Romanization = "pen",
                        Meaning = "pen"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaPo,
                Symbol = "ポ",
                Romanization = "po",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-pokemon",
                        CharacterId = KatakanaPo,
                        Word = "ポケモン",
                        Romanization = "pokemon",
                        Meaning = "Pokemon"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaMa,
                Symbol = "マ",
                Romanization = "ma",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-maiku",
                        CharacterId = KatakanaMa,
                        Word = "マイク",
                        Romanization = "maiku",
                        Meaning = "microphone"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaMi,
                Symbol = "ミ",
                Romanization = "mi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-miso",
                        CharacterId = KatakanaMi,
                        Word = "ミソ",
                        Romanization = "miso",
                        Meaning = "miso"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaMu,
                Symbol = "ム",
                Romanization = "mu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-mushi",
                        CharacterId = KatakanaMu,
                        Word = "ムシ",
                        Romanization = "mushi",
                        Meaning = "insect"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaMe,
                Symbol = "メ",
                Romanization = "me",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-megane",
                        CharacterId = KatakanaMe,
                        Word = "メガネ",
                        Romanization = "megane",
                        Meaning = "glasses"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaMo,
                Symbol = "モ",
                Romanization = "mo",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-mono",
                        CharacterId = KatakanaMo,
                        Word = "モノ",
                        Romanization = "mono",
                        Meaning = "thing"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaYa,
                Symbol = "ヤ",
                Romanization = "ya",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-yakitori",
                        CharacterId = KatakanaYa,
                        Word = "ヤキトリ",
                        Romanization = "yakitori",
                        Meaning = "grilled chicken"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaYu,
                Symbol = "ユ",
                Romanization = "yu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-yuki",
                        CharacterId = KatakanaYu,
                        Word = "ユキ",
                        Romanization = "yuki",
                        Meaning = "snow"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaYo,
                Symbol = "ヨ",
                Romanization = "yo",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-yoga",
                        CharacterId = KatakanaYo,
                        Word = "ヨガ",
                        Romanization = "yoga",
                        Meaning = "yoga"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaRa,
                Symbol = "ラ",
                Romanization = "ra",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-ramen",
                        CharacterId = KatakanaRa,
                        Word = "ラーメン",
                        Romanization = "raamen",
                        Meaning = "ramen"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaRi,
                Symbol = "リ",
                Romanization = "ri",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-ringo",
                        CharacterId = KatakanaRi,
                        Word = "リンゴ",
                        Romanization = "ringo"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaRu,
                Symbol = "ル",
                Romanization = "ru",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-rusuban",
                        CharacterId = KatakanaRu,
                        Word = "ルスバン",
                        Romanization = "rusuban",
                        Meaning = "hostel"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaRe,
                Symbol = "レ",
                Romanization = "re",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-reizouko",
                        CharacterId = KatakanaRe,
                        Word = "レイゾウコ",
                        Romanization = "reizouko",
                        Meaning = "refrigerator"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaRo,
                Symbol = "ロ",
                Romanization = "ro",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-robotto",
                        CharacterId = KatakanaRo,
                        Word = "ロボット",
                        Romanization = "robotto",
                        Meaning = "robot"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaWa,
                Symbol = "ワ",
                Romanization = "wa",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-wanpaku",
                        CharacterId = KatakanaWa,
                        Word = "ワンパク",
                        Romanization = "wanpaku",
                        Meaning = "naughty"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaWo,
                Symbol = "ヲ",
                Romanization = "wo",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-wotagei",
                        CharacterId = KatakanaWo,
                        Word = "ヲタゲイ",
                        Romanization = "wotagei",
                        Meaning = "otaku dance"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaN,
                Symbol = "ン",
                Romanization = "n",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-ninja",
                        CharacterId = KatakanaN,
                        Word = "ニンジャ",
                        Romanization = "ninja",
                        Meaning = "ninja"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaGa,
                Symbol = "ガ",
                Romanization = "ga",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-gakkou",
                        CharacterId = KatakanaGa,
                        Word = "ガッコウ",
                        Romanization = "gakkou",
                        Meaning = "school"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaGi,
                Symbol = "ギ",
                Romanization = "gi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-gin",
                        CharacterId = KatakanaGi,
                        Word = "ギン",
                        Romanization = "gin",
                        Meaning = "silver"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaGu,
                Symbol = "グ",
                Romanization = "gu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-gundam",
                        CharacterId = KatakanaGu,
                        Word = "ガンダム",
                        Romanization = "gandamu",
                        Meaning = "Gundam"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaGe,
                Symbol = "ゲ",
                Romanization = "ge",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-geemu",
                        CharacterId = KatakanaGe,
                        Word = "ゲーム",
                        Romanization = "geemu",
                        Meaning = "game"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaGo,
                Symbol = "ゴ",
                Romanization = "go",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-gohan",
                        CharacterId = KatakanaGo,
                        Word = "ゴハン",
                        Romanization = "gohan",
                        Meaning = "rice"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaZa,
                Symbol = "ザ",
                Romanization = "za",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-zasshi",
                        CharacterId = KatakanaZa,
                        Word = "ザッシ",
                        Romanization = "zasshi",
                        Meaning = "magazine"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaJi,
                Symbol = "ジ",
                Romanization = "ji",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-jinrui",
                        CharacterId = KatakanaJi,
                        Word = "ジンルイ",
                        Romanization = "jinrui",
                        Meaning = "humanity"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaZu,
                Symbol = "ズ",
                Romanization = "zu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-zutto",
                        CharacterId = KatakanaZu,
                        Word = "ズット",
                        Romanization = "zutto",
                        Meaning = "always"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaZe,
                Symbol = "ゼ",
                Romanization = "ze",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-zenbu",
                        CharacterId = KatakanaZe,
                        Word = "ゼンブ",
                        Romanization = "zenbu",
                        Meaning = "all"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaZo,
                Symbol = "ゾ",
                Romanization = "zo",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-zou",
                        CharacterId = KatakanaZo,
                        Word = "ゾウ",
                        Romanization = "zou",
                        Meaning = "elephant"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaDa,
                Symbol = "ダ",
                Romanization = "da",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-daisuki",
                        CharacterId = KatakanaDa,
                        Word = "ダイスキ",
                        Romanization = "daisuki",
                        Meaning = "love"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaDe,
                Symbol = "デ",
                Romanization = "de",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-dekai",
                        CharacterId = KatakanaDe,
                        Word = "デカイ",
                        Romanization = "dekai",
                        Meaning = "big"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaDo,
                Symbol = "ド",
                Romanization = "do",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-doraemon",
                        CharacterId = KatakanaDo,
                        Word = "ドラエモン",
                        Romanization = "doraemon",
                        Meaning = "Doraemon"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaBa,
                Symbol = "バ",
                Romanization = "ba",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-banana",
                        CharacterId = KatakanaBa,
                        Word = "バナナ",
                        Romanization = "banana",
                        Meaning = "banana"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaBi,
                Symbol = "ビ",
                Romanization = "bi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-biru",
                        CharacterId = KatakanaBi,
                        Word = "ビール",
                        Romanization = "biiru",
                        Meaning = "beer"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaBu,
                Symbol = "ブ",
                Romanization = "bu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-bus",
                        CharacterId = KatakanaBu,
                        Word = "バス",
                        Romanization = "basu",
                        Meaning = "bus"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaBe,
                Symbol = "ベ",
                Romanization = "be",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-benri",
                        CharacterId = KatakanaBe,
                        Word = "ベンリ",
                        Romanization = "benri",
                        Meaning = "convenient"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaBo,
                Symbol = "ボ",
                Romanization = "bo",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-boku",
                        CharacterId = KatakanaBo,
                        Word = "ボク",
                        Romanization = "boku",
                        Meaning = "I"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaPa,
                Symbol = "パ",
                Romanization = "pa",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-piano",
                        CharacterId = KatakanaPa,
                        Word = "ピアノ",
                        Romanization = "piano",
                        Meaning = "piano"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaPi,
                Symbol = "ピ",
                Romanization = "pi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-pikachu",
                        CharacterId = KatakanaPi,
                        Word = "ピカチュウ",
                        Romanization = "pikachuu",
                        Meaning = "Pikachu"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaPu,
                Symbol = "プ",
                Romanization = "pu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-purin",
                        CharacterId = KatakanaPu,
                        Word = "プリン",
                        Romanization = "purin",
                        Meaning = "pudding"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaPe,
                Symbol = "ペ",
                Romanization = "pe",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-pen",
                        CharacterId = KatakanaPe,
                        Word = "ペン",
                        Romanization = "pen",
                        Meaning = "pen"
                    }
                ]
            },

            new Character
            {
                Id = KatakanaPo,
                Symbol = "ポ",
                Romanization = "po",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Id = "example-pokemon",
                        CharacterId = KatakanaPo,
                        Word = "ポケモン",
                        Romanization = "pokemon",
                        Meaning = "Pokemon"
                    }
                ]
            }
        ];
    }

    public static List<User> GetUsers()
    {
        return
        [
            new User
            {
                Id = "user1",
                Username = "TestUser1",
                Proficiencies =
                [
                    new Proficiency
                    {
                        Id = "prof1",
                        UserId = "user1",
                        CharacterId = HiraganaA,
                        Level = 80,
                        LastPracticed = DateTime.UtcNow.AddDays(-1)
                    },

                    new Proficiency
                    {
                        Id = "prof2",
                        UserId = "user1",
                        CharacterId = HiraganaI,
                        Level = 60,
                        LastPracticed = DateTime.UtcNow.AddDays(-2)
                    }
                ]
            }
        ];
    }

    public const string HiraganaA = "hiragana-a";
    public const string HiraganaI = "hiragana-i";
    public const string HiraganaU = "hiragana-u";
    public const string HiraganaE = "hiragana-e";
    public const string HiraganaO = "hiragana-o";
    public const string HiraganaKa = "hiragana-ka";
    public const string HiraganaKi = "hiragana-ki";
    public const string HiraganaKu = "hiragana-ku";
    public const string HiraganaKe = "hiragana-ke";
    public const string HiraganaKo = "hiragana-ko";
    public const string HiraganaSa = "hiragana-sa";
    public const string HiraganaShi = "hiragana-shi";
    public const string HiraganaSu = "hiragana-su";
    public const string HiraganaSe = "hiragana-se";
    public const string HiraganaSo = "hiragana-so";
    public const string HiraganaTa = "hiragana-ta";
    public const string HiraganaChi = "hiragana-chi";
    public const string HiraganaTsu = "hiragana-tsu";
    public const string HiraganaTe = "hiragana-te";
    public const string HiraganaTo = "hiragana-to";
    public const string HiraganaNa = "hiragana-na";
    public const string HiraganaNi = "hiragana-ni";
    public const string HiraganaNu = "hiragana-nu";
    public const string HiraganaNe = "hiragana-ne";
    public const string HiraganaNo = "hiragana-no";
    public const string HiraganaHa = "hiragana-ha";
    public const string HiraganaHi = "hiragana-hi";
    public const string HiraganaFu = "hiragana-fu";
    public const string HiraganaHe = "hiragana-he";
    public const string HiraganaHo = "hiragana-ho";
    public const string HiraganaMa = "hiragana-ma";
    public const string HiraganaMi = "hiragana-mi";
    public const string HiraganaMu = "hiragana-mu";
    public const string HiraganaMe = "hiragana-me";
    public const string HiraganaMo = "hiragana-mo";
    public const string HiraganaYa = "hiragana-ya";
    public const string HiraganaYu = "hiragana-yu";
    public const string HiraganaYo = "hiragana-yo";
    public const string HiraganaRa = "hiragana-ra";
    public const string HiraganaRi = "hiragana-ri";
    public const string HiraganaRu = "hiragana-ru";
    public const string HiraganaRe = "hiragana-re";
    public const string HiraganaRo = "hiragana-ro";
    public const string HiraganaWa = "hiragana-wa";
    public const string HiraganaWo = "hiragana-wo";
    public const string HiraganaN = "hiragana-n";
    public const string HiraganaGa = "hiragana-ga";
    public const string HiraganaGi = "hiragana-gi";
    public const string HiraganaGu = "hiragana-gu";
    public const string HiraganaGe = "hiragana-ge";
    public const string HiraganaGo = "hiragana-go";
    public const string HiraganaZa = "hiragana-za";
    public const string HiraganaJi = "hiragana-ji";
    public const string HiraganaZu = "hiragana-zu";
    public const string HiraganaZe = "hiragana-ze";
    public const string HiraganaZo = "hiragana-zo";
    public const string HiraganaDa = "hiragana-da";
    public const string HiraganaDe = "hiragana-de";
    public const string HiraganaDo = "hiragana-do";
    public const string HiraganaBa = "hiragana-ba";
    public const string HiraganaBi = "hiragana-bi";
    public const string HiraganaBu = "hiragana-bu";
    public const string HiraganaBe = "hiragana-be";
    public const string HiraganaBo = "hiragana-bo";
    public const string HiraganaPa = "hiragana-pa";
    public const string HiraganaPi = "hiragana-pi";
    public const string HiraganaPu = "hiragana-pu";
    public const string HiraganaPe = "hiragana-pe";
    public const string HiraganaPo = "hiragana-po";

    public const string KatakanaA = "katakana-a";
    public const string KatakanaI = "katakana-i";
    public const string KatakanaU = "katakana-u";
    public const string KatakanaE = "katakana-e";
    public const string KatakanaO = "katakana-o";
    public const string KatakanaKa = "katakana-ka";
    public const string KatakanaKi = "katakana-ki";
    public const string KatakanaKu = "katakana-ku";
    public const string KatakanaKe = "katakana-ke";
    public const string KatakanaKo = "katakana-ko";
    public const string KatakanaSa = "katakana-sa";
    public const string KatakanaShi = "katakana-shi";
    public const string KatakanaSu = "katakana-su";
    public const string KatakanaSe = "katakana-se";
    public const string KatakanaSo = "katakana-so";
    public const string KatakanaTa = "katakana-ta";
    public const string KatakanaChi = "katakana-chi";
    public const string KatakanaTsu = "katakana-tsu";
    public const string KatakanaTe = "katakana-te";
    public const string KatakanaTo = "katakana-to";
    public const string KatakanaNa = "katakana-na";
    public const string KatakanaNi = "katakana-ni";
    public const string KatakanaNu = "katakana-nu";
    public const string KatakanaNe = "katakana-ne";
    public const string KatakanaNo = "katakana-no";
    public const string KatakanaHa = "katakana-ha";
    public const string KatakanaHi = "katakana-hi";
    public const string KatakanaFu = "katakana-fu";
    public const string KatakanaHe = "katakana-he";
    public const string KatakanaHo = "katakana-ho";
    public const string KatakanaMa = "katakana-ma";
    public const string KatakanaMi = "katakana-mi";
    public const string KatakanaMu = "katakana-mu";
    public const string KatakanaMe = "katakana-me";
    public const string KatakanaMo = "katakana-mo";
    public const string KatakanaYa = "katakana-ya";
    public const string KatakanaYu = "katakana-yu";
    public const string KatakanaYo = "katakana-yo";
    public const string KatakanaRa = "katakana-ra";
    public const string KatakanaRi = "katakana-ri";
    public const string KatakanaRu = "katakana-ru";
    public const string KatakanaRe = "katakana-re";
    public const string KatakanaRo = "katakana-ro";
    public const string KatakanaWa = "katakana-wa";
    public const string KatakanaWo = "katakana-wo";
    public const string KatakanaN = "katakana-n";
    public const string KatakanaGa = "katakana-ga";
    public const string KatakanaGi = "katakana-gi";
    public const string KatakanaGu = "katakana-gu";
    public const string KatakanaGe = "katakana-ge";
    public const string KatakanaGo = "katakana-go";
    public const string KatakanaZa = "katakana-za";
    public const string KatakanaJi = "katakana-ji";
    public const string KatakanaZu = "katakana-zu";
    public const string KatakanaZe = "katakana-ze";
    public const string KatakanaZo = "katakana-zo";
    public const string KatakanaDa = "katakana-da";
    public const string KatakanaDe = "katakana-de";
    public const string KatakanaDo = "katakana-do";
    public const string KatakanaBa = "katakana-ba";
    public const string KatakanaBi = "katakana-bi";
    public const string KatakanaBu = "katakana-bu";
    public const string KatakanaBe = "katakana-be";
    public const string KatakanaBo = "katakana-bo";
    public const string KatakanaPa = "katakana-pa";
    public const string KatakanaPi = "katakana-pi";
    public const string KatakanaPu = "katakana-pu";
    public const string KatakanaPe = "katakana-pe";
    public const string KatakanaPo = "katakana-po";
}
