import WritingInput from "@/components/lessons/WritingInput";
import api from "@/services/lessonService";
import { useEffect, useState } from "react";

const WritingPracticePage = () => {
  const [writingReviews, setWritingReviews] = useState<WritingReview[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const [currentIndex, setCurrentIndex] = useState<number>(0);
  const [currentReview, setCurrentReview] = useState<WritingReview | undefined>(undefined);

  const [feedback, setFeedback] = useState<{ result: LessonReviewResult | null; lastAnswer: string | null }>({
    result: null,
    lastAnswer: null,
  });

  useEffect(() => {
    const fetchWritingReviews = async () => {
      try {
        const reviews = await api.getWritingReviews();
        setWritingReviews(reviews);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Failed to load writing reviews");
      } finally {
        setLoading(false);
      }
    };

    fetchWritingReviews();
  }, []);

  useEffect(() => {
    if (writingReviews.length > 0 && currentIndex < writingReviews.length) {
      setCurrentReview(writingReviews[currentIndex]);
    } else {
      setCurrentReview(undefined);
    }
  }, [writingReviews, currentIndex]);

  const handleSubmit = async (typedCharacter: string) => {
    if (!currentReview) return;
    try {
      const result = await api.postWritingReviewCheck(currentReview.characterId, typedCharacter);
      setFeedback({ result, lastAnswer: typedCharacter });
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to check answer");
    }
  };

  const advanceToNext = () => {
    if (!currentReview) return;

    if (feedback.result?.isCorrect) {
      const updated = writingReviews.filter((_, idx) => idx !== currentIndex);
      setWritingReviews(updated);
      if (currentIndex >= updated.length) {
        setCurrentIndex(Math.max(updated.length - 1, 0));
      }
    } else {
      // For incorrect answers, cycle the current item to the end of the queue
      // so the user must answer it again before the session can complete.
      const updated = [...writingReviews];
      const [current] = updated.splice(currentIndex, 1);
      updated.push(current);
      setWritingReviews(updated);
      // currentIndex stays the same — the next item is now at the same position
    }

    setFeedback({ result: null, lastAnswer: null });
  };

  if (loading) {
    return <div className="p-4 text-white">Loading...</div>;
  }

  if (error) {
    return <div className="p-4 text-red-500">Error: {error}</div>;
  }

  if (!currentReview) {
    return (
      <div className="flex flex-col items-center justify-center p-6">
        <span className="text-2xl font-semibold text-white">
          {writingReviews.length === 0 ? "No items to review." : "Writing practice complete!"}
        </span>
      </div>
    );
  }

  const onSubmit = feedback.result ? () => advanceToNext() : handleSubmit;

  return (
    <div className="flex flex-col items-center justify-center p-6 w-full">
      <span className="text-sm text-gray-400 uppercase tracking-widest mb-2">
        {currentReview.characterType === "hiragana" ? "Hiragana" : "Katakana"}
      </span>
      <span className="text-6xl font-bold mb-4 text-white">{currentReview.romanization}</span>

      <WritingInput
        key={`${currentReview.characterId}:${currentIndex}`}
        characterType={currentReview.characterType}
        onSubmit={onSubmit}
        disabled={feedback.result !== null}
      />

      {feedback.result && (
        <button
          type="button"
          onClick={advanceToNext}
          aria-label="Continue"
          className={`mt-4 px-4 py-2 rounded text-center focus:outline-none focus:ring-2 focus:ring-offset-2 ${
            feedback.result.isCorrect
              ? "bg-green-600 text-white focus:ring-green-300"
              : "bg-red-700 text-white focus:ring-red-300"
          }`}
        >
          {feedback.result.isCorrect ? (
            <span>Correct! Press Enter or click to continue.</span>
          ) : (
            <div>
              <div className="font-semibold">Incorrect.</div>
              <div className="mt-1">
                Your answer: <span className="font-mono">{feedback.lastAnswer}</span>
              </div>
              <div className="mt-1">
                Correct answer:{" "}
                <span className="font-mono text-2xl">{feedback.result.correctAnswer}</span>
              </div>
              <div className="mt-2">Press Enter or click to continue.</div>
            </div>
          )}
        </button>
      )}
    </div>
  );
};

export default WritingPracticePage;
