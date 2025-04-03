using KanjiKa.Core.Entities;
using KanjiKa.Core.Entities.Kana;
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
        const string testPassword = "12345";
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
                        Level = 80,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-10),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },

                    new Proficiency
                    {
                        Id = 2,
                        UserId = 1,
                        CharacterId = 2,
                        Level = 80,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-9),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-2)
                    },
                    new Proficiency
                    {
                        Id = 3,
                        UserId = 1,
                        CharacterId = 3,
                        Level = 75,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-8),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 4,
                        UserId = 1,
                        CharacterId = 4,
                        Level = 70,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-7),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 5,
                        UserId = 1,
                        CharacterId = 5,
                        Level = 60,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-6),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 6,
                        UserId = 1,
                        CharacterId = 6,
                        Level = 70,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-5),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 7,
                        UserId = 1,
                        CharacterId = 7,
                        Level = 80,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-4),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 8,
                        UserId = 1,
                        CharacterId = 8,
                        Level = 90,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-3),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 9,
                        UserId = 1,
                        CharacterId = 9,
                        Level = 100,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-2),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 10,
                        UserId = 1,
                        CharacterId = 10,
                        Level = 100,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 11,
                        UserId = 1,
                        CharacterId = 11,
                        Level = 100,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 12,
                        UserId = 1,
                        CharacterId = 12,
                        Level = 60,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 13,
                        UserId = 1,
                        CharacterId = 13,
                        Level = 70,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 14,
                        UserId = 1,
                        CharacterId = 14,
                        Level = 40,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 15,
                        UserId = 1,
                        CharacterId = 15,
                        Level = 50,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 16,
                        UserId = 1,
                        CharacterId = 16,
                        Level = 60,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 17,
                        UserId = 1,
                        CharacterId = 17,
                        Level = 70,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-20),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-10)
                    },
                    new Proficiency
                    {
                        Id = 18,
                        UserId = 1,
                        CharacterId = 18,
                        Level = 80,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-30),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-20)
                    },
                    new Proficiency
                    {
                        Id = 19,
                        UserId = 1,
                        CharacterId = 19,
                        Level = 90,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-40),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-30)
                    },
                    new Proficiency
                    {
                        Id = 20,
                        UserId = 1,
                        CharacterId = 20,
                        Level = 100,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-50),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-40)
                    },
                    new Proficiency
                    {
                        Id = 21,
                        UserId = 1,
                        CharacterId = 21,
                        Level = 100,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-60),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-50)
                    },
                    new Proficiency
                    {
                        Id = 22,
                        UserId = 1,
                        CharacterId = 22,
                        Level = 100,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-20),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-10)
                    },
                    new Proficiency
                    {
                        Id = 23,
                        UserId = 1,
                        CharacterId = 23,
                        Level = 80,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-20),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-10)
                    },
                    new Proficiency
                    {
                        Id = 24,
                        UserId = 1,
                        CharacterId = 24,
                        Level = 20,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-2),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 25,
                        UserId = 1,
                        CharacterId = 25,
                        Level = 10,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-2),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 26,
                        UserId = 1,
                        CharacterId = 26,
                        Level = 5,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 27,
                        UserId = 1,
                        CharacterId = 27,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 28,
                        UserId = 1,
                        CharacterId = 28,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 29,
                        UserId = 1,
                        CharacterId = 29,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 30,
                        UserId = 1,
                        CharacterId = 30,
                        Level = 10,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 31,
                        UserId = 1,
                        CharacterId = 31,
                        Level = 20,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-2),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 32,
                        UserId = 1,
                        CharacterId = 32,
                        Level = 10,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 33,
                        UserId = 1,
                        CharacterId = 33,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 34,
                        UserId = 1,
                        CharacterId = 34,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 35,
                        UserId = 1,
                        CharacterId = 35,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 36,
                        UserId = 1,
                        CharacterId = 36,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 37,
                        UserId = 1,
                        CharacterId = 37,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 38,
                        UserId = 1,
                        CharacterId = 38,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 39,
                        UserId = 1,
                        CharacterId = 39,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 40,
                        UserId = 1,
                        CharacterId = 40,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 41,
                        UserId = 1,
                        CharacterId = 41,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 42,
                        UserId = 1,
                        CharacterId = 42,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 43,
                        UserId = 1,
                        CharacterId = 43,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 44,
                        UserId = 1,
                        CharacterId = 44,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 45,
                        UserId = 1,
                        CharacterId = 45,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 46,
                        UserId = 1,
                        CharacterId = 46,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 47,
                        UserId = 1,
                        CharacterId = 47,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 48,
                        UserId = 1,
                        CharacterId = 48,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 49,
                        UserId = 1,
                        CharacterId = 49,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 50,
                        UserId = 1,
                        CharacterId = 50,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 51,
                        UserId = 1,
                        CharacterId = 51,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 52,
                        UserId = 1,
                        CharacterId = 52,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 53,
                        UserId = 1,
                        CharacterId = 53,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 54,
                        UserId = 1,
                        CharacterId = 54,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 55,
                        UserId = 1,
                        CharacterId = 55,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 56,
                        UserId = 1,
                        CharacterId = 56,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 57,
                        UserId = 1,
                        CharacterId = 57,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 58,
                        UserId = 1,
                        CharacterId = 58,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 59,
                        UserId = 1,
                        CharacterId = 59,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 60,
                        UserId = 1,
                        CharacterId = 60,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                    new Proficiency
                    {
                        Id = 61,
                        UserId = 1,
                        CharacterId = 61,
                        Level = 0,
                        LearnedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        LastPracticed = DateTimeOffset.UtcNow.AddDays(-1)
                    }
                ]
            }
        ];
    }
}
