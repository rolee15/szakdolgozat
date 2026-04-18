type AdminUser = {
  id: number;
  username: string;
  role: string;
  mustChangePassword: boolean;
  proficiencyCount: number;
  lessonCompletionCount: number;
};

type ProficiencySummary = {
  characterId: number;
  characterSymbol: string;
  learnedAt: string;
  lastPracticed: string;
};

type LessonCompletionSummary = {
  characterId: number;
  characterSymbol: string;
  completionDate: string;
};

type AdminUserDetail = AdminUser & {
  proficiencies: ProficiencySummary[];
  lessonCompletions: LessonCompletionSummary[];
};
