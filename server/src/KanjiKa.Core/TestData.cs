using KanjiKa.Core.Entities;
using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Kanji;
using KanjiKa.Core.Entities.Learning;
using KanjiKa.Core.Entities.Users;
using KanjiKa.Core.Services;

namespace KanjiKa.Core;

public static class TestData
{
    public static List<Character> GetKanaCharacters()
    {
        return
        [
            new Character
            {
                Symbol = "あ",
                Romanization = "a",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "あめ",
                        Romanization = "ame",
                        Meaning = "rain"
                    },

                    new Example
                    {
                        Word = "あさ",
                        Romanization = "asa",
                        Meaning = "morning"
                    }
                ]
            },

            new Character
            {
                Symbol = "い",
                Romanization = "i",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "いぬ",
                        Romanization = "inu",
                        Meaning = "dog"
                    }
                ]
            },

            new Character
            {
                Symbol = "う",
                Romanization = "u",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "うし",
                        Romanization = "ushi",
                        Meaning = "cow"
                    }
                ]
            },

            new Character
            {
                Symbol = "え",
                Romanization = "e",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "えき",
                        Romanization = "eki",
                        Meaning = "station"
                    }
                ]
            },

            new Character
            {
                Symbol = "お",
                Romanization = "o",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "おみず",
                        Romanization = "omizu",
                        Meaning = "water"
                    }
                ]
            },

            new Character
            {
                Symbol = "か",
                Romanization = "ka",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "かさ",
                        Romanization = "kasa",
                        Meaning = "umbrella"
                    }
                ]
            },

            new Character
            {
                Symbol = "き",
                Romanization = "ki",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "きく",
                        Romanization = "kiku",
                        Meaning = "to listen"
                    }
                ]
            },

            new Character
            {
                Symbol = "く",
                Romanization = "ku",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "くも",
                        Romanization = "kumo",
                        Meaning = "cloud"
                    }
                ]
            },

            new Character
            {
                Symbol = "け",
                Romanization = "ke",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "けん",
                        Romanization = "ken",
                        Meaning = "sword"
                    }
                ]
            },

            new Character
            {
                Symbol = "こ",
                Romanization = "ko",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "こい",
                        Romanization = "koi",
                        Meaning = "love"
                    }
                ]
            },

            new Character
            {
                Symbol = "さ",
                Romanization = "sa",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "さかな",
                        Romanization = "sakana",
                        Meaning = "fish"
                    }
                ]
            },

            new Character
            {
                Symbol = "し",
                Romanization = "shi",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "しあわせ",
                        Romanization = "shiawase",
                        Meaning = "happiness"
                    }
                ]
            },

            new Character
            {
                Symbol = "す",
                Romanization = "su",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "すし",
                        Romanization = "sushi",
                        Meaning = "sushi"
                    }
                ]
            },

            new Character
            {
                Symbol = "せ",
                Romanization = "se",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "せん",
                        Romanization = "sen",
                        Meaning = "thousand"
                    }
                ]
            },

            new Character
            {
                Symbol = "そ",
                Romanization = "so",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "そら",
                        Romanization = "sora",
                        Meaning = "sky"
                    }
                ]
            },

            new Character
            {
                Symbol = "た",
                Romanization = "ta",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "たこ",
                        Romanization = "tako",
                        Meaning = "octopus"
                    }
                ]
            },

            new Character
            {
                Symbol = "ち",
                Romanization = "chi",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ちいさい",
                        Romanization = "chiisai",
                        Meaning = "small"
                    }
                ]
            },

            new Character
            {
                Symbol = "つ",
                Romanization = "tsu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "つなみ",
                        Romanization = "tsunami",
                        Meaning = "tsunami"
                    }
                ]
            },

            new Character
            {
                Symbol = "て",
                Romanization = "te",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "てがみ",
                        Romanization = "tegami",
                        Meaning = "letter"
                    }
                ]
            },

            new Character
            {
                Symbol = "と",
                Romanization = "to",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "とうきょう",
                        Romanization = "toukyou",
                        Meaning = "Tokyo"
                    }
                ]
            },

            new Character
            {
                Symbol = "な",
                Romanization = "na",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "なつ",
                        Romanization = "natsu",
                        Meaning = "summer"
                    }
                ]
            },

            new Character
            {
                Symbol = "に",
                Romanization = "ni",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "にほん",
                        Romanization = "nihon",
                        Meaning = "Japan"
                    }
                ]
            },

            new Character
            {
                Symbol = "ぬ",
                Romanization = "nu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ぬき",
                        Romanization = "nuki",
                        Meaning = "pull out"
                    }
                ]
            },

            new Character
            {
                Symbol = "ね",
                Romanization = "ne",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ねこ",
                        Romanization = "neko",
                        Meaning = "cat"
                    }
                ]
            },

            new Character
            {
                Symbol = "の",
                Romanization = "no",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "のり",
                        Romanization = "nori",
                        Meaning = "seaweed"
                    }
                ]
            },

            new Character
            {
                Symbol = "は",
                Romanization = "ha",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "はな",
                        Romanization = "hana",
                        Meaning = "flower"
                    }
                ]
            },

            new Character
            {
                Symbol = "ひ",
                Romanization = "hi",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ひかり",
                        Romanization = "hikari",
                        Meaning = "light"
                    }
                ]
            },

            new Character
            {
                Symbol = "ふ",
                Romanization = "fu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ふね",
                        Romanization = "fune",
                        Meaning = "boat"
                    }
                ]
            },

            new Character
            {
                Symbol = "へ",
                Romanization = "he",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "へや",
                        Romanization = "heya",
                        Meaning = "room"
                    }
                ]
            },

            new Character
            {
                Symbol = "ほ",
                Romanization = "ho",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ほし",
                        Romanization = "hoshi",
                        Meaning = "star"
                    }
                ]
            },

            new Character
            {
                Symbol = "ま",
                Romanization = "ma",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "まち",
                        Romanization = "machi",
                        Meaning = "town"
                    }
                ]
            },

            new Character
            {
                Symbol = "み",
                Romanization = "mi",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "みず",
                        Romanization = "mizu",
                        Meaning = "water"
                    }
                ]
            },

            new Character
            {
                Symbol = "む",
                Romanization = "mu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "むし",
                        Romanization = "mushi",
                        Meaning = "insect"
                    }
                ]
            },

            new Character
            {
                Symbol = "め",
                Romanization = "me",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "めがね",
                        Romanization = "megane",
                        Meaning = "glasses"
                    }
                ]
            },

            new Character
            {
                Symbol = "も",
                Romanization = "mo",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "もり",
                        Romanization = "mori",
                        Meaning = "forest"
                    }
                ]
            },

            new Character
            {
                Symbol = "や",
                Romanization = "ya",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "やま",
                        Romanization = "yama",
                        Meaning = "mountain"
                    }
                ]
            },

            new Character
            {
                Symbol = "ゆ",
                Romanization = "yu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ゆき",
                        Romanization = "yuki",
                        Meaning = "snow"
                    }
                ]
            },

            new Character
            {
                Symbol = "よ",
                Romanization = "yo",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "よる",
                        Romanization = "yoru",
                        Meaning = "night"
                    }
                ]
            },

            new Character
            {
                Symbol = "ら",
                Romanization = "ra",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "らいん",
                        Romanization = "rain",
                        Meaning = "rain"
                    }
                ]
            },

            new Character
            {
                Symbol = "り",
                Romanization = "ri",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "りんぐ",
                        Romanization = "ringu",
                        Meaning = "ring"
                    }
                ]
            },

            new Character
            {
                Symbol = "る",
                Romanization = "ru",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "るむ",
                        Romanization = "rumu",
                        Meaning = "room"
                    }
                ]
            },

            new Character
            {
                Symbol = "れ",
                Romanization = "re",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "れんたる",
                        Romanization = "rentaru",
                        Meaning = "rental"
                    }
                ]
            },

            new Character
            {
                Symbol = "ろ",
                Romanization = "ro",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ろぼっと",
                        Romanization = "robotto",
                        Meaning = "robot"
                    }
                ]
            },

            new Character
            {
                Symbol = "わ",
                Romanization = "wa",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "わたし",
                        Romanization = "watashi",
                        Meaning = "I"
                    }
                ]
            },

            new Character
            {
                Symbol = "を",
                Romanization = "wo",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "をん",
                        Romanization = "won",
                        Meaning = "won"
                    }
                ]
            },

            new Character
            {
                Symbol = "ん",
                Romanization = "n",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "にほん",
                        Romanization = "nihon",
                        Meaning = "Japan"
                    }
                ]
            },

            new Character
            {
                Symbol = "が",
                Romanization = "ga",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "がくせい",
                        Romanization = "gakusei",
                        Meaning = "student"
                    }
                ]
            },

            new Character
            {
                Symbol = "ぎ",
                Romanization = "gi",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ぎん",
                        Romanization = "gin",
                        Meaning = "silver"
                    }
                ]
            },

            new Character
            {
                Symbol = "ぐ",
                Romanization = "gu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ぐみ",
                        Romanization = "gumi",
                        Meaning = "group"
                    }
                ]
            },

            new Character
            {
                Symbol = "げ",
                Romanization = "ge",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "げーむ",
                        Romanization = "geemu",
                        Meaning = "game"
                    }
                ]
            },

            new Character
            {
                Symbol = "ご",
                Romanization = "go",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ごはん",
                        Romanization = "gohan",
                        Meaning = "rice"
                    }
                ]
            },

            new Character
            {
                Symbol = "ざ",
                Romanization = "za",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ざっし",
                        Romanization = "zasshi",
                        Meaning = "magazine"
                    }
                ]
            },

            new Character
            {
                Symbol = "じ",
                Romanization = "ji",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "じかん",
                        Romanization = "jikan",
                        Meaning = "time"
                    }
                ]
            },

            new Character
            {
                Symbol = "ず",
                Romanization = "zu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ずぼん",
                        Romanization = "zubon",
                        Meaning = "trousers"
                    }
                ]
            },

            new Character
            {
                Symbol = "ぜ",
                Romanization = "ze",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ぜんぶ",
                        Romanization = "zenbu",
                        Meaning = "all"
                    }
                ]
            },

            new Character
            {
                Symbol = "ぞ",
                Romanization = "zo",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ぞう",
                        Romanization = "zou",
                        Meaning = "elephant"
                    }
                ]
            },

            new Character
            {
                Symbol = "だ",
                Romanization = "da",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "だんご",
                        Romanization = "dango",
                        Meaning = "dumpling"
                    }
                ]
            },

            new Character
            {
                Symbol = "で",
                Romanization = "de",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "でんわ",
                        Romanization = "denwa",
                        Meaning = "telephone"
                    }
                ]
            },

            new Character
            {
                Symbol = "ど",
                Romanization = "do",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "どらいぶ",
                        Romanization = "doraibu",
                        Meaning = "drive"
                    }
                ]
            },

            new Character
            {
                Symbol = "ば",
                Romanization = "ba",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ばす",
                        Romanization = "basu",
                        Meaning = "bus"
                    }
                ]
            },

            new Character
            {
                Symbol = "び",
                Romanization = "bi",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "びる",
                        Romanization = "biru",
                        Meaning = "beer"
                    }
                ]
            },

            new Character
            {
                Symbol = "ぶ",
                Romanization = "bu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ぶどう",
                        Romanization = "budou",
                        Meaning = "grape"
                    }
                ]
            },

            new Character
            {
                Symbol = "べ",
                Romanization = "be",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "べんきょう",
                        Romanization = "benkyou",
                        Meaning = "study"
                    }
                ]
            },

            new Character
            {
                Symbol = "ぼ",
                Romanization = "bo",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ぼく",
                        Romanization = "boku",
                        Meaning = "I"
                    }
                ]
            },

            new Character
            {
                Symbol = "ぱ",
                Romanization = "pa",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ぱちんこ",
                        Romanization = "pachinko",
                        Meaning = "pachinko"
                    }
                ]
            },

            new Character
            {
                Symbol = "ぴ",
                Romanization = "pi",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ぴかちゅう",
                        Romanization = "pikachuu",
                        Meaning = "Pikachu"
                    }
                ]
            },

            new Character
            {
                Symbol = "ぷ",
                Romanization = "pu",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ぷりん",
                        Romanization = "purin",
                        Meaning = "pudding"
                    }
                ]
            },

            new Character
            {
                Symbol = "ぺ",
                Romanization = "pe",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ぺん",
                        Romanization = "pen",
                        Meaning = "pen"
                    }
                ]
            },

            new Character
            {
                Symbol = "ぽ",
                Romanization = "po",
                Type = KanaType.Hiragana,
                Examples =
                [
                    new Example
                    {
                        Word = "ぽけもん",
                        Romanization = "pokemon",
                        Meaning = "Pokemon"
                    }
                ]
            },
            //TODO Add yoon characters


            new Character
            {
                Symbol = "ア",
                Romanization = "a",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "アニメ",
                        Romanization = "anime",
                        Meaning = "animation"
                    }
                ]
            },

            new Character
            {
                Symbol = "イ",
                Romanization = "i",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "イクラ",
                        Romanization = "ikura",
                        Meaning = "salmon roe"
                    }
                ]
            },

            new Character
            {
                Symbol = "ウ",
                Romanization = "u",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ウニ",
                        Romanization = "uni",
                        Meaning = "sea urchin"
                    }
                ]
            },

            new Character
            {
                Symbol = "エ",
                Romanization = "e",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "エビ",
                        Romanization = "ebi",
                        Meaning = "shrimp"
                    }
                ]
            },

            new Character
            {
                Symbol = "オ",
                Romanization = "o",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "オムライス",
                        Romanization = "omuraisu",
                        Meaning = "omelette rice"
                    }
                ]
            },

            new Character
            {
                Symbol = "カ",
                Romanization = "ka",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "カメラ",
                        Romanization = "kamera",
                        Meaning = "camera"
                    }
                ]
            },

            new Character
            {
                Symbol = "キ",
                Romanization = "ki",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "キロ",
                        Romanization = "kiro",
                        Meaning = "kilometer"
                    }
                ]
            },

            new Character
            {
                Symbol = "ク",
                Romanization = "ku",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "クモ",
                        Romanization = "kumo",
                        Meaning = "spider"
                    }
                ]
            },

            new Character
            {
                Symbol = "ケ",
                Romanization = "ke",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ケン",
                        Romanization = "ken",
                        Meaning = "sword"
                    }
                ]
            },

            new Character
            {
                Symbol = "コ",
                Romanization = "ko",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "コイ",
                        Romanization = "koi",
                        Meaning = "carp"
                    }
                ]
            },

            new Character
            {
                Symbol = "サ",
                Romanization = "sa",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "サクラ",
                        Romanization = "sakura",
                        Meaning = "cherry blossom"
                    }
                ]
            },

            new Character
            {
                Symbol = "シ",
                Romanization = "shi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "シンブン",
                        Romanization = "shinbun",
                        Meaning = "newspaper"
                    }
                ]
            },

            new Character
            {
                Symbol = "ス",
                Romanization = "su",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "スシ",
                        Romanization = "sushi",
                        Meaning = "sushi"
                    }
                ]
            },

            new Character
            {
                Symbol = "セ",
                Romanization = "se",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "セン",
                        Romanization = "sen",
                        Meaning = "thousand"
                    }
                ]
            },

            new Character
            {
                Symbol = "ソ",
                Romanization = "so",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ソバ",
                        Romanization = "soba",
                        Meaning = "buckwheat noodles"
                    }
                ]
            },

            new Character
            {
                Symbol = "タ",
                Romanization = "ta",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "タコヤキ",
                        Romanization = "takoyaki",
                        Meaning = "octopus balls"
                    }
                ]
            },

            new Character
            {
                Symbol = "チ",
                Romanization = "chi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "チーズ",
                        Romanization = "chiizu",
                        Meaning = "cheese"
                    }
                ]
            },

            new Character
            {
                Symbol = "ツ",
                Romanization = "tsu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ツキ",
                        Romanization = "tsuki",
                        Meaning = "moon"
                    }
                ]
            },

            new Character
            {
                Symbol = "テ",
                Romanization = "te",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "テニス",
                        Romanization = "tenisu",
                        Meaning = "tennis"
                    }
                ]
            },

            new Character
            {
                Symbol = "ト",
                Romanization = "to",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "トウキョウ",
                        Romanization = "toukyou",
                        Meaning = "Tokyo"
                    }
                ]
            },

            new Character
            {
                Symbol = "ナ",
                Romanization = "na",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ナツ",
                        Romanization = "natsu",
                        Meaning = "summer"
                    }
                ]
            },

            new Character
            {
                Symbol = "ニ",
                Romanization = "ni",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ニホン",
                        Romanization = "nihon",
                        Meaning = "Japan"
                    }
                ]
            },

            new Character
            {
                Symbol = "ヌ",
                Romanization = "nu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ヌキ",
                        Romanization = "nuki",
                        Meaning = "pull out"
                    }
                ]
            },

            new Character
            {
                Symbol = "ネ",
                Romanization = "ne",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ネコ",
                        Romanization = "neko",
                        Meaning = "cat"
                    }
                ]
            },

            new Character
            {
                Symbol = "ノ",
                Romanization = "no",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ノリ",
                        Romanization = "nori",
                        Meaning = "seaweed"
                    }
                ]
            },

            new Character
            {
                Symbol = "ハ",
                Romanization = "ha",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ハナビ",
                        Romanization = "hanabi",
                        Meaning = "fireworks"
                    }
                ]
            },

            new Character
            {
                Symbol = "ヒ",
                Romanization = "hi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ヒコウキ",
                        Romanization = "hikouki",
                        Meaning = "airplane"
                    }
                ]
            },

            new Character
            {
                Symbol = "フ",
                Romanization = "fu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "フジ",
                        Romanization = "fuji",
                        Meaning = "Mt. Fuji"
                    }
                ]
            },

            new Character
            {
                Symbol = "ヘ",
                Romanization = "he",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ヘンシン",
                        Romanization = "henshin",
                        Meaning = "transformation"
                    }
                ]
            },

            new Character
            {
                Symbol = "ホ",
                Romanization = "ho",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ホン",
                        Romanization = "hon",
                        Meaning = "book"
                    }
                ]
            },

            new Character
            {
                Symbol = "マ",
                Romanization = "ma",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "マイク",
                        Romanization = "maiku",
                        Meaning = "microphone"
                    }
                ]
            },

            new Character
            {
                Symbol = "ミ",
                Romanization = "mi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ミソ",
                        Romanization = "miso",
                        Meaning = "miso"
                    }
                ]
            },

            new Character
            {
                Symbol = "ム",
                Romanization = "mu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ムシ",
                        Romanization = "mushi",
                        Meaning = "insect"
                    }
                ]
            },

            new Character
            {
                Symbol = "メ",
                Romanization = "me",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "メガネ",
                        Romanization = "megane",
                        Meaning = "glasses"
                    }
                ]
            },

            new Character
            {
                Symbol = "モ",
                Romanization = "mo",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "モノ",
                        Romanization = "mono",
                        Meaning = "thing"
                    }
                ]
            },

            new Character
            {
                Symbol = "ヤ",
                Romanization = "ya",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ヤキトリ",
                        Romanization = "yakitori",
                        Meaning = "grilled chicken"
                    }
                ]
            },

            new Character
            {
                Symbol = "ユ",
                Romanization = "yu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ユキ",
                        Romanization = "yuki",
                        Meaning = "snow"
                    }
                ]
            },

            new Character
            {
                Symbol = "ヨ",
                Romanization = "yo",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ヨガ",
                        Romanization = "yoga",
                        Meaning = "yoga"
                    }
                ]
            },

            new Character
            {
                Symbol = "ラ",
                Romanization = "ra",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ラーメン",
                        Romanization = "raamen",
                        Meaning = "ramen"
                    }
                ]
            },

            new Character
            {
                Symbol = "リ",
                Romanization = "ri",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ラムネ",
                        Romanization = "ramune",
                        Meaning = "ramune"
                    }
                ]
            },

            new Character
            {
                Symbol = "ル",
                Romanization = "ru",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "リンゴ",
                        Romanization = "ringo",
                        Meaning = "apple"
                    }
                ]
            },

            new Character
            {
                Symbol = "レ",
                Romanization = "re",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "レイゾウコ",
                        Romanization = "reizouko",
                        Meaning = "refrigerator"
                    }
                ]
            },

            new Character
            {
                Symbol = "ロ",
                Romanization = "ro",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ロボット",
                        Romanization = "robotto",
                        Meaning = "robot"
                    }
                ]
            },

            new Character
            {
                Symbol = "ワ",
                Romanization = "wa",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ワンパク",
                        Romanization = "wanpaku",
                        Meaning = "naughty"
                    }
                ]
            },

            new Character
            {
                Symbol = "ヲ",
                Romanization = "wo",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ヲタゲイ",
                        Romanization = "wotagei",
                        Meaning = "otaku dance"
                    }
                ]
            },

            new Character
            {
                Symbol = "ン",
                Romanization = "n",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ニンジャ",
                        Romanization = "ninja",
                        Meaning = "ninja"
                    }
                ]
            },

            new Character
            {
                Symbol = "ガ",
                Romanization = "ga",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ガッコウ",
                        Romanization = "gakkou",
                        Meaning = "school"
                    }
                ]
            },

            new Character
            {
                Symbol = "ギ",
                Romanization = "gi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ギン",
                        Romanization = "gin",
                        Meaning = "silver"
                    }
                ]
            },

            new Character
            {
                Symbol = "グ",
                Romanization = "gu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ガンダム",
                        Romanization = "gandamu",
                        Meaning = "Gundam"
                    }
                ]
            },

            new Character
            {
                Symbol = "ゲ",
                Romanization = "ge",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ゲーム",
                        Romanization = "geemu",
                        Meaning = "game"
                    }
                ]
            },

            new Character
            {
                Symbol = "ゴ",
                Romanization = "go",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ゴハン",
                        Romanization = "gohan",
                        Meaning = "rice"
                    }
                ]
            },

            new Character
            {
                Symbol = "ザ",
                Romanization = "za",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ザッシ",
                        Romanization = "zasshi",
                        Meaning = "magazine"
                    }
                ]
            },

            new Character
            {
                Symbol = "ジ",
                Romanization = "ji",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ジンルイ",
                        Romanization = "jinrui",
                        Meaning = "humanity"
                    }
                ]
            },

            new Character
            {
                Symbol = "ズ",
                Romanization = "zu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ズット",
                        Romanization = "zutto",
                        Meaning = "always"
                    }
                ]
            },

            new Character
            {
                Symbol = "ゼ",
                Romanization = "ze",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ゼンブ",
                        Romanization = "zenbu",
                        Meaning = "all"
                    }
                ]
            },

            new Character
            {
                Symbol = "ゾ",
                Romanization = "zo",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ゾウ",
                        Romanization = "zou",
                        Meaning = "elephant"
                    }
                ]
            },

            new Character
            {
                Symbol = "ダ",
                Romanization = "da",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ダイスキ",
                        Romanization = "daisuki",
                        Meaning = "love"
                    }
                ]
            },

            new Character
            {
                Symbol = "デ",
                Romanization = "de",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "デカイ",
                        Romanization = "dekai",
                        Meaning = "big"
                    }
                ]
            },

            new Character
            {
                Symbol = "ド",
                Romanization = "do",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ドラエモン",
                        Romanization = "doraemon",
                        Meaning = "Doraemon"
                    }
                ]
            },

            new Character
            {
                Symbol = "バ",
                Romanization = "ba",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "バナナ",
                        Romanization = "banana",
                        Meaning = "banana"
                    }
                ]
            },

            new Character
            {
                Symbol = "ビ",
                Romanization = "bi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ビール",
                        Romanization = "biiru",
                        Meaning = "beer"
                    }
                ]
            },

            new Character
            {
                Symbol = "ブ",
                Romanization = "bu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "バス",
                        Romanization = "basu",
                        Meaning = "bus"
                    }
                ]
            },

            new Character
            {
                Symbol = "ベ",
                Romanization = "be",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ベンリ",
                        Romanization = "benri",
                        Meaning = "convenient"
                    }
                ]
            },

            new Character
            {
                Symbol = "ボ",
                Romanization = "bo",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ボク",
                        Romanization = "boku",
                        Meaning = "I"
                    }
                ]
            },

            new Character
            {
                Symbol = "パ",
                Romanization = "pa",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ピアノ",
                        Romanization = "piano",
                        Meaning = "piano"
                    }
                ]
            },

            new Character
            {
                Symbol = "ピ",
                Romanization = "pi",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ピカチュウ",
                        Romanization = "pikachuu",
                        Meaning = "Pikachu"
                    }
                ]
            },

            new Character
            {
                Symbol = "プ",
                Romanization = "pu",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "プリン",
                        Romanization = "purin",
                        Meaning = "pudding"
                    }
                ]
            },

            new Character
            {
                Symbol = "ペ",
                Romanization = "pe",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
                        Word = "ペン",
                        Romanization = "pen",
                        Meaning = "pen"
                    }
                ]
            },

            new Character
            {
                Symbol = "ポ",
                Romanization = "po",
                Type = KanaType.Katakana,
                Examples =
                [
                    new Example
                    {
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
        const string testPassword = "almafa123";
        var hashService = new HashService();
        var (hash, salt) = hashService.Hash(testPassword);
        return
        [
            new User
            {
                Id = 1,
                Username = "testuser1@kanjika.com",
                PasswordHash = hash,
                PasswordSalt = salt,
                Proficiencies =
                [
                    new Proficiency
                    {
                        Id = 1,
                        UserId = 1,
                        CharacterId = 1,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-10),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },

                    new Proficiency
                    {
                        Id = 2,
                        UserId = 1,
                        CharacterId = 2,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-9),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-2)
                    },
                    new Proficiency
                    {
                        Id = 3,
                        UserId = 1,
                        CharacterId = 3,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-8),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 4,
                        UserId = 1,
                        CharacterId = 4,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-7),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 5,
                        UserId = 1,
                        CharacterId = 5,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-6),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 6,
                        UserId = 1,
                        CharacterId = 6,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-5),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 7,
                        UserId = 1,
                        CharacterId = 7,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-4),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 8,
                        UserId = 1,
                        CharacterId = 8,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-3),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 9,
                        UserId = 1,
                        CharacterId = 9,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-2),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 10,
                        UserId = 1,
                        CharacterId = 10,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 11,
                        UserId = 1,
                        CharacterId = 11,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 12,
                        UserId = 1,
                        CharacterId = 12,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 13,
                        UserId = 1,
                        CharacterId = 13,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 14,
                        UserId = 1,
                        CharacterId = 14,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 15,
                        UserId = 1,
                        CharacterId = 15,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 16,
                        UserId = 1,
                        CharacterId = 16,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 17,
                        UserId = 1,
                        CharacterId = 17,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-20),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-10)
                    },
                    new Proficiency
                    {
                        Id = 18,
                        UserId = 1,
                        CharacterId = 18,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-30),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-20)
                    },
                    new Proficiency
                    {
                        Id = 19,
                        UserId = 1,
                        CharacterId = 19,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-40),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-30)
                    },
                    new Proficiency
                    {
                        Id = 20,
                        UserId = 1,
                        CharacterId = 20,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-50),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-40)
                    },
                    new Proficiency
                    {
                        Id = 21,
                        UserId = 1,
                        CharacterId = 21,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-60),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-50)
                    },
                    new Proficiency
                    {
                        Id = 22,
                        UserId = 1,
                        CharacterId = 22,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-20),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-10)
                    },
                    new Proficiency
                    {
                        Id = 23,
                        UserId = 1,
                        CharacterId = 23,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-20),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-10)
                    },
                    new Proficiency
                    {
                        Id = 24,
                        UserId = 1,
                        CharacterId = 24,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-2),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 25,
                        UserId = 1,
                        CharacterId = 25,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-2),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 26,
                        UserId = 1,
                        CharacterId = 26,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 27,
                        UserId = 1,
                        CharacterId = 27,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 28,
                        UserId = 1,
                        CharacterId = 28,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 29,
                        UserId = 1,
                        CharacterId = 29,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 30,
                        UserId = 1,
                        CharacterId = 30,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 31,
                        UserId = 1,
                        CharacterId = 31,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-2),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 32,
                        UserId = 1,
                        CharacterId = 32,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 33,
                        UserId = 1,
                        CharacterId = 33,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 34,
                        UserId = 1,
                        CharacterId = 34,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 35,
                        UserId = 1,
                        CharacterId = 35,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 36,
                        UserId = 1,
                        CharacterId = 36,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 37,
                        UserId = 1,
                        CharacterId = 37,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 38,
                        UserId = 1,
                        CharacterId = 38,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 39,
                        UserId = 1,
                        CharacterId = 39,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 40,
                        UserId = 1,
                        CharacterId = 40,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 41,
                        UserId = 1,
                        CharacterId = 41,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 42,
                        UserId = 1,
                        CharacterId = 42,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 43,
                        UserId = 1,
                        CharacterId = 43,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 44,
                        UserId = 1,
                        CharacterId = 44,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 45,
                        UserId = 1,
                        CharacterId = 45,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 46,
                        UserId = 1,
                        CharacterId = 46,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 47,
                        UserId = 1,
                        CharacterId = 47,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 48,
                        UserId = 1,
                        CharacterId = 48,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 49,
                        UserId = 1,
                        CharacterId = 49,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 50,
                        UserId = 1,
                        CharacterId = 50,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 51,
                        UserId = 1,
                        CharacterId = 51,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 52,
                        UserId = 1,
                        CharacterId = 52,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 53,
                        UserId = 1,
                        CharacterId = 53,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 54,
                        UserId = 1,
                        CharacterId = 54,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 55,
                        UserId = 1,
                        CharacterId = 55,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 56,
                        UserId = 1,
                        CharacterId = 56,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 57,
                        UserId = 1,
                        CharacterId = 57,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 58,
                        UserId = 1,
                        CharacterId = 58,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 59,
                        UserId = 1,
                        CharacterId = 59,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 60,
                        UserId = 1,
                        CharacterId = 60,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 61,
                        UserId = 1,
                        CharacterId = 61,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    }
                ],
                LessonCompletions = [
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 1,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-100),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 2,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-99),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 3,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-98),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 4,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-97),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 5,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-96),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 6,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-95),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 7,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-94),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 8,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-93),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 9,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-92),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 10,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-91),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 11,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-90),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 12,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-89),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 13,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-88),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 14,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-87),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 15,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-86),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 16,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-85),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 17,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-84),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 18,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-83),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 19,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-82),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 20,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-81),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 21,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-80),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 22,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-79),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 23,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-78),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 24,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-77),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 25,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-76),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 26,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-75),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 27,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-74),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 28,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-73),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 29,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-72),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 30,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-71),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 31,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-70),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 32,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-69),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 33,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-68),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 34,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-67),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 35,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-66),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 36,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-65),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 37,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-64),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 38,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-63),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 39,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-62),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 40,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-61),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 41,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-60),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 42,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-59),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 43,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-58),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 44,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-57),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 45,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-56),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 46,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-55),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 47,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-54),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 48,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-53),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 49,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-52),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 50,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-51),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 51,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-50),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 52,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-49),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 53,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-48),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 54,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-47),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 55,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-46),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 56,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-45),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 57,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-44),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 58,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-43),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 59,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-42),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 60,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-41),
                    },
                    new LessonCompletion
                    {
                        UserId = 1,
                        CharacterId = 61,
                        CompletionDate = DateTimeOffset.UtcNow.AddDays(-40),
                    }
                ]
            }
        ];
    }

    public static IEnumerable<Kanji> GetKanjiData() =>
    [
        new() { Character = "一", Meaning = "one", OnyomiReading = "イチ, イツ", KunyomiReading = "ひと-", StrokeCount = 1, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "一月", Reading = "いちがつ", Meaning = "January" }, new() { Word = "一日", Reading = "いちにち", Meaning = "one day" } ] },
        new() { Character = "二", Meaning = "two", OnyomiReading = "ニ", KunyomiReading = "ふた-", StrokeCount = 2, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "二月", Reading = "にがつ", Meaning = "February" }, new() { Word = "二人", Reading = "ふたり", Meaning = "two people" } ] },
        new() { Character = "三", Meaning = "three", OnyomiReading = "サン", KunyomiReading = "み-", StrokeCount = 3, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "三月", Reading = "さんがつ", Meaning = "March" } ] },
        new() { Character = "四", Meaning = "four", OnyomiReading = "シ", KunyomiReading = "よ-, よっ-", StrokeCount = 5, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "四月", Reading = "しがつ", Meaning = "April" } ] },
        new() { Character = "五", Meaning = "five", OnyomiReading = "ゴ", KunyomiReading = "いつ-", StrokeCount = 4, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "五月", Reading = "ごがつ", Meaning = "May" } ] },
        new() { Character = "六", Meaning = "six", OnyomiReading = "ロク", KunyomiReading = "む-", StrokeCount = 4, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "六月", Reading = "ろくがつ", Meaning = "June" } ] },
        new() { Character = "七", Meaning = "seven", OnyomiReading = "シチ", KunyomiReading = "なな-", StrokeCount = 2, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "七月", Reading = "しちがつ", Meaning = "July" } ] },
        new() { Character = "八", Meaning = "eight", OnyomiReading = "ハチ", KunyomiReading = "や-", StrokeCount = 2, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "八月", Reading = "はちがつ", Meaning = "August" } ] },
        new() { Character = "九", Meaning = "nine", OnyomiReading = "キュウ, ク", KunyomiReading = "ここの-", StrokeCount = 2, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "九月", Reading = "くがつ", Meaning = "September" } ] },
        new() { Character = "十", Meaning = "ten", OnyomiReading = "ジュウ", KunyomiReading = "とお", StrokeCount = 2, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "十月", Reading = "じゅうがつ", Meaning = "October" } ] },
        new() { Character = "百", Meaning = "hundred", OnyomiReading = "ヒャク", KunyomiReading = "", StrokeCount = 6, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "百円", Reading = "ひゃくえん", Meaning = "100 yen" } ] },
        new() { Character = "千", Meaning = "thousand", OnyomiReading = "セン", KunyomiReading = "ち", StrokeCount = 3, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "千円", Reading = "せんえん", Meaning = "1000 yen" } ] },
        new() { Character = "万", Meaning = "ten thousand", OnyomiReading = "マン, バン", KunyomiReading = "", StrokeCount = 3, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "一万円", Reading = "いちまんえん", Meaning = "10,000 yen" } ] },
        new() { Character = "円", Meaning = "yen, circle", OnyomiReading = "エン", KunyomiReading = "まる", StrokeCount = 4, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "円", Reading = "えん", Meaning = "yen" } ] },
        new() { Character = "年", Meaning = "year", OnyomiReading = "ネン", KunyomiReading = "とし", StrokeCount = 6, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "今年", Reading = "ことし", Meaning = "this year" } ] },
        new() { Character = "月", Meaning = "moon, month", OnyomiReading = "ゲツ, ガツ", KunyomiReading = "つき", StrokeCount = 4, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "月曜日", Reading = "げつようび", Meaning = "Monday" } ] },
        new() { Character = "日", Meaning = "sun, day", OnyomiReading = "ニチ, ジツ", KunyomiReading = "ひ, か", StrokeCount = 4, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "日曜日", Reading = "にちようび", Meaning = "Sunday" } ] },
        new() { Character = "火", Meaning = "fire", OnyomiReading = "カ", KunyomiReading = "ひ", StrokeCount = 4, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "火曜日", Reading = "かようび", Meaning = "Tuesday" } ] },
        new() { Character = "水", Meaning = "water", OnyomiReading = "スイ", KunyomiReading = "みず", StrokeCount = 4, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "水曜日", Reading = "すいようび", Meaning = "Wednesday" }, new() { Word = "水", Reading = "みず", Meaning = "water" } ] },
        new() { Character = "木", Meaning = "tree, wood", OnyomiReading = "モク, ボク", KunyomiReading = "き", StrokeCount = 4, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "木曜日", Reading = "もくようび", Meaning = "Thursday" } ] },
        new() { Character = "金", Meaning = "gold, money", OnyomiReading = "キン, コン", KunyomiReading = "かね", StrokeCount = 8, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "金曜日", Reading = "きんようび", Meaning = "Friday" }, new() { Word = "お金", Reading = "おかね", Meaning = "money" } ] },
        new() { Character = "土", Meaning = "earth, soil", OnyomiReading = "ド, ト", KunyomiReading = "つち", StrokeCount = 3, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "土曜日", Reading = "どようび", Meaning = "Saturday" } ] },
        new() { Character = "山", Meaning = "mountain", OnyomiReading = "サン", KunyomiReading = "やま", StrokeCount = 3, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "山", Reading = "やま", Meaning = "mountain" }, new() { Word = "富士山", Reading = "ふじさん", Meaning = "Mt. Fuji" } ] },
        new() { Character = "川", Meaning = "river", OnyomiReading = "セン", KunyomiReading = "かわ", StrokeCount = 3, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "川", Reading = "かわ", Meaning = "river" } ] },
        new() { Character = "田", Meaning = "rice field", OnyomiReading = "デン", KunyomiReading = "た", StrokeCount = 5, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "田んぼ", Reading = "たんぼ", Meaning = "rice paddy" } ] },
        new() { Character = "天", Meaning = "heaven, sky", OnyomiReading = "テン", KunyomiReading = "あめ, あま", StrokeCount = 4, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "天気", Reading = "てんき", Meaning = "weather" } ] },
        new() { Character = "人", Meaning = "person", OnyomiReading = "ジン, ニン", KunyomiReading = "ひと", StrokeCount = 2, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "人", Reading = "ひと", Meaning = "person" }, new() { Word = "日本人", Reading = "にほんじん", Meaning = "Japanese person" } ] },
        new() { Character = "子", Meaning = "child", OnyomiReading = "シ, ス", KunyomiReading = "こ", StrokeCount = 3, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "子供", Reading = "こども", Meaning = "child" } ] },
        new() { Character = "女", Meaning = "woman", OnyomiReading = "ジョ, ニョ", KunyomiReading = "おんな", StrokeCount = 3, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "女性", Reading = "じょせい", Meaning = "woman, female" } ] },
        new() { Character = "男", Meaning = "man", OnyomiReading = "ダン, ナン", KunyomiReading = "おとこ", StrokeCount = 7, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "男性", Reading = "だんせい", Meaning = "man, male" } ] },
        new() { Character = "大", Meaning = "big, great", OnyomiReading = "ダイ, タイ", KunyomiReading = "おお-", StrokeCount = 3, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "大学", Reading = "だいがく", Meaning = "university" }, new() { Word = "大きい", Reading = "おおきい", Meaning = "big" } ] },
        new() { Character = "小", Meaning = "small", OnyomiReading = "ショウ", KunyomiReading = "ちい-, こ-", StrokeCount = 3, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "小学校", Reading = "しょうがっこう", Meaning = "elementary school" } ] },
        new() { Character = "中", Meaning = "inside, middle", OnyomiReading = "チュウ", KunyomiReading = "なか", StrokeCount = 4, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "中学校", Reading = "ちゅうがっこう", Meaning = "middle school" }, new() { Word = "中国", Reading = "ちゅうごく", Meaning = "China" } ] },
        new() { Character = "上", Meaning = "above, up", OnyomiReading = "ジョウ, ショウ", KunyomiReading = "うえ, うわ, かみ", StrokeCount = 3, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "上", Reading = "うえ", Meaning = "above" } ] },
        new() { Character = "下", Meaning = "below, down", OnyomiReading = "カ, ゲ", KunyomiReading = "した, しも, くだ-", StrokeCount = 3, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "下", Reading = "した", Meaning = "below" } ] },
        new() { Character = "左", Meaning = "left", OnyomiReading = "サ", KunyomiReading = "ひだり", StrokeCount = 5, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "左", Reading = "ひだり", Meaning = "left" } ] },
        new() { Character = "右", Meaning = "right", OnyomiReading = "ウ, ユウ", KunyomiReading = "みぎ", StrokeCount = 5, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "右", Reading = "みぎ", Meaning = "right" } ] },
        new() { Character = "東", Meaning = "east", OnyomiReading = "トウ", KunyomiReading = "ひがし", StrokeCount = 8, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "東京", Reading = "とうきょう", Meaning = "Tokyo" }, new() { Word = "東", Reading = "ひがし", Meaning = "east" } ] },
        new() { Character = "西", Meaning = "west", OnyomiReading = "セイ, サイ", KunyomiReading = "にし", StrokeCount = 6, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "西", Reading = "にし", Meaning = "west" } ] },
        new() { Character = "南", Meaning = "south", OnyomiReading = "ナン, ナ", KunyomiReading = "みなみ", StrokeCount = 9, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "南", Reading = "みなみ", Meaning = "south" } ] },
        new() { Character = "北", Meaning = "north", OnyomiReading = "ホク", KunyomiReading = "きた", StrokeCount = 5, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "北", Reading = "きた", Meaning = "north" } ] },
        new() { Character = "口", Meaning = "mouth", OnyomiReading = "コウ, ク", KunyomiReading = "くち", StrokeCount = 3, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "口", Reading = "くち", Meaning = "mouth" } ] },
        new() { Character = "目", Meaning = "eye", OnyomiReading = "モク, ボク", KunyomiReading = "め", StrokeCount = 5, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "目", Reading = "め", Meaning = "eye" } ] },
        new() { Character = "耳", Meaning = "ear", OnyomiReading = "ジ", KunyomiReading = "みみ", StrokeCount = 6, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "耳", Reading = "みみ", Meaning = "ear" } ] },
        new() { Character = "手", Meaning = "hand", OnyomiReading = "シュ, ズ", KunyomiReading = "て, た-", StrokeCount = 4, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "手", Reading = "て", Meaning = "hand" } ] },
        new() { Character = "足", Meaning = "foot, leg", OnyomiReading = "ソク", KunyomiReading = "あし", StrokeCount = 7, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "足", Reading = "あし", Meaning = "foot, leg" } ] },
        new() { Character = "力", Meaning = "power, strength", OnyomiReading = "リョク, リキ", KunyomiReading = "ちから", StrokeCount = 2, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "力", Reading = "ちから", Meaning = "power, strength" } ] },
        new() { Character = "気", Meaning = "spirit, energy", OnyomiReading = "キ, ケ", KunyomiReading = "", StrokeCount = 6, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "天気", Reading = "てんき", Meaning = "weather" }, new() { Word = "元気", Reading = "げんき", Meaning = "healthy, energetic" } ] },
        new() { Character = "学", Meaning = "learn, study", OnyomiReading = "ガク", KunyomiReading = "まな-", StrokeCount = 8, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "学校", Reading = "がっこう", Meaning = "school" }, new() { Word = "大学", Reading = "だいがく", Meaning = "university" } ] },
        new() { Character = "校", Meaning = "school", OnyomiReading = "コウ", KunyomiReading = "", StrokeCount = 10, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "学校", Reading = "がっこう", Meaning = "school" } ] },
        new() { Character = "先", Meaning = "ahead, previous", OnyomiReading = "セン", KunyomiReading = "さき", StrokeCount = 6, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "先生", Reading = "せんせい", Meaning = "teacher" } ] },
        new() { Character = "生", Meaning = "life, birth", OnyomiReading = "セイ, ショウ", KunyomiReading = "い-, う-, なま", StrokeCount = 5, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "先生", Reading = "せんせい", Meaning = "teacher" }, new() { Word = "学生", Reading = "がくせい", Meaning = "student" } ] },
        new() { Character = "本", Meaning = "book, origin", OnyomiReading = "ホン", KunyomiReading = "もと", StrokeCount = 5, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "本", Reading = "ほん", Meaning = "book" }, new() { Word = "日本", Reading = "にほん", Meaning = "Japan" } ] },
        new() { Character = "語", Meaning = "language, word", OnyomiReading = "ゴ", KunyomiReading = "かた-", StrokeCount = 14, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "日本語", Reading = "にほんご", Meaning = "Japanese language" }, new() { Word = "英語", Reading = "えいご", Meaning = "English language" } ] },
        new() { Character = "国", Meaning = "country", OnyomiReading = "コク", KunyomiReading = "くに", StrokeCount = 8, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "国", Reading = "くに", Meaning = "country" }, new() { Word = "中国", Reading = "ちゅうごく", Meaning = "China" } ] },
        new() { Character = "外", Meaning = "outside", OnyomiReading = "ガイ, ゲ", KunyomiReading = "そと, はず-", StrokeCount = 5, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "外国", Reading = "がいこく", Meaning = "foreign country" }, new() { Word = "外", Reading = "そと", Meaning = "outside" } ] },
        new() { Character = "白", Meaning = "white", OnyomiReading = "ハク, ビャク", KunyomiReading = "しろ, しら-", StrokeCount = 5, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "白い", Reading = "しろい", Meaning = "white" } ] },
        new() { Character = "赤", Meaning = "red", OnyomiReading = "セキ, シャク", KunyomiReading = "あか-", StrokeCount = 7, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "赤い", Reading = "あかい", Meaning = "red" } ] },
        new() { Character = "青", Meaning = "blue, green", OnyomiReading = "セイ, ショウ", KunyomiReading = "あお-", StrokeCount = 8, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "青い", Reading = "あおい", Meaning = "blue" } ] },
        new() { Character = "高", Meaning = "tall, expensive", OnyomiReading = "コウ", KunyomiReading = "たか-", StrokeCount = 10, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "高い", Reading = "たかい", Meaning = "tall, expensive" }, new() { Word = "高校", Reading = "こうこう", Meaning = "high school" } ] },
        new() { Character = "長", Meaning = "long, chief", OnyomiReading = "チョウ", KunyomiReading = "なが-", StrokeCount = 8, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "長い", Reading = "ながい", Meaning = "long" } ] },
        new() { Character = "時", Meaning = "time, hour", OnyomiReading = "ジ", KunyomiReading = "とき", StrokeCount = 10, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "時間", Reading = "じかん", Meaning = "time" }, new() { Word = "何時", Reading = "なんじ", Meaning = "what time" } ] },
        new() { Character = "間", Meaning = "interval, between", OnyomiReading = "カン, ケン", KunyomiReading = "あいだ, ま", StrokeCount = 12, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "時間", Reading = "じかん", Meaning = "time" }, new() { Word = "人間", Reading = "にんげん", Meaning = "human being" } ] },
        new() { Character = "分", Meaning = "minute, part", OnyomiReading = "ブン, フン", KunyomiReading = "わ-", StrokeCount = 4, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "五分", Reading = "ごふん", Meaning = "five minutes" } ] },
        new() { Character = "毎", Meaning = "every", OnyomiReading = "マイ", KunyomiReading = "", StrokeCount = 6, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "毎日", Reading = "まいにち", Meaning = "every day" }, new() { Word = "毎朝", Reading = "まいあさ", Meaning = "every morning" } ] },
        new() { Character = "何", Meaning = "what, how many", OnyomiReading = "カ", KunyomiReading = "なに, なん", StrokeCount = 7, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "何", Reading = "なに", Meaning = "what" }, new() { Word = "何時", Reading = "なんじ", Meaning = "what time" } ] },
        new() { Character = "食", Meaning = "eat, food", OnyomiReading = "ショク, ジキ", KunyomiReading = "た-, く-", StrokeCount = 9, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "食べる", Reading = "たべる", Meaning = "to eat" }, new() { Word = "食事", Reading = "しょくじ", Meaning = "meal" } ] },
        new() { Character = "飲", Meaning = "drink", OnyomiReading = "イン", KunyomiReading = "の-", StrokeCount = 12, JlptLevel = 5, Grade = 3,
            Examples = [ new() { Word = "飲む", Reading = "のむ", Meaning = "to drink" }, new() { Word = "飲み物", Reading = "のみもの", Meaning = "beverage" } ] },
        new() { Character = "見", Meaning = "see, look", OnyomiReading = "ケン", KunyomiReading = "み-", StrokeCount = 7, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "見る", Reading = "みる", Meaning = "to see, to watch" } ] },
        new() { Character = "書", Meaning = "write", OnyomiReading = "ショ", KunyomiReading = "か-", StrokeCount = 10, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "書く", Reading = "かく", Meaning = "to write" }, new() { Word = "書道", Reading = "しょどう", Meaning = "calligraphy" } ] },
        new() { Character = "読", Meaning = "read", OnyomiReading = "ドク, トク", KunyomiReading = "よ-", StrokeCount = 14, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "読む", Reading = "よむ", Meaning = "to read" } ] },
        new() { Character = "聞", Meaning = "hear, ask", OnyomiReading = "ブン, モン", KunyomiReading = "き-, き-こえる", StrokeCount = 14, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "聞く", Reading = "きく", Meaning = "to hear, to ask" } ] },
        new() { Character = "話", Meaning = "speak, story", OnyomiReading = "ワ", KunyomiReading = "はな-, はなし", StrokeCount = 13, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "話す", Reading = "はなす", Meaning = "to speak" }, new() { Word = "話", Reading = "はなし", Meaning = "story, talk" } ] },
        new() { Character = "来", Meaning = "come", OnyomiReading = "ライ", KunyomiReading = "く-, き-, こ-", StrokeCount = 7, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "来る", Reading = "くる", Meaning = "to come" }, new() { Word = "来年", Reading = "らいねん", Meaning = "next year" } ] },
        new() { Character = "行", Meaning = "go", OnyomiReading = "コウ, ギョウ", KunyomiReading = "い-, ゆ-, おこな-", StrokeCount = 6, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "行く", Reading = "いく", Meaning = "to go" }, new() { Word = "旅行", Reading = "りょこう", Meaning = "travel" } ] },
        new() { Character = "出", Meaning = "exit, come out", OnyomiReading = "シュツ, スイ", KunyomiReading = "で-, だ-", StrokeCount = 5, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "出る", Reading = "でる", Meaning = "to come out, to exit" } ] },
        new() { Character = "入", Meaning = "enter", OnyomiReading = "ニュウ", KunyomiReading = "い-, はい-", StrokeCount = 2, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "入る", Reading = "はいる", Meaning = "to enter" }, new() { Word = "入口", Reading = "いりぐち", Meaning = "entrance" } ] },
        new() { Character = "車", Meaning = "car, vehicle", OnyomiReading = "シャ", KunyomiReading = "くるま", StrokeCount = 7, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "車", Reading = "くるま", Meaning = "car" }, new() { Word = "電車", Reading = "でんしゃ", Meaning = "electric train" } ] },
        new() { Character = "電", Meaning = "electricity", OnyomiReading = "デン", KunyomiReading = "", StrokeCount = 13, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "電話", Reading = "でんわ", Meaning = "telephone" }, new() { Word = "電車", Reading = "でんしゃ", Meaning = "electric train" } ] },
        new() { Character = "駅", Meaning = "station", OnyomiReading = "エキ", KunyomiReading = "", StrokeCount = 14, JlptLevel = 5, Grade = 3,
            Examples = [ new() { Word = "駅", Reading = "えき", Meaning = "station" } ] },
        new() { Character = "道", Meaning = "road, way", OnyomiReading = "ドウ, トウ", KunyomiReading = "みち", StrokeCount = 12, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "道", Reading = "みち", Meaning = "road, way" }, new() { Word = "北海道", Reading = "ほっかいどう", Meaning = "Hokkaido" } ] },
        new() { Character = "会", Meaning = "meeting, society", OnyomiReading = "カイ, エ", KunyomiReading = "あ-", StrokeCount = 6, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "会社", Reading = "かいしゃ", Meaning = "company" }, new() { Word = "会う", Reading = "あう", Meaning = "to meet" } ] },
        new() { Character = "社", Meaning = "company, society", OnyomiReading = "シャ, ジャ", KunyomiReading = "やしろ", StrokeCount = 7, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "会社", Reading = "かいしゃ", Meaning = "company" } ] },
        new() { Character = "父", Meaning = "father", OnyomiReading = "フ", KunyomiReading = "ちち", StrokeCount = 4, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "父", Reading = "ちち", Meaning = "father (own)" }, new() { Word = "お父さん", Reading = "おとうさん", Meaning = "father (polite)" } ] },
        new() { Character = "母", Meaning = "mother", OnyomiReading = "ボ", KunyomiReading = "はは", StrokeCount = 5, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "母", Reading = "はは", Meaning = "mother (own)" }, new() { Word = "お母さん", Reading = "おかあさん", Meaning = "mother (polite)" } ] },
        new() { Character = "友", Meaning = "friend", OnyomiReading = "ユウ", KunyomiReading = "とも", StrokeCount = 4, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "友達", Reading = "ともだち", Meaning = "friend" } ] },
        new() { Character = "半", Meaning = "half", OnyomiReading = "ハン", KunyomiReading = "なか-", StrokeCount = 5, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "一時半", Reading = "いちじはん", Meaning = "one-thirty" } ] },
        new() { Character = "今", Meaning = "now", OnyomiReading = "コン, キン", KunyomiReading = "いま", StrokeCount = 4, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "今日", Reading = "きょう", Meaning = "today" }, new() { Word = "今", Reading = "いま", Meaning = "now" } ] },
        new() { Character = "前", Meaning = "front, before", OnyomiReading = "ゼン", KunyomiReading = "まえ", StrokeCount = 9, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "前", Reading = "まえ", Meaning = "front, before" }, new() { Word = "午前", Reading = "ごぜん", Meaning = "AM, morning" } ] },
        new() { Character = "後", Meaning = "after, behind", OnyomiReading = "ゴ, コウ", KunyomiReading = "のち, うし-, あと", StrokeCount = 9, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "後", Reading = "あと", Meaning = "after" }, new() { Word = "午後", Reading = "ごご", Meaning = "PM, afternoon" } ] },
        new() { Character = "午", Meaning = "noon", OnyomiReading = "ゴ", KunyomiReading = "", StrokeCount = 4, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "午前", Reading = "ごぜん", Meaning = "AM" }, new() { Word = "午後", Reading = "ごご", Meaning = "PM" } ] },
        new() { Character = "朝", Meaning = "morning", OnyomiReading = "チョウ", KunyomiReading = "あさ", StrokeCount = 12, JlptLevel = 5, Grade = 3,
            Examples = [ new() { Word = "朝", Reading = "あさ", Meaning = "morning" }, new() { Word = "今朝", Reading = "けさ", Meaning = "this morning" } ] },
        new() { Character = "晩", Meaning = "evening", OnyomiReading = "バン", KunyomiReading = "", StrokeCount = 12, JlptLevel = 5, Grade = 0,
            Examples = [ new() { Word = "今晩", Reading = "こんばん", Meaning = "tonight" }, new() { Word = "晩ご飯", Reading = "ばんごはん", Meaning = "dinner" } ] },
        new() { Character = "夜", Meaning = "night", OnyomiReading = "ヤ", KunyomiReading = "よる, よ", StrokeCount = 8, JlptLevel = 5, Grade = 2,
            Examples = [ new() { Word = "夜", Reading = "よる", Meaning = "night" } ] },
        new() { Character = "花", Meaning = "flower", OnyomiReading = "カ", KunyomiReading = "はな", StrokeCount = 7, JlptLevel = 5, Grade = 1,
            Examples = [ new() { Word = "花", Reading = "はな", Meaning = "flower" }, new() { Word = "花火", Reading = "はなび", Meaning = "fireworks" } ] },
    ];
}
