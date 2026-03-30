import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';

vi.mock('@/services/routes', () => ({
  API_LESSONS_URL: 'http://api.test/lessons',
}));

vi.mock('@/services/apiClient', () => ({
  apiFetch: vi.fn(),
}));

import lessonService from '@/services/lessonService';
import { apiFetch } from '@/services/apiClient';

const mockApiFetch = apiFetch as ReturnType<typeof vi.fn>;

function mockOk(data: unknown) {
  mockApiFetch.mockResolvedValue({ ok: true, json: vi.fn().mockResolvedValue(data) });
}

function mockNotOk() {
  mockApiFetch.mockResolvedValue({ ok: false, json: vi.fn() });
}

describe('lessonService', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  describe('getLessonsCount', () => {
    it('fetches lessons count and returns JSON', async () => {
      const payload = { count: 42 };
      mockOk(payload);

      const result = await lessonService.getLessonsCount();

      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith('http://api.test/lessons/count');
    });

    it('throws when response is not ok', async () => {
      mockNotOk();
      await expect(lessonService.getLessonsCount()).rejects.toThrow('Failed to fetch lesson count');
    });
  });

  describe('getLessons', () => {
    it('fetches lessons with pagination and returns JSON', async () => {
      const payload = [
        { characterId: 1, symbol: 'あ', romanization: 'a', type: 0 },
        { characterId: 2, symbol: 'い', romanization: 'i', type: 0 },
      ];
      mockOk(payload);

      const pageIndex = 3;
      const pageSize = 25;
      const result = await lessonService.getLessons(pageIndex, pageSize);

      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith(
        `http://api.test/lessons/?pageIndex=${pageIndex}&pageSize=${pageSize}`
      );
    });

    it('throws when response is not ok', async () => {
      mockNotOk();
      await expect(lessonService.getLessons(0, 10)).rejects.toThrow('Failed to fetch new lessons');
    });
  });

  describe('postLearnLesson', () => {
    it('posts learn lesson with correct method', async () => {
      mockApiFetch.mockResolvedValue({ ok: true });

      await expect(lessonService.postLearnLesson(123)).resolves.toBeUndefined();

      expect(mockApiFetch).toHaveBeenCalledWith(
        'http://api.test/lessons/learn/123',
        expect.objectContaining({ method: 'POST' })
      );
    });
  });

  describe('getLessonReviewsCount', () => {
    it('fetches review count and returns JSON', async () => {
      const payload = { count: 7 };
      mockOk(payload);

      const result = await lessonService.getLessonReviewsCount();

      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith('http://api.test/lessons/reviews/count');
    });

    it('throws when response is not ok', async () => {
      mockNotOk();
      await expect(lessonService.getLessonReviewsCount()).rejects.toThrow('Failed to fetch lesson review count');
    });
  });

  describe('getLessonReviews', () => {
    it('fetches reviews and returns JSON', async () => {
      const payload = [{ question: 'Q1' }, { question: 'Q2' }];
      mockOk(payload);

      const result = await lessonService.getLessonReviews();

      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith('http://api.test/lessons/reviews');
    });

    it('throws when response is not ok', async () => {
      mockNotOk();
      await expect(lessonService.getLessonReviews()).rejects.toThrow('Failed to fetch lesson reviews');
    });
  });

  describe('postLessonReviewCheck', () => {
    it('posts review check and returns JSON', async () => {
      const payload = { isCorrect: true, correctAnswer: 'あ' };
      mockOk(payload);

      const question = 'What is あ?';
      const answer = 'a';
      const result = await lessonService.postLessonReviewCheck(question, answer);

      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith(
        'http://api.test/lessons/reviews/check',
        expect.objectContaining({
          method: 'POST',
          body: JSON.stringify({ question, answer }),
        })
      );
    });

    it('throws when response is not ok', async () => {
      mockNotOk();
      await expect(lessonService.postLessonReviewCheck('Q', 'A')).rejects.toThrow(
        'Failed to post lesson review answer'
      );
    });
  });
});
