import { useParams, useNavigate, Link } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import pathService from '@/services/pathService';

const UnitDetailPage = () => {
  const { unitId } = useParams<{ unitId: string }>();
  const navigate = useNavigate();
  const id = Number(unitId);

  const { data: unit, isLoading, isError } = useQuery({
    queryKey: ['path', id],
    queryFn: () => pathService.getUnitDetail(id),
  });

  if (isLoading) {
    return <div className="p-4 text-white">Loading...</div>;
  }

  if (isError || !unit) {
    return <div className="p-4 text-red-500">Failed to load unit detail.</div>;
  }

  return (
    <div className="p-4 max-w-2xl mx-auto text-white">
      <button
        onClick={() => navigate('/path')}
        className="text-gray-400 hover:text-white mb-6 flex items-center gap-1"
      >
        ← Back to Learning Path
      </button>

      <h1 className="text-3xl font-bold mb-2">{unit.title}</h1>
      <p className="text-gray-400 mb-6">{unit.description}</p>

      <section className="mb-8">
        <h2 className="text-xl font-semibold mb-3">Contents</h2>
        {unit.contents.length === 0 ? (
          <p className="text-gray-400">No content items.</p>
        ) : (
          <ul className="space-y-2">
            {unit.contents.map((item) => (
              <li
                key={`${item.contentType}-${item.contentId}`}
                className="bg-gray-800 rounded-lg px-4 py-3 flex items-center gap-3"
              >
                <span className="text-xs bg-gray-600 text-gray-300 px-2 py-1 rounded-full font-medium uppercase">
                  {item.contentType}
                </span>
                <span>{item.title}</span>
              </li>
            ))}
          </ul>
        )}
      </section>

      <div className="flex items-center gap-4">
        <button
          onClick={() => navigate(`/path/${unit.id}/test`)}
          className="bg-blue-600 hover:bg-blue-500 text-white px-6 py-2 rounded-lg font-medium"
        >
          Take Test
        </button>
        <Link to="/path" className="text-gray-400 hover:text-white">
          Back to Path
        </Link>
      </div>
    </div>
  );
};

export default UnitDetailPage;
