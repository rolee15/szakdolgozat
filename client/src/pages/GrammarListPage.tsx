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
          <div
            key={point.id}
            role="button"
            tabIndex={0}
            onClick={() => navigate(`/grammar/${point.id}`)}
            onKeyDown={(e) => { if (e.key === 'Enter' || e.key === ' ') navigate(`/grammar/${point.id}`); }}
            className="bg-gray-800 hover:bg-gray-700 transition-colors rounded-lg shadow-md p-5 cursor-pointer"
          >
            <h2 className="text-lg font-semibold text-white mb-1">{point.title}</h2>
            <p className="text-gray-400 text-sm font-mono mb-3">{point.pattern}</p>
            <div className="flex items-center">
              {point.isCompleted ? (
                <span className="flex items-center text-green-400 text-sm font-medium">
                  <svg
                    className="w-4 h-4 mr-1"
                    fill="currentColor"
                    viewBox="0 0 20 20"
                  >
                    <path
                      fillRule="evenodd"
                      d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
                      clipRule="evenodd"
                    />
                  </svg>
                  Completed
                </span>
              ) : (
                <span className="text-yellow-400 text-sm">
                  {point.correctCount}/3 correct
                </span>
              )}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default GrammarListPage;
