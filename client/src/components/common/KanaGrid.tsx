import api from "@/services/kanaService";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

const KanaGrid = ({ type }: { type: "hiragana" | "katakana" }) => {
  const [characters, setCharacters] = useState<KanaCharacter[]>([]);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

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
              <button
                key={char.character}
                onClick={() => navigate(`/${type}/${char.character}`)}
                className="w-20 h-20 rounded-lg border-2 border-blue-500 hover:bg-blue-50
                             transition-colors duration-200 flex flex-col items-center justify-center"
              >
                <span className="text-2xl font-bold">{char.character}</span>
                <span className="text-sm text-gray-600">{char.romanization}</span>
              </button>
            ))}
          </div>
        ))}
      </div>
    </div>
  );
};

export default KanaGrid;
