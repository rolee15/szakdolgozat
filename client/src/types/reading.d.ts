type ReadingPassage = {
  id: number;
  title: string;
  jlptLevel: number;
  isPassed: boolean;
  score: number;
  attemptCount: number;
};

type ComprehensionQuestion = {
  id: number;
  questionText: string;
  options: Record<string, string>;
};

type ReadingPassageDetail = ReadingPassage & {
  content: string;
  source: string;
  questions: ComprehensionQuestion[];
};

type ReadingResult = {
  score: number;
  isPassed: boolean;
  correctCount: number;
  totalQuestions: number;
  results: QuestionResult[];
};

type QuestionResult = {
  questionId: number;
  isCorrect: boolean;
  correctOption: string;
  chosenOption: string;
};
