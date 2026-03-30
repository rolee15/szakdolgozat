type KanjiExample = {
  word: string;
  reading: string;
  meaning: string;
};

type KanjiDetail = {
  character: string;
  meaning: string;
  onyomiReading: string;
  kunyomiReading: string;
  strokeCount: number;
  jlptLevel: number;
  grade: number;
  proficiency: number;
  srsStage: string;
  examples: KanjiExample[];
};
