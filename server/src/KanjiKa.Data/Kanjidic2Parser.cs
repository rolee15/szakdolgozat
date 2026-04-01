// [1] EDRDG, "KANJIDIC2 Project" — http://www.edrdg.org/wiki/index.php/KANJIDIC_Project (accessed 2025-01-01)
using System.IO.Compression;
using System.Xml;
using KanjiKa.Core.Entities.Kanji;

namespace KanjiKa.Data;

public static class Kanjidic2Parser
{
    private static readonly Dictionary<int, int> JlptMap = new()
    {
        { 4, 5 },
        { 3, 4 },
        { 2, 2 },
        { 1, 1 }
    };

    public static List<Kanji> Parse()
    {
        var assembly = typeof(Kanjidic2Parser).Assembly;
        using var gzStream = assembly.GetManifestResourceStream("KanjiKa.Data.Data.kanjidic2.xml.gz")!;
        using var deflateStream = new GZipStream(gzStream, CompressionMode.Decompress);

        var settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Parse,
            ValidationType = ValidationType.None
        };

        var result = new List<Kanji>();

        using var reader = XmlReader.Create(deflateStream, settings);

        while (reader.Read())
        {
            if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "character")
                continue;

            using var subtree = reader.ReadSubtree();
            var kanji = ParseCharacter(subtree);
            if (kanji != null)
                result.Add(kanji);
        }

        return result;
    }

    private static Kanji? ParseCharacter(XmlReader reader)
    {
        string? literal = null;
        var meanings = new List<string>();
        var onyomi = new List<string>();
        var kunyomi = new List<string>();
        int strokeCount = 0;
        int grade = 0;
        int jlptLevel = 0;

        while (reader.Read())
        {
            if (reader.NodeType != XmlNodeType.Element)
                continue;

            switch (reader.LocalName)
            {
                case "literal":
                    literal = reader.ReadElementContentAsString();
                    break;

                case "grade":
                    if (int.TryParse(reader.ReadElementContentAsString(), out var g))
                        grade = g;
                    break;

                case "stroke_count":
                    // Only capture the first stroke_count element
                    if (strokeCount == 0 && int.TryParse(reader.ReadElementContentAsString(), out var sc))
                        strokeCount = sc;
                    break;

                case "jlpt":
                    if (int.TryParse(reader.ReadElementContentAsString(), out var rawJlpt))
                        jlptLevel = JlptMap.TryGetValue(rawJlpt, out var mapped) ? mapped : 0;
                    break;

                case "reading":
                    var rType = reader.GetAttribute("r_type");
                    var readingValue = reader.ReadElementContentAsString();
                    if (rType == "ja_on")
                        onyomi.Add(readingValue);
                    else if (rType == "ja_kun")
                        kunyomi.Add(readingValue);
                    break;

                case "meaning":
                    var mLang = reader.GetAttribute("m_lang");
                    if (mLang == null) // no m_lang attribute means English
                        meanings.Add(reader.ReadElementContentAsString());
                    break;
            }
        }

        if (literal == null)
            return null;

        var meaningStr = string.Join(", ", meanings);
        var onyomiStr = string.Join(", ", onyomi);
        var kunyomiStr = string.Join(", ", kunyomi);

        // Skip kanji where all of meaning, onyomi, and kunyomi are empty
        if (string.IsNullOrEmpty(meaningStr) && string.IsNullOrEmpty(onyomiStr) && string.IsNullOrEmpty(kunyomiStr))
            return null;

        return new Kanji
        {
            Character = literal,
            Meaning = meaningStr,
            OnyomiReading = onyomiStr,
            KunyomiReading = kunyomiStr,
            StrokeCount = strokeCount,
            Grade = grade,
            JlptLevel = jlptLevel,
            Examples = []
        };
    }
}
