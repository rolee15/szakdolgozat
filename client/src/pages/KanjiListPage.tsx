import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import kanjiService from '@/services/kanjiService';

type JlptLevel = 5 | 4 | 3 | 2 | 1;
const JLPT_LEVELS: JlptLevel[] = [5, 4, 3, 2, 1];

const SRS_STAGE_COLORS: Record<string, string> = {
  Apprentice: 'bg-pink-600',
  Guru: 'bg-purple-600',
  Master: 'bg-blue-600',
  Enlightened: 'bg-indigo-600',
  Burned: 'bg-gray-600',
};

const KanjiListPage = () => {
  const [selectedLevel, setSelectedLevel] = useState<JlptLevel>(5);
  const navigate = useNavigate();

  const { data: kanjiList, isLoading, isError } = useQuery<KanjiCharacter[]>({
    queryKey: ['kanji', 'level', selectedLevel],
    queryFn: () => kanjiService.getKanjiByLevel(selectedLevel),
  });

  return (
    <div className="max-w-4xl mx-auto p-4">
      <h1 className="text-3xl font-bold mb-6">Kanji</h1>

      <div className="flex gap-2 mb-8">
        {JLPT_LEVELS.map((level) => (
          <button
            key={level}
            onClick={() => setSelectedLevel(level)}
            className={`px-5 py-2 rounded-lg font-medium transition-colors focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:ring-offset-2 ${
              selectedLevel === level
                ? 'bg-indigo-500 text-white'
                : 'bg-gray-700 text-gray-300 hover:bg-gray-600'
            }`}
          >
            N{level}
          </button>
        ))}
      </div>

      {isLoading && (
        <div className="flex justify-center p-12">
          <span className="text-white text-lg">Loading kanji...</span>
        </div>
      )}

      {isError && (
        <div className="flex justify-center p-12">
          <span className="text-red-400 text-lg">Failed to load kanji.</span>
        </div>
      )}

      {kanjiList && (
        <div className="grid grid-cols-4 sm:grid-cols-6 md:grid-cols-8 gap-3">
          {kanjiList.map((kanji) => {
            const stageColor = SRS_STAGE_COLORS[kanji.srsStage] ?? 'bg-gray-700';
            return (
              <button
                key={kanji.character}
                onClick={() => navigate(`/kanji/${encodeURIComponent(kanji.character)}`)}
                className="flex flex-col items-center gap-1 p-2 rounded border border-blue-500 hover:bg-blue-500 hover:text-white transition-colors focus:outline-none focus:ring-2 focus:ring-blue-400"
                aria-label={`Kanji ${kanji.character}, meaning: ${kanji.meaning}`}
              >
                <span className="text-3xl font-bold">{kanji.character}</span>
                <span className="text-xs text-gray-400 text-center leading-tight line-clamp-1">
                  {kanji.meaning}
                </span>
                <span className={`text-xs px-1.5 py-0.5 rounded text-white font-medium ${stageColor}`}>
                  {kanji.srsStage}
                </span>
              </button>
            );
          })}
        </div>
      )}
    </div>
  );
};

export default KanjiListPage;
