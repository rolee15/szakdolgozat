import LessonReviewInput from "@/components/lessons/LessonReviewInput";
import api from "@/services/lessonService";
import { useEffect, useState } from "react";

const ReviewLessonsPage = () => {
  const [lessonReviews, setLessonReviews] = useState<LessonReview[]>([]);
  const [error, setError] = useState<string | null>(null);

  const [currentReview, setCurrentReview] = useState<LessonReview>();
  const [currentReviewIndex, setCurrentReviewIndex] = useState<number>(0);

  useEffect(() => {
    const fetchLessonReviews = async () => {
      try {
        const lessonReviews = await api.getLessonReviews();
        setLessonReviews(lessonReviews);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Failed to load lesson reviews");
      }
    };

    fetchLessonReviews();
  }, []);

  useEffect(() => {
    if (lessonReviews.length > 0) {
      setCurrentReview(lessonReviews[currentReviewIndex]);
    }
  }, [lessonReviews, currentReviewIndex]);

  const handleNext = async (answer: string) => {
    if (!currentReview) return;

    const isCorrect = await api.postLessonReviewCheck(currentReview.question, answer);
    if (isCorrect) {
      console.log("Correct!");
    } else {
      console.log("Incorrect. Try again.");
    }
    setCurrentReviewIndex(currentReviewIndex + 1);
  };

  if (error) {
    return <div className="p-4 text-red-500">Error: {error}</div>;
  }

  return (
    <div className="flex flex-col items-center justify-center p-6">
      {currentReview && <span className="text-4xl font-bold mb-4 text-white">{currentReview.question}</span>}
      <LessonReviewInput onSubmit={handleNext} />
    </div>
  );
};

export default ReviewLessonsPage;
