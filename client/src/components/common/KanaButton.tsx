import { useNavigate } from "react-router-dom";
import React from "react";
interface KanaButtonProps {
  type: "hiragana" | "katakana";
  character: string;
  romanization: string;
  proficiency: number;
}

const KanaButton: React.FC<KanaButtonProps> = ({ type, character, romanization, proficiency }) => {
  const validProficiency = Math.max(0, Math.min(100, proficiency));
  const level = Math.floor(validProficiency / 20) + 1;

  const navigate = useNavigate();

  return (
    <div className="relative flex flex-col items-center">
      <button
        className="w-20 h-20 rounded border border-blue-500 flex flex-col items-center justify-center text-2xl font-bold hover:bg-blue-500 hover:text-white transition-colors"
        aria-label={`Kana ${character}, Proficiency Level ${level}`}
        onClick={() => navigate(`/${type}/${character}`)}
      >
        <span className="text-2xl font-bold">{character}</span>
        <span className="text-sm text-gray-400">{romanization}</span>
      </button>

      <div className="w-20 h-2 mt-1 bg-gray-200 rounded-full overflow-hidden">
        <div className="h-full bg-green-400 rounded-full" style={{ width: `${validProficiency}%` }} />
      </div>
    </div>
  );
};

export default KanaButton;
