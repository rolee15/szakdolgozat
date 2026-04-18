type LearningUnit = {
  id: number;
  title: string;
  description: string;
  sortOrder: number;
  contentCount: number;
  isPassed: boolean;
  bestScore: number;
  isUnlocked: boolean;
};

type UnitContentSummary = {
  contentType: string;
  contentId: number;
  title: string;
};

type LearningUnitDetail = LearningUnit & {
  contents: UnitContentSummary[];
  testQuestionCount: number;
};

type UnitTestQuestion = {
  id: number;
  questionText: string;
  options: Record<string, string>;
};

type UnitTest = {
  questions: UnitTestQuestion[];
};

type UnitTestResult = {
  score: number;
  isPassed: boolean;
  correctCount: number;
  totalQuestions: number;
};
