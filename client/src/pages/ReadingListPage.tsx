import { useQuery } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import readingService from '@/services/readingService';

const ReadingListPage = () => {
  const navigate = useNavigate();
  const { data: passages, isLoading, isError } = useQuery({
    queryKey: ['reading'],
    queryFn: readingService.getPassages,
  });

  if (isLoading) {
    return <div className="p-4 text-white">Loading...</div>;
  }

  if (isError) {
    return <div className="p-4 text-red-500">Failed to load reading passages.</div>;
  }

  return (
    <div className="p-4">
      <h1 className="text-3xl font-bold text-white mb-6">Reading</h1>
      {passages && passages.length === 0 ? (
        <p className="text-gray-400">No passages available.</p>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
          {passages?.map((passage) => (
            <div
              key={passage.id}
              role="button"
              tabIndex={0}
              onClick={() => navigate(`/reading/${passage.id}`)}
              onKeyDown={(e) => { if (e.key === 'Enter' || e.key === ' ') navigate(`/reading/${passage.id}`); }}
              className="bg-gray-800 hover:bg-gray-700 transition-colors rounded-lg shadow-md p-5 cursor-pointer"
            >
              <div className="flex items-center justify-between mb-2">
                <h2 className="text-lg font-semibold text-white">{passage.title}</h2>
                <span className="text-xs bg-blue-700 text-blue-100 px-2 py-1 rounded-full font-medium">
                  N{passage.jlptLevel}
                </span>
              </div>
              <div className="flex items-center mt-3">
                {passage.attemptCount === 0 ? (
                  <span className="text-gray-400 text-sm">Not attempted</span>
                ) : passage.isPassed ? (
                  <span className="flex items-center text-green-400 text-sm font-medium">
                    <svg className="w-4 h-4 mr-1" fill="currentColor" viewBox="0 0 20 20">
                      <path
                        fillRule="evenodd"
                        d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
                        clipRule="evenodd"
                      />
                    </svg>
                    Passed — {passage.score}%
                  </span>
                ) : (
                  <span className="text-yellow-400 text-sm">
                    Score: {passage.score}%
                  </span>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default ReadingListPage;
