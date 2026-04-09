export interface MoveResult {
  isValid: boolean;
  reason?: string;
  nextChar?: string;
  computerWord?: string;
  gameOver: boolean;
  winner?: string;
}

export type GameStatus = 'idle' | 'connecting' | 'playing' | 'over';

export interface ShiritoriState {
  status: GameStatus;
  currentWord: string;
  requiredChar: string;
  history: Array<{ word: string; by: 'player' | 'computer' }>;
  winner?: string;
  error?: string;
}
