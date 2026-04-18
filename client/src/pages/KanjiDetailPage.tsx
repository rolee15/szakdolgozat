import { useParams, useNavigate } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import kanjiService from '@/services/kanjiService';
import SrsBadge from '@/components/common/SrsBadge';

const KanjiDetailPage = () => {
  const { character } = useParams<{ character: string }>();
  const navigate = useNavigate();

  const { data: kanji, isLoading, isError } = useQuery<KanjiDetail>({
    queryKey: ['kanji', 'detail', character],
    queryFn: () => kanjiService.getKanjiDetail(character!),
    enabled: !!character,
  });

  if (isLoading) {
    return (
      <div className="flex justify-center p-12">
        <span className="text-white text-lg">Loading...</span>
      </div>
    );
  }

  if (isError || !kanji) {
    return (
      <div className="flex justify-center p-12">
        <span className="text-red-400 text-lg">Failed to load kanji details.</span>
      </div>
    );
  }

  return (
    <div className="max-w-2xl mx-auto p-4">
      <button
        onClick={() => navigate('/kanji')}
        className="mb-6 text-blue-400 hover:text-blue-300 transition-colors"
      >
        &larr; Back to Kanji
      </button>

      <div className="flex flex-col items-center gap-4 mb-8">
        <span className="text-9xl font-bold">{kanji.character}</span>
        <span className="text-2xl text-gray-300">{kanji.meaning}</span>
      </div>

      <div className="grid grid-cols-2 gap-4 mb-8">
        <div className="bg-gray-800 rounded-lg p-4">
          <p className="text-sm text-gray-400 mb-1">On&apos;yomi</p>
          <p className="text-lg font-medium">{kanji.onyomiReading || '—'}</p>
        </div>
        <div className="bg-gray-800 rounded-lg p-4">
          <p className="text-sm text-gray-400 mb-1">Kun&apos;yomi</p>
          <p className="text-lg font-medium">{kanji.kunyomiReading || '—'}</p>
        </div>
        <div className="bg-gray-800 rounded-lg p-4">
          <p className="text-sm text-gray-400 mb-1">Stroke Count</p>
          <p className="text-lg font-medium">{kanji.strokeCount}</p>
        </div>
        <div className="bg-gray-800 rounded-lg p-4">
          <p className="text-sm text-gray-400 mb-1">JLPT Level</p>
          <p className="text-lg font-medium">N{kanji.jlptLevel}</p>
        </div>
        <div className="bg-gray-800 rounded-lg p-4">
          <p className="text-sm text-gray-400 mb-1">SRS Stage</p>
          <div className="mt-1">
            <SrsBadge stage={kanji.srsStage} />
          </div>
        </div>
        <div className="bg-gray-800 rounded-lg p-4">
          <p className="text-sm text-gray-400 mb-1">Proficiency</p>
          <div className="mt-1">
            <div className="w-full h-2 bg-gray-600 rounded-full overflow-hidden">
              <div
                className="h-full bg-green-400 rounded-full"
                style={{ width: `${Math.max(0, Math.min(100, kanji.proficiency))}%` }}
              />
            </div>
            <p className="text-sm text-gray-400 mt-1">{kanji.proficiency}%</p>
          </div>
        </div>
      </div>

      {kanji.examples.length > 0 && (
        <div>
          <h2 className="text-xl font-semibold mb-3">Examples</h2>
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="border-b border-gray-700">
                <th className="py-2 pr-4 text-gray-400 font-medium">Word</th>
                <th className="py-2 pr-4 text-gray-400 font-medium">Reading</th>
                <th className="py-2 text-gray-400 font-medium">Meaning</th>
              </tr>
            </thead>
            <tbody>
              {kanji.examples.map((ex, i) => (
                <tr key={i} className="border-b border-gray-800">
                  <td className="py-2 pr-4 text-lg">{ex.word}</td>
                  <td className="py-2 pr-4 text-gray-300">{ex.reading}</td>
                  <td className="py-2 text-gray-300">{ex.meaning}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default KanjiDetailPage;
