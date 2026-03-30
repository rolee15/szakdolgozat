import { useState } from "react";
import { useQuery, useMutation } from "@tanstack/react-query";
import kanaService from "@/services/kanaService";
import lessonService from "@/services/lessonService";
import styles from "./FlashCardPage.module.css";

type KanaMode = "hiragana" | "katakana";

const FlashCardPage = () => {
  const [mode, setMode] = useState<KanaMode>("hiragana");
  const [currentIndex, setCurrentIndex] = useState(0);
  const [isFlipped, setIsFlipped] = useState(false);
  const [sessionDone, setSessionDone] = useState(false);

  const { data: characters, isLoading, isError } = useQuery<KanaCharacter[]>({
    queryKey: ["flashcards", mode],
    queryFn: () => kanaService.getCharacters(mode),
  });

  const reviewMutation = useMutation<LessonReviewResult, Error, { question: string; answer: string }>({
    mutationFn: ({ question, answer }) =>
      lessonService.postLessonReviewCheck(question, answer),
  });

  const cards = characters ?? [];
  const currentCard = cards[currentIndex];
  const total = cards.length;

  const handleFlip = () => {
    setIsFlipped((prev) => !prev);
  };

  const advance = () => {
    const nextIndex = currentIndex + 1;
    if (nextIndex >= total) {
      setSessionDone(true);
    } else {
      setCurrentIndex(nextIndex);
      setIsFlipped(false);
    }
  };

  const handleKnowIt = () => {
    if (!currentCard) return;
    reviewMutation.mutate(
      { question: currentCard.character, answer: currentCard.romanization },
      { onSettled: () => advance() }
    );
  };

  const handleDontKnowIt = () => {
    if (!currentCard) return;
    reviewMutation.mutate(
      { question: currentCard.character, answer: "" },
      { onSettled: () => advance() }
    );
  };

  const handleRestart = () => {
    setCurrentIndex(0);
    setIsFlipped(false);
    setSessionDone(false);
  };

  const handleModeChange = (newMode: KanaMode) => {
    setMode(newMode);
    setCurrentIndex(0);
    setIsFlipped(false);
    setSessionDone(false);
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center p-12">
        <span className="text-white text-lg">Loading cards...</span>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="flex items-center justify-center p-12">
        <span className="text-red-400 text-lg">Failed to load characters.</span>
      </div>
    );
  }

  if (sessionDone) {
    return (
      <div className="flex flex-col items-center justify-center gap-6 p-12">
        <h2 className="text-3xl font-bold text-white">Session complete!</h2>
        <p className="text-gray-400">You reviewed all {total} {mode} cards.</p>
        <button
          onClick={handleRestart}
          className="px-8 py-3 bg-indigo-500 text-white font-medium rounded-lg hover:bg-indigo-600 transition-colors focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:ring-offset-2"
        >
          Restart
        </button>
      </div>
    );
  }

  return (
    <div className="flex flex-col items-center gap-8 p-8">
      <h1 className="text-2xl font-bold text-white">Flash Cards</h1>

      {/* Mode selector */}
      <div className="flex gap-2">
        {(["hiragana", "katakana"] as KanaMode[]).map((m) => (
          <button
            key={m}
            onClick={() => handleModeChange(m)}
            className={`px-5 py-2 rounded-lg font-medium capitalize transition-colors focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:ring-offset-2 ${
              mode === m
                ? "bg-indigo-500 text-white"
                : "bg-gray-700 text-gray-300 hover:bg-gray-600"
            }`}
          >
            {m.charAt(0).toUpperCase() + m.slice(1)}
          </button>
        ))}
        <button
          disabled
          title="Kanji not available yet"
          className="px-5 py-2 rounded-lg font-medium capitalize bg-gray-800 text-gray-600 cursor-not-allowed"
        >
          Kanji
        </button>
      </div>

      {/* Progress indicator */}
      <p className="text-gray-400 text-sm">
        {currentIndex + 1} / {total}
      </p>

      {/* Flip card */}
      <div className={styles.scene} onClick={handleFlip} role="button" aria-label="Flip card">
        <div className={`${styles.card} ${isFlipped ? styles.flipped : ""}`}>
          <div className={`${styles.cardFace} ${styles.cardFront}`}>
            <span className="text-8xl font-bold text-indigo-300 select-none">
              {currentCard?.character}
            </span>
            <span className="mt-4 text-sm text-indigo-400 select-none">Click to reveal</span>
          </div>
          <div className={`${styles.cardFace} ${styles.cardBack}`}>
            <span className="text-4xl font-bold text-white select-none">
              {currentCard?.romanization}
            </span>
            <span className="mt-3 text-sm uppercase tracking-widest text-blue-400 select-none">
              {currentCard?.type}
            </span>
          </div>
        </div>
      </div>

      {/* Know it / Don't know it buttons */}
      <div className="flex gap-4">
        <button
          onClick={handleDontKnowIt}
          disabled={reviewMutation.isPending}
          className="px-6 py-3 bg-red-700 text-white font-medium rounded-lg hover:bg-red-800 transition-colors focus:outline-none focus:ring-2 focus:ring-red-400 focus:ring-offset-2 disabled:opacity-50"
        >
          Don't know it
        </button>
        <button
          onClick={handleKnowIt}
          disabled={reviewMutation.isPending}
          className="px-6 py-3 bg-green-700 text-white font-medium rounded-lg hover:bg-green-800 transition-colors focus:outline-none focus:ring-2 focus:ring-green-400 focus:ring-offset-2 disabled:opacity-50"
        >
          Know it
        </button>
      </div>
    </div>
  );
};

export default FlashCardPage;
