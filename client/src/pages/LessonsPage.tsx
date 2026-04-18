import api from "@/services/lessonService";
import { useEffect, useState } from "react";
import { NavLink } from "react-router-dom";

const LessonsPage = () => {
  const [lessonsCount, setLessonsCount] = useState<number>(0);
  const [reviewsCount, setReviewsCount] = useState<number>(0);
  const [writingCount, setWritingCount] = useState<number>(0);
  const [loaded, setLoaded] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchLessonsCount = async () => {
      try {
        const count = await api.getLessonsCount();
        setLessonsCount(count.count);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Failed to load lesson count");
      }
    };
    const fetchReviewsCount = async () => {
      try {
        const count = await api.getLessonReviewsCount();
        setReviewsCount(count.count);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Failed to load review count");
      }
    };
    const fetchWritingCount = async () => {
      try {
        const count = await api.getWritingReviewsCount();
        setWritingCount(count.count);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Failed to load writing review count");
      }
    };

    Promise.all([fetchLessonsCount(), fetchReviewsCount(), fetchWritingCount()]).then(() => {
      setLoaded(true);
    });
  }, []);

  if (error) {
    return <div className="p-4 text-red-500">Error: {error}</div>;
  }

  const renderCard = (
    to: string,
    count: number,
    color: string,
    hoverColor: string,
    title: string,
    label: string,
  ) => {
    const inner = (
      <div
        className={`${color} ${loaded && count > 0 ? `${hoverColor} transition-colors hover:shadow-lg` : "opacity-50 cursor-not-allowed"} rounded-lg shadow-md p-6 flex flex-col items-center justify-center text-center`}
      >
        <h2 className="text-2xl font-bold text-black mb-2">{title}</h2>
        <div className="flex flex-col items-center justify-center">
          <span className="text-4xl font-bold text-black">{count}</span>
          <span className="ml-2 text-black">{label}</span>
        </div>
      </div>
    );

    if (loaded && count === 0) {
      return <div className="w-full md:w-1/2">{inner}</div>;
    }

    return (
      <NavLink to={to} className="w-full md:w-1/2">
        {inner}
      </NavLink>
    );
  };

  return (
    <div>
      <h1 className="mb-8">Lessons</h1>
      <div className="flex flex-col md:flex-row gap-6">
        {renderCard(
          "/lessons/new",
          lessonsCount,
          "bg-pink-500",
          "hover:bg-pink-600",
          "Learn",
          `new ${lessonsCount === 1 ? "lesson" : "lessons"} available`,
        )}
        {renderCard(
          "/lessons/review",
          reviewsCount,
          "bg-purple-500",
          "hover:bg-purple-600",
          "Review",
          `${reviewsCount === 1 ? "item" : "items"} to review`,
        )}
        {renderCard(
          "/lessons/writing",
          writingCount,
          "bg-blue-500",
          "hover:bg-blue-600",
          "Writing",
          `${writingCount === 1 ? "item" : "items"} to practice`,
        )}
      </div>
    </div>
  );
};

export default LessonsPage;
