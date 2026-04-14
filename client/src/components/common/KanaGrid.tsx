import hiraganaService from "@/services/hiraganaService";
import katakanaService from "@/services/katakanaService";
import { useEffect, useState } from "react";
import KanaButton from "./KanaButton";

const KANA_GRID: (string | null)[][] = [
  ["a", "i", "u", "e", "o"],
  ["ka", "ki", "ku", "ke", "ko"],
  ["sa", "shi", "su", "se", "so"],
  ["ta", "chi", "tsu", "te", "to"],
  ["na", "ni", "nu", "ne", "no"],
  ["ha", "hi", "fu", "he", "ho"],
  ["ma", "mi", "mu", "me", "mo"],
  ["ya", null, "yu", null, "yo"],
  ["ra", "ri", "ru", "re", "ro"],
  ["wa", null, null, null, "wo"],
  ["n", null, null, null, null],
  ["ga", "gi", "gu", "ge", "go"],
  ["za", "ji", "zu", "ze", "zo"],
  ["da", null, null, "de", "do"],
  ["ba", "bi", "bu", "be", "bo"],
  ["pa", "pi", "pu", "pe", "po"],
];

const KanaGrid = ({ type }: { type: "hiragana" | "katakana" }) => {
  const [charMap, setCharMap] = useState<Map<string, KanaCharacter>>(new Map());
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const api = type === "hiragana" ? hiraganaService : katakanaService;
    const fetchCharacters = async () => {
      try {
        const data = await api.getCharacters();
        setCharMap(new Map(data.map((c) => [c.romanization, c])));
      } catch (err) {
        setError(err instanceof Error ? err.message : "Failed to load characters");
      }
    };

    fetchCharacters();
  }, [type]);

  if (error) {
    return <div className="p-4 text-red-500">Error: {error}</div>;
  }

  return (
    <div className="max-w-4xl mx-auto p-4">
      <h1 className="text-3xl font-bold mb-6">{type === "hiragana" ? "Hiragana" : "Katakana"} Characters</h1>
      <div className="overflow-x-auto">
        <div className="space-y-2 min-w-max mx-auto">
          {KANA_GRID.map((row, rowIndex) => (
            <div key={rowIndex} className="flex justify-center space-x-2">
              {row.map((romanization, colIndex) => {
                const char = romanization ? charMap.get(romanization) : null;
                if (!char) {
                  return <div key={colIndex} className="w-20 h-[88px]" aria-hidden="true" />;
                }
                return (
                  <KanaButton
                    key={char.character}
                    type={type}
                    character={char.character}
                    romanization={char.romanization}
                    proficiency={char.proficiency}
                  />
                );
              })}
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default KanaGrid;
