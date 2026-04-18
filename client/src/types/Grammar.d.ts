type GrammarPoint = {
  id: number;
  title: string;
  pattern: string;
  jlptLevel: number;
  correctCount: number;
  attemptCount: number;
  isCompleted: boolean;
};

type GrammarPointDetail = {
  id: number;
  title: string;
  pattern: string;
  explanation: string;
  jlptLevel: number;
  correctCount: number;
  attemptCount: number;
  isCompleted: boolean;
  examples: GrammarExample[];
  exercises: GrammarExercise[];
};

type GrammarExample = {
  japanese: string;
  reading: string;
  english: string;
};

type GrammarExercise = {
  id: number;
  sentence: string;
  options: string[];
};

type GrammarExerciseResult = {
  isCorrect: boolean;
  correctAnswer: string;
  correctCount: number;
  attemptCount: number;
  isCompleted: boolean;
};
