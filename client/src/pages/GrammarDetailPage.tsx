import { useParams, useNavigate } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import grammarService from '@/services/grammarService';

const GrammarDetailPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const grammarId = Number(id);

  const { data: detail, isLoading, isError } = useQuery({
    queryKey: ['grammar', grammarId],
    queryFn: () => grammarService.getGrammarDetail(grammarId),
  });

  if (isLoading) {
    return <div className="p-4 text-white">Loading...</div>;
  }

  if (isError || !detail) {
    return <div className="p-4 text-red-500">Failed to load grammar detail.</div>;
  }

  return (
    <div className="p-4 max-w-3xl mx-auto text-white">
      <button
        onClick={() => navigate('/grammar')}
        className="text-gray-400 hover:text-white mb-6 flex items-center gap-1"
      >
        ← Back
      </button>

      <section className="mb-8">
        <h1 className="text-3xl font-bold mb-2">{detail.title}</h1>
        <p className="font-mono bg-gray-800 rounded px-3 py-2 text-indigo-300 mb-4 inline-block">
          {detail.pattern}
        </p>
        <p className="text-gray-300 mb-6">{detail.explanation}</p>

        {detail.examples.length > 0 && (
          <div>
            <h2 className="text-xl font-semibold mb-3">Examples</h2>
            <ul className="space-y-4">
              {detail.examples.map((ex, i) => (
                <li key={i} className="bg-gray-800 rounded-lg shadow-md p-5">
                  <p className="text-2xl mb-1">{ex.japanese}</p>
                  <p className="text-sm italic text-gray-400 mb-1">{ex.reading}</p>
                  <p className="text-gray-300">{ex.english}</p>
                </li>
              ))}
            </ul>
          </div>
        )}

        <p className="text-gray-400 text-sm mt-8">
          Practice this grammar point in the Learning Path.
        </p>
      </section>
    </div>
  );
};

export default GrammarDetailPage;
