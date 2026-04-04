type KanaCharacter = {
  character: string;
  romanization: string;
  type: "hiragana" | "katakana";
  proficiency: number;
  srsStage?: number;
  srsStageName?: string;
};