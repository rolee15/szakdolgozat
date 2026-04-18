import { useQuery } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import pathService from '@/services/pathService';

const LearningPathPage = () => {
  const navigate = useNavigate();
  const { data: units, isLoading, isError } = useQuery({
    queryKey: ['path'],
    queryFn: pathService.getPath,
  });

  if (isLoading) {
    return <div className="p-4 text-white">Loading...</div>;
  }

  if (isError) {
    return <div className="p-4 text-red-500">Failed to load learning path.</div>;
  }

  return (
    <div className="p-4 max-w-2xl mx-auto">
      <h1 className="text-3xl font-bold text-white mb-8">Learning Path</h1>
      <div className="relative">
        {/* Vertical line */}
        <div className="absolute left-5 top-0 bottom-0 w-0.5 bg-gray-600" />

        <ol className="space-y-6">
          {units?.map((unit) => {
            const isLocked = !unit.isUnlocked;
            return (
              <li key={unit.id} className="relative flex items-start gap-4">
                {/* Circle indicator */}
                <div
                  className={`relative z-10 flex-shrink-0 w-10 h-10 rounded-full flex items-center justify-center font-bold text-sm ${
                    unit.isPassed
                      ? 'bg-green-600 text-white'
                      : isLocked
                        ? 'bg-gray-700 text-gray-500'
                        : 'bg-blue-600 text-white'
                  }`}
                >
                  {unit.isPassed ? (
                    <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
                      <path
                        fillRule="evenodd"
                        d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
                        clipRule="evenodd"
                      />
                    </svg>
                  ) : isLocked ? (
                    <svg className="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                      <path
                        fillRule="evenodd"
                        d="M5 9V7a5 5 0 0110 0v2a2 2 0 012 2v5a2 2 0 01-2 2H5a2 2 0 01-2-2v-5a2 2 0 012-2zm8-2v2H7V7a3 3 0 016 0z"
                        clipRule="evenodd"
                      />
                    </svg>
                  ) : (
                    unit.sortOrder
                  )}
                </div>

                {/* Card */}
                <div
                  onClick={() => !isLocked && navigate(`/path/${unit.id}`)}
                  className={`flex-1 bg-gray-800 rounded-lg p-5 transition-colors ${
                    isLocked
                      ? 'opacity-50 cursor-not-allowed'
                      : 'hover:bg-gray-700 cursor-pointer'
                  }`}
                >
                  <div className="flex items-center justify-between mb-1">
                    <h2 className="text-lg font-semibold text-white">{unit.title}</h2>
                    {unit.isPassed && (
                      <span className="text-xs bg-green-700 text-green-100 px-2 py-1 rounded-full font-medium">
                        Passed
                      </span>
                    )}
                    {!unit.isPassed && !isLocked && (
                      <span className="text-xs bg-blue-700 text-blue-100 px-2 py-1 rounded-full font-medium">
                        Unlocked
                      </span>
                    )}
                    {isLocked && (
                      <span className="text-xs bg-gray-600 text-gray-300 px-2 py-1 rounded-full font-medium">
                        Locked
                      </span>
                    )}
                  </div>
                  <p className="text-gray-400 text-sm mb-2">{unit.description}</p>
                  <div className="flex items-center gap-4 text-sm text-gray-400">
                    <span>{unit.contentCount} items</span>
                    {unit.bestScore > 0 && (
                      <span>Best score: {unit.bestScore}%</span>
                    )}
                  </div>
                </div>
              </li>
            );
          })}
        </ol>
      </div>
    </div>
  );
};

export default LearningPathPage;
