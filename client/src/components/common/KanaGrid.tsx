import hiraganaService from "@/services/hiraganaService";
import katakanaService from "@/services/katakanaService";
import { useEffect, useState } from "react";
import KanaButton from "./KanaButton";

const BASIC_GRID: (string | null)[][] = [
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
];
const BASIC_LABELS = ["−", "k", "s", "t", "n", "h", "m", "y", "r", "w", "n"];

const DAKUTEN_GRID: (string | null)[][] = [
  ["ga", "gi", "gu", "ge", "go"],
  ["za", "ji", "zu", "ze", "zo"],
  ["da", null, null, "de", "do"],
  ["ba", "bi", "bu", "be", "bo"],
];
const DAKUTEN_LABELS = ["g", "z", "d", "b"];

const HANDAKUTEN_GRID: (string | null)[][] = [
  ["pa", "pi", "pu", "pe", "po"],
];
const HANDAKUTEN_LABELS = ["p"];

const YOON_GRID: (string | null)[][] = [
  ["kya", "sha", "cha", "nya", "hya", "mya", "rya"],
  ["kyu", "shu", "chu", "nyu", "hyu", "myu", "ryu"],
  ["kyo", "sho", "cho", "nyo", "hyo", "myo", "ryo"],
];
const YOON_LABELS = ["ya", "yu", "yo"];
const YOON_COL_HEADERS = ["ky", "sh", "ch", "ny", "hy", "my", "ry"];

const YOON_DAKUTEN_GRID: (string | null)[][] = [
  ["gya", "ja", "bya"],
  ["gyu", "ju", "byu"],
  ["gyo", "jo", "byo"],
];
const YOON_DAKUTEN_LABELS = ["ya", "yu", "yo"];
const YOON_DAKUTEN_COL_HEADERS = ["gy", "j", "by"];

const YOON_HANDAKUTEN_GRID: (string | null)[][] = [
  ["pya"],
  ["pyu"],
  ["pyo"],
];
const YOON_HANDAKUTEN_LABELS = ["ya", "yu", "yo"];
const YOON_HANDAKUTEN_COL_HEADERS = ["py"];

const KANA_COL_HEADERS = ["a", "i", "u", "e", "o"];

interface GridSectionProps {
  title: string;
  grid: (string | null)[][];
  rowLabels: string[];
  colHeaders: string[];
  charMap: Map<string, KanaCharacter>;
  type: "hiragana" | "katakana";
  className?: string;
}

const GridSection = ({ title, grid, rowLabels, colHeaders, charMap, type, className }: GridSectionProps) => (
  <div className={className}>
    <h2 className="text-xl font-semibold mb-3">{title}</h2>
    <div className="space-y-2">
      <div className="flex space-x-2">
        <div className="w-10" aria-hidden="true" />
        {colHeaders.map((h) => (
          <div key={h} className="w-20 flex items-center justify-center text-sm font-medium text-gray-400">
            {h}
          </div>
        ))}
      </div>
      {grid.map((row, rowIndex) => (
        <div key={rowIndex} className="flex items-start space-x-2">
          <div className="w-10 h-[88px] flex items-center justify-center text-sm font-medium text-gray-400">
            {rowLabels[rowIndex]}
          </div>
          {row.map((romanization, colIndex) => {
            const char = romanization ? charMap.get(romanization) : null;
            if (!char) {
              return (
                <div key={colIndex} className="flex flex-col items-center" aria-hidden="true">
                  <div className="w-20 h-20 rounded border border-blue-500" />
                  <div className="w-20 h-2 mt-1" />
                </div>
              );
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
);

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
    <div className="overflow-x-auto p-4">
      <div className="w-fit mx-auto">
        <h1 className="text-3xl font-bold mb-6">{type === "hiragana" ? "Hiragana" : "Katakana"} Characters</h1>
        <GridSection title="Basic" grid={BASIC_GRID} rowLabels={BASIC_LABELS} colHeaders={KANA_COL_HEADERS} charMap={charMap} type={type} />
        <GridSection title="Dakuten ゛" grid={DAKUTEN_GRID} rowLabels={DAKUTEN_LABELS} colHeaders={KANA_COL_HEADERS} charMap={charMap} type={type} className="mt-8" />
        <GridSection title="Handakuten ゜" grid={HANDAKUTEN_GRID} rowLabels={HANDAKUTEN_LABELS} colHeaders={KANA_COL_HEADERS} charMap={charMap} type={type} className="mt-8" />
        <GridSection title="Yoon" grid={YOON_GRID} rowLabels={YOON_LABELS} colHeaders={YOON_COL_HEADERS} charMap={charMap} type={type} className="mt-8" />
        <GridSection title="Yoon + Dakuten ゛" grid={YOON_DAKUTEN_GRID} rowLabels={YOON_DAKUTEN_LABELS} colHeaders={YOON_DAKUTEN_COL_HEADERS} charMap={charMap} type={type} className="mt-8" />
        <GridSection title="Yoon + Handakuten ゜" grid={YOON_HANDAKUTEN_GRID} rowLabels={YOON_HANDAKUTEN_LABELS} colHeaders={YOON_HANDAKUTEN_COL_HEADERS} charMap={charMap} type={type} className="mt-8" />
      </div>
    </div>
  );
};

export default KanaGrid;
