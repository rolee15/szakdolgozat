import api from "@/services/lessonService";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

const NewLessonsPage = () => {
  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [error, setError] = useState<string | null>(null);

  const [currentLesson, setCurrentLesson] = useState<Lesson>();
  const [currentLessonIndex, setCurrentLessonIndex] = useState<number>(0);

  const navigate = useNavigate();

  useEffect(() => {
    const fetchNewLessons = async () => {
      try {
        const newLessons = await api.getLessons(0, 5);
        setLessons(newLessons);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Failed to load new lessons");
      }
    };

    fetchNewLessons();
  }, []);

  useEffect(() => {
    if (lessons.length > 0) {
      setCurrentLesson(lessons[currentLessonIndex]);
    }
  }, [lessons, currentLessonIndex]);

  if (error) {
    return <div className="p-4 text-red-500">Error: {error}</div>;
  }

  const handleNext = async () => {
    if (currentLesson) {
      await api.postLearnLesson(currentLesson.characterId);
      setCurrentLessonIndex(currentLessonIndex + 1);
      if (currentLessonIndex + 1 >= lessons.length) {
        navigate("/lessons");
      }
    }
  };

  return (
    <div className="flex flex-col items-center justify-center p-6">
      <div className="max-w-md w-full shadow-lg rounded-xl p-8 flex flex-col items-center">
        <div className="text-sm font-medium text-white uppercase tracking-wider mb-2">
          {currentLesson?.type === 0 ? "Hiragana" : "Katakana"}
        </div>

        <div className="text-9xl font-bold mb-4 text-indigo-500">
          {currentLesson?.symbol}
        </div>

        <div className="text-2xl font-medium white mb-8">
          {currentLesson?.romanization}
        </div>

        <button
          onClick={handleNext}
          className="px-8 py-3 bg-indigo-500 text-white font-medium rounded-lg hover:bg-indigo-600 transition-colors focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
        >
          Next
        </button>

        <div className="mt-4 text-sm text-gray-500">
          {currentLessonIndex + 1} of {lessons.length}
        </div>
      </div>
    </div>
  );
};
export default NewLessonsPage;
