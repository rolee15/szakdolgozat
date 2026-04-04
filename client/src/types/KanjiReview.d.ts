type KanjiReview = {
  kanjiId: number;
  character: string;
  meaning: string;
  onyomiReading?: string;
  kunyomiReading?: string;
};

type KanjiReviewResult = {
  isCorrect: boolean;
  srsStage: number;
  srsStageName: string;
  nextReviewDate?: string;
};
