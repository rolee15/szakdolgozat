import { useState } from "react";
import { useQuery, useMutation } from "@tanstack/react-query";
import kanaService from "@/services/kanaService";
import kanjiService from "@/services/kanjiService";
import lessonService from "@/services/lessonService";
import styles from "./FlashCardPage.module.css";

type KanaMode = "hiragana" | "katakana" | "kanji";

// Must match the transition duration in FlashCardPage.module.css (.card { transition: transform 0.55s })
const FLIP_DURATION_MS = 550;

const FlashCardPage = () => {
  const [mode, setMode] = useState<KanaMode>("hiragana");
  const [currentIndex, setCurrentIndex] = useState(0);
  const [isFlipped, setIsFlipped] = useState(false);
  const [sessionDone, setSessionDone] = useState(false);

  const { data: kanaCharacters, isLoading: isKanaLoading, isError: isKanaError } = useQuery<KanaCharacter[]>({
    queryKey: ["flashcards", mode],
    queryFn: () => kanaService.getCharacters(mode as "hiragana" | "katakana"),
    enabled: mode !== "kanji",
  });

  const { data: kanjiReviews, isLoading: isKanjiLoading, isError: isKanjiError } = useQuery<KanjiReview[]>({
    queryKey: ["flashcards", "kanji"],
    queryFn: () => kanjiService.getKanjiReviews(),
    enabled: mode === "kanji",
  });

  const reviewMutation = useMutation<LessonReviewResult, Error, { question: string; answer: string }>({
    mutationFn: ({ question, answer }) =>
      lessonService.postLessonReviewCheck(question, answer),
  });

  const kanjiReviewMutation = useMutation<KanjiReviewResult, Error, { kanjiId: number; isCorrect: boolean }>({
    mutationFn: ({ kanjiId, isCorrect }) =>
      kanjiService.checkKanjiReview(kanjiId, isCorrect),
  });

  const isLoading = mode === "kanji" ? isKanjiLoading : isKanaLoading;
  const isError = mode === "kanji" ? isKanjiError : isKanaError;

  const cards: (KanaCharacter | KanjiReview)[] = mode === "kanji"
    ? (kanjiReviews ?? [])
    : (kanaCharacters ?? []);

  const currentCard = cards[currentIndex];
  const total = cards.length;

  const handleFlip = () => {
    setIsFlipped((prev) => !prev);
  };

  const advance = () => {
    // Flip the card back to its front face first, then change the card data only
    // after the CSS flip-back animation finishes. This prevents the next card's
    // back-face content from briefly showing through while the card is mid-rotation.
    setIsFlipped(false);
    setTimeout(() => {
      const nextIndex = currentIndex + 1;
      if (nextIndex >= total) {
        setSessionDone(true);
      } else {
        setCurrentIndex(nextIndex);
      }
    }, FLIP_DURATION_MS);
  };

  const handleKnowIt = () => {
    if (!currentCard) return;
    if (mode === "kanji") {
      const kanjiCard = currentCard as KanjiReview;
      kanjiReviewMutation.mutate(
        { kanjiId: kanjiCard.kanjiId, isCorrect: true },
        { onSettled: () => advance() }
      );
    } else {
      const kanaCard = currentCard as KanaCharacter;
      reviewMutation.mutate(
        { question: kanaCard.character, answer: kanaCard.romanization },
        { onSettled: () => advance() }
      );
    }
  };

  const handleDontKnowIt = () => {
    if (!currentCard) return;
    if (mode === "kanji") {
      const kanjiCard = currentCard as KanjiReview;
      kanjiReviewMutation.mutate(
        { kanjiId: kanjiCard.kanjiId, isCorrect: false },
        { onSettled: () => advance() }
      );
    } else {
      const kanaCard = currentCard as KanaCharacter;
      reviewMutation.mutate(
        { question: kanaCard.character, answer: "" },
        { onSettled: () => advance() }
      );
    }
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

  if (mode === "kanji" && total === 0) {
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
            onClick={() => handleModeChange("kanji")}
            className="px-5 py-2 rounded-lg font-medium capitalize transition-colors focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:ring-offset-2 bg-indigo-500 text-white"
          >
            Kanji
          </button>
        </div>

        <p className="text-gray-400 text-lg">No kanji due for review. Study some kanji first!</p>
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

  const isMutationPending = reviewMutation.isPending || kanjiReviewMutation.isPending;

  const cardFront = currentCard ? currentCard.character : undefined;
  const cardBack = mode === "kanji"
    ? (currentCard as KanjiReview | undefined)?.meaning
    : (currentCard as KanaCharacter | undefined)?.romanization;
  const cardLabel = mode === "kanji"
    ? "kanji"
    : (currentCard as KanaCharacter | undefined)?.type;

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
          onClick={() => handleModeChange("kanji")}
          className={`px-5 py-2 rounded-lg font-medium capitalize transition-colors focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:ring-offset-2 ${
            mode === "kanji"
              ? "bg-indigo-500 text-white"
              : "bg-gray-700 text-gray-300 hover:bg-gray-600"
          }`}
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
              {cardFront}
            </span>
            <span className="mt-4 text-sm text-indigo-400 select-none">Click to reveal</span>
          </div>
          <div className={`${styles.cardFace} ${styles.cardBack}`}>
            <span className="text-4xl font-bold text-white select-none">
              {cardBack}
            </span>
            <span className="mt-3 text-sm uppercase tracking-widest text-blue-400 select-none">
              {cardLabel}
            </span>
          </div>
        </div>
      </div>

      {/* Know it / Don't know it buttons */}
      <div className="flex gap-4">
        <button
          onClick={handleDontKnowIt}
          disabled={isMutationPending}
          className="px-6 py-3 bg-red-700 text-white font-medium rounded-lg hover:bg-red-800 transition-colors focus:outline-none focus:ring-2 focus:ring-red-400 focus:ring-offset-2 disabled:opacity-50"
        >
          Don't know it
        </button>
        <button
          onClick={handleKnowIt}
          disabled={isMutationPending}
          className="px-6 py-3 bg-green-700 text-white font-medium rounded-lg hover:bg-green-800 transition-colors focus:outline-none focus:ring-2 focus:ring-green-400 focus:ring-offset-2 disabled:opacity-50"
        >
          Know it
        </button>
      </div>
    </div>
  );
};

export default FlashCardPage;
