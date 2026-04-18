import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';

vi.mock('@/services/apiClient', () => ({
  apiFetch: vi.fn(),
}));

import grammarService from '@/services/grammarService';
import { apiFetch } from '@/services/apiClient';

const mockApiFetch = apiFetch as ReturnType<typeof vi.fn>;

function mockOk(data: unknown) {
  mockApiFetch.mockResolvedValue({ ok: true, json: vi.fn().mockResolvedValue(data) });
}

function mockNotOk() {
  mockApiFetch.mockResolvedValue({ ok: false, json: vi.fn() });
}

describe('grammarService', () => {
  beforeEach(() => vi.clearAllMocks());
  afterEach(() => vi.clearAllMocks());

  describe('getGrammarPoints', () => {
    it('fetches grammar points and returns JSON', async () => {
      const payload: GrammarPoint[] = [
        {
          id: 1,
          title: 'は (wa) — Topic Marker',
          pattern: 'Noun + は + Predicate',
          jlptLevel: 5,
          correctCount: 2,
          attemptCount: 3,
          isCompleted: false,
        },
      ];
      mockOk(payload);
      const result = await grammarService.getGrammarPoints();
      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith(expect.stringContaining('/grammar'));
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(grammarService.getGrammarPoints()).rejects.toThrow(
        'Failed to fetch grammar points',
      );
    });
  });

  describe('getGrammarDetail', () => {
    it('fetches grammar detail by id and returns JSON', async () => {
      const payload: GrammarPointDetail = {
        id: 1,
        title: 'は (wa) — Topic Marker',
        pattern: 'Noun + は + Predicate',
        explanation: 'Marks the topic of the sentence.',
        jlptLevel: 5,
        correctCount: 0,
        attemptCount: 0,
        isCompleted: false,
        examples: [],
        exercises: [],
      };
      mockOk(payload);
      const result = await grammarService.getGrammarDetail(1);
      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith(expect.stringContaining('/grammar/1'));
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(grammarService.getGrammarDetail(1)).rejects.toThrow(
        'Failed to fetch grammar detail',
      );
    });
  });

  describe('checkExercise', () => {
    it('posts exercise answer and returns result', async () => {
      const payload: GrammarExerciseResult = {
        isCorrect: true,
        correctAnswer: 'は',
        correctCount: 1,
        attemptCount: 1,
        isCompleted: false,
      };
      mockOk(payload);
      const result = await grammarService.checkExercise(1, 10, 'は');
      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith(
        expect.stringContaining('/grammar/1/exercises/check'),
        expect.objectContaining({ method: 'POST' }),
      );
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(grammarService.checkExercise(1, 10, 'は')).rejects.toThrow(
        'Failed to check exercise',
      );
    });
  });
});
