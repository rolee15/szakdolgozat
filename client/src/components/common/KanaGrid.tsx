import api from "@services/kanaService";
import { useEffect, useState } from "react";
import KanaButton from "./KanaButton";

const KanaGrid = ({ type }: { type: "hiragana" | "katakana" }) => {
  const [characters, setCharacters] = useState<KanaCharacter[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchCharacters = async () => {
      try {
        const data = await api.getCharacters(type);
        setCharacters(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Failed to load characters");
      }
    };

    fetchCharacters();
  }, [type]);

  const rows = characters.reduce((acc: KanaCharacter[][], char, i) => {
    const rowIndex = Math.floor(i / 5);
    if (!acc[rowIndex]) acc[rowIndex] = [];
    acc[rowIndex].push(char);
    return acc;
  }, []);

  // TODO: pad some cells so the endings are aligned
  //console.log(rows);
  // insert null values to space out the grid
  // const yaRow = rows[8];
  // yaRow.splice(1, 0, null);

  if (error) {
    return <div className="p-4 text-red-500">Error: {error}</div>;
  }

  return (
    <div className="max-w-4xl mx-auto p-4">
      <h1 className="text-3xl font-bold mb-6">{type === "hiragana" ? "Hiragana" : "Katakana"} Characters</h1>
      <div className="space-y-2">
        {rows.map((row, rowIndex) => (
          <div key={rowIndex} className="flex justify-center space-x-2">
            {row.map((char) => (
              <KanaButton
                key={char.character}
                type={type}
                character={char.character}
                romanization={char.romanization}
                proficiency={char.proficiency}
              />
            ))}
          </div>
        ))}
      </div>
    </div>
  );
};

export default KanaGrid;
