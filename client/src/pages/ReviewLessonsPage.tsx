import LessonReviewInput from "@/components/lessons/LessonReviewInput";
import api from "@/services/lessonService";
import { useEffect, useState } from "react";

const ReviewLessonsPage = () => {
  const [lessonReviews, setLessonReviews] = useState<LessonReview[]>([]);
  const [error, setError] = useState<string | null>(null);

  const [currentReview, setCurrentReview] = useState<LessonReview>();
  const [currentReviewIndex, setCurrentReviewIndex] = useState<number>(0);

  const [feedback, setFeedback] = useState<{ result: LessonReviewResult | null; lastAnswer: string | null }>({ result: null, lastAnswer: null });

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
    if (lessonReviews.length > 0 && currentReviewIndex < lessonReviews.length) {
      setCurrentReview(lessonReviews[currentReviewIndex]);
    } else {
      setCurrentReview(undefined);
    }
  }, [lessonReviews, currentReviewIndex]);

  const handleSubmit = async (answer: string) => {
    if (!currentReview) return;
    try {
      const result = await api.postLessonReviewCheck(currentReview.question, answer);
      setFeedback({ result, lastAnswer: answer });
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to check answer");
    }
  };

  const advanceToNext = () => {
    if (!currentReview) return;

    // If correct, remove it from the local review items
    if (feedback.result?.isCorrect) {
      const updated = lessonReviews.filter((_, idx) => idx !== currentReviewIndex);
      setLessonReviews(updated);
      // keep the same index to show the next item now occupying this index
      if (currentReviewIndex >= updated.length) {
        setCurrentReviewIndex(Math.max(updated.length - 1, 0));
      }
    } else {
      // Incorrect: just move to the next index
      setCurrentReviewIndex((idx) => idx + 1);
    }

    // Reset feedback state
    setFeedback({ result: null, lastAnswer: null });
  };

  const handleContainerClick = () => {
    if (feedback.result) {
      advanceToNext();
    }
  };

  if (error) {
    return <div className="p-4 text-red-500">Error: {error}</div>;
  }

  if (!currentReview) {
    return (
      <div className="flex flex-col items-center justify-center p-6">
        <span className="text-2xl font-semibold text-white">No more items to review.</span>
      </div>
    );
  }

  const onSubmit = feedback.result ? (() => advanceToNext()) : handleSubmit;

  return (
    <div className="flex flex-col items-center justify-center p-6 w-full" onClick={handleContainerClick}>
      <span className="text-4xl font-bold mb-4 text-white">{currentReview.question}</span>

      <LessonReviewInput key={`${currentReview.question}:${currentReviewIndex}`} onSubmit={onSubmit} />

      {feedback.result && (
        <div className={`mt-4 px-4 py-2 rounded text-center ${feedback.result.isCorrect ? "bg-green-600 text-white" : "bg-red-700 text-white"}`}>
          {feedback.result.isCorrect
            ? "Correct! Press Enter or click to continue."
            : (
              <div>
                <div className="font-semibold">Incorrect.</div>
                <div className="mt-1">Your answer: <span className="font-mono">{feedback.lastAnswer}</span></div>
                <div className="mt-1">Correct answer: <span className="font-mono">{feedback.result.correctAnswer}</span></div>
                <div className="mt-2">Press Enter or click to continue.</div>
              </div>
            )}
        </div>
      )}
    </div>
  );
};

export default ReviewLessonsPage;
