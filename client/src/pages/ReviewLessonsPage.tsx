import LessonReviewInput from "@/components/lessons/LessonReviewInput";
import WritingInput from "@/components/lessons/WritingInput";
import api from "@/services/lessonService";
import { useEffect, useState } from "react";

type ReviewItem =
  | { kind: "reading"; data: LessonReview }
  | { kind: "writing"; data: WritingReview };

const READING_BUTTON_CLASS = "bg-purple-600 hover:bg-purple-700";
const WRITING_BUTTON_CLASS = "bg-orange-600 hover:bg-orange-700";

const ReviewLessonsPage = () => {
  const [items, setItems] = useState<ReviewItem[]>([]);
  const [loaded, setLoaded] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const [currentIndex, setCurrentIndex] = useState<number>(0);

  const [feedback, setFeedback] = useState<{ result: LessonReviewResult | null; lastAnswer: string | null }>({
    result: null,
    lastAnswer: null,
  });

  useEffect(() => {
    const fetchAll = async () => {
      try {
        const [readings, writings] = await Promise.all([api.getLessonReviews(), api.getWritingReviews()]);
        const merged: ReviewItem[] = [
          ...readings.map((r): ReviewItem => ({ kind: "reading", data: r })),
          ...writings.map((w): ReviewItem => ({ kind: "writing", data: w })),
        ];
        // Interleave reading and writing items so practice alternates between types
        merged.sort((a, b) => (a.kind === b.kind ? 0 : a.kind === "reading" ? -1 : 1));
        const interleaved: ReviewItem[] = [];
        const readingItems = merged.filter((m) => m.kind === "reading");
        const writingItems = merged.filter((m) => m.kind === "writing");
        const max = Math.max(readingItems.length, writingItems.length);
        for (let i = 0; i < max; i++) {
          if (i < readingItems.length) interleaved.push(readingItems[i]);
          if (i < writingItems.length) interleaved.push(writingItems[i]);
        }
        setItems(interleaved);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Failed to load reviews");
      } finally {
        setLoaded(true);
      }
    };

    fetchAll();
  }, []);

  const currentItem: ReviewItem | undefined =
    items.length > 0 && currentIndex < items.length ? items[currentIndex] : undefined;

  const handleSubmit = async (answer: string) => {
    if (!currentItem) return;
    try {
      const result =
        currentItem.kind === "reading"
          ? await api.postLessonReviewCheck(currentItem.data.question, answer)
          : await api.postWritingReviewCheck(currentItem.data.characterId, answer);
      setFeedback({ result, lastAnswer: answer });
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to check answer");
    }
  };

  const advanceToNext = () => {
    if (!currentItem) return;

    if (feedback.result?.isCorrect) {
      const updated = items.filter((_, idx) => idx !== currentIndex);
      setItems(updated);
      if (currentIndex >= updated.length) {
        setCurrentIndex(Math.max(updated.length - 1, 0));
      }
    } else {
      // Cycle the failed item to the end so the user must answer it again
      // before the session can complete.
      const updated = [...items];
      const [current] = updated.splice(currentIndex, 1);
      updated.push(current);
      setItems(updated);
    }

    setFeedback({ result: null, lastAnswer: null });
  };

  if (error) {
    return <div className="p-4 text-red-500">Error: {error}</div>;
  }

  if (!loaded) {
    return <div className="p-4 text-white">Loading...</div>;
  }

  if (!currentItem) {
    return (
      <div className="flex flex-col items-center justify-center p-6">
        <span className="text-2xl font-semibold text-white">No more items to review.</span>
      </div>
    );
  }

  const onSubmit = feedback.result ? () => advanceToNext() : handleSubmit;

  const isReading = currentItem.kind === "reading";
  const buttonClass = isReading ? READING_BUTTON_CLASS : WRITING_BUTTON_CLASS;
  const typeLabel = isReading
    ? "Reading"
    : currentItem.data.characterType === "hiragana"
      ? "Writing · Hiragana"
      : "Writing · Katakana";
  const promptText = isReading ? currentItem.data.question : currentItem.data.romanization;
  const itemKey = isReading
    ? `reading:${currentItem.data.question}:${currentIndex}`
    : `writing:${currentItem.data.characterId}:${currentIndex}`;

  return (
    <div className="flex flex-col items-center justify-center p-6 w-full">
      <span className="text-sm text-gray-400 uppercase tracking-widest mb-2">{typeLabel}</span>
      <span className="text-4xl font-bold mb-4 text-white">{promptText}</span>

      {isReading ? (
        <LessonReviewInput key={itemKey} onSubmit={onSubmit} buttonClassName={buttonClass} />
      ) : (
        <WritingInput
          key={itemKey}
          characterType={currentItem.data.characterType}
          onSubmit={onSubmit}
          disabled={feedback.result !== null}
          buttonClassName={buttonClass}
        />
      )}

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

export default ReviewLessonsPage;
