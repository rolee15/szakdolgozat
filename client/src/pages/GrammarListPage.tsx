import { useQuery } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import grammarService from '@/services/grammarService';

const GrammarListPage = () => {
  const navigate = useNavigate();
  const { data: grammarPoints, isLoading, isError } = useQuery({
    queryKey: ['grammar'],
    queryFn: grammarService.getGrammarPoints,
  });

  if (isLoading) {
    return <div className="p-4 text-white">Loading...</div>;
  }

  if (isError) {
    return <div className="p-4 text-red-500">Failed to load grammar points.</div>;
  }

  return (
    <div className="p-4">
      <h1 className="text-3xl font-bold text-white mb-6">Grammar</h1>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        {grammarPoints?.map((point) => (
          <button
            key={point.id}
            type="button"
            onClick={() => navigate(`/grammar/${point.id}`)}
            className="w-full text-left bg-gray-800 hover:bg-gray-700 transition-colors rounded-lg shadow-md p-5 cursor-pointer"
          >
            <h2 className="text-lg font-semibold text-white mb-1">{point.title}</h2>
            <p className="text-gray-400 text-sm font-mono">{point.pattern}</p>
          </button>
        ))}
      </div>
    </div>
  );
};

export default GrammarListPage;
