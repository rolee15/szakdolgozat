import { useParams, useNavigate } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import readingService from '@/services/readingService';

const ReadingDetailPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const passageId = Number(id);

  const { data: detail, isLoading, isError } = useQuery({
    queryKey: ['reading', passageId],
    queryFn: () => readingService.getPassageDetail(passageId),
  });

  if (isLoading) {
    return <div className="p-4 text-white">Loading...</div>;
  }

  if (isError || !detail) {
    return <div className="p-4 text-red-500">Failed to load passage detail.</div>;
  }

  return (
    <div className="p-4 max-w-3xl mx-auto text-white">
      <button
        onClick={() => navigate('/reading')}
        className="text-gray-400 hover:text-white mb-6 flex items-center gap-1"
      >
        ← Back
      </button>

      <div className="flex items-center gap-3 mb-2">
        <h1 className="text-3xl font-bold">{detail.title}</h1>
        <span className="text-xs bg-indigo-700 text-indigo-100 px-2 py-1 rounded-full font-medium">
          N{detail.jlptLevel}
        </span>
      </div>
      {detail.source && (
        <p className="text-gray-400 text-sm mb-4">Source: {detail.source}</p>
      )}

      <div className="bg-gray-800 rounded-lg p-6 mb-8">
        <p className="text-xl leading-relaxed whitespace-pre-wrap">{detail.content}</p>
      </div>

      <p className="text-gray-400 text-sm">
        Test your comprehension in the final unit of the Learning Path.
      </p>
    </div>
  );
};

export default ReadingDetailPage;
