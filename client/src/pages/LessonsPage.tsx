import api from "@/services/lessonService";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

const LessonsPage = () => {
  const [lessonsCount, setLessonsCount] = useState<number>(0);
  const [reviewsCount, setReviewsCount] = useState<number>(0);
  const [error, setError] = useState<string | null>(null);

  const navigate = useNavigate();

  useEffect(() => {
    const fetchLessonsCount = async () => {
      try {
        const count = await api.getLessonsCount();
        console.log(count);
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

    fetchLessonsCount();
    fetchReviewsCount();
  }, []);

  if (error) {
    return <div className="p-4 text-red-500">Error: {error}</div>;
  }

  return (
    <div>
      <h1 className="mb-8">Lessons</h1>
      <div className="flex flex-col md:flex-row gap-6">
        <div
          className="w-full md:w-1/2 bg-pink-500 hover:bg-pink-600 transition-colors rounded-lg shadow-md p-6 flex flex-col items-center justify-center text-center hover:shadow-lg"
          onClick={() => navigate("/lessons/new")}
        >
          <h2 className="text-2xl font-bold text-black mb-2">Learn</h2>
          <div className="flex flex-col items-center justify-center">
            <span className="text-4xl font-bold text-black">{lessonsCount}</span>
            <span className="ml-2 text-black">new {lessonsCount === 1 ? "lesson" : "lessons"} available</span>
          </div>
        </div>

        <div
          className="w-full md:w-1/2 bg-purple-500 hover:bg-purple-600 transition-colors rounded-lg shadow-md p-6 flex flex-col items-center justify-center text-center hover:shadow-lg"
          onClick={() => navigate("/lessons/review")}
        >
          <h2 className="text-2xl font-bold text-black mb-2">Review</h2>
          <div className="flex flex-col items-center justify-center">
            <span className="text-4xl font-bold text-black">{reviewsCount}</span>
            <span className="ml-2 text-black">{reviewsCount === 1 ? "item" : "items"} to review</span>
          </div>
        </div>
      </div>
    </div>
  );
};
export default LessonsPage;
