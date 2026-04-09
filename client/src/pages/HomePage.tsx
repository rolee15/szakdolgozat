import { useQuery } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import lessonService from '@/services/lessonService';
import pathService from '@/services/pathService';

const StatCard = ({
  label,
  value,
  isLoading,
  isError,
  to,
}: {
  label: string;
  value: string;
  isLoading: boolean;
  isError: boolean;
  to: string;
}) => (
  <Link to={to} className="bg-gray-800 rounded-lg p-6 text-center hover:bg-gray-700 transition-colors block">
    <p className="text-gray-400 text-sm mb-2">{label}</p>
    {isLoading ? (
      <div className="animate-pulse bg-gray-700 rounded h-8 w-16 mx-auto" />
    ) : (
      <p className="text-3xl font-bold text-white">{isError ? '–' : value}</p>
    )}
  </Link>
);

const HomePage = () => {
  const reviewsQuery = useQuery({
    queryKey: ['lessonReviewsCount'],
    queryFn: () => lessonService.getLessonReviewsCount(),
  });

  const lessonsQuery = useQuery({
    queryKey: ['lessonsCount'],
    queryFn: () => lessonService.getLessonsCount(),
  });

  const pathQuery = useQuery({
    queryKey: ['path'],
    queryFn: () => pathService.getPath(),
  });

  const pathCompleted = pathQuery.data?.filter((u) => u.isPassed).length ?? 0;
  const pathTotal = pathQuery.data?.length ?? 0;
  const pathValue = `${pathCompleted} / ${pathTotal} units completed`;

  return (
    <div className="mx-auto p-6">
      <h1 className="text-3xl font-bold text-center mb-6">Welcome to KanjiKa</h1>
      <p className="text-center">
        Where you can learn and practice Japanese characters.
      </p>
      <p className="text-center pt-16 text-6xl">漢字家</p>

      <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 mt-8 max-w-2xl mx-auto">
        <StatCard
          label="Due Reviews"
          value={String(reviewsQuery.data?.count ?? 0)}
          isLoading={reviewsQuery.isLoading}
          isError={reviewsQuery.isError}
          to="/lessons/reviews"
        />
        <StatCard
          label="New Lessons"
          value={String(lessonsQuery.data?.count ?? 0)}
          isLoading={lessonsQuery.isLoading}
          isError={lessonsQuery.isError}
          to="/lessons"
        />
        <StatCard
          label="Path Progress"
          value={pathValue}
          isLoading={pathQuery.isLoading}
          isError={pathQuery.isError}
          to="/path"
        />
      </div>
    </div>
  );
};

export default HomePage;
