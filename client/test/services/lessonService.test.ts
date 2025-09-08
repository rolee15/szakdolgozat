import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';

// Mock the routes to avoid dependency on import.meta.env
vi.mock('@/services/routes', () => ({
  API_LESSONS_URL: 'http://api.test/lessons',
}));

import lessonService from '@/services/lessonService';

// Helpers to build fetch mocks
function mockFetchOk(data: unknown) {
  const json = vi.fn().mockResolvedValue(data);
  const okResponse = { ok: true, json } as unknown as Response;
  (globalThis as { fetch: typeof fetch }).fetch = vi
    .fn()
    .mockResolvedValue(okResponse) as unknown as typeof fetch;
}

function mockFetchNotOk() {
  const notOkResponse = { ok: false, json: vi.fn() } as unknown as Response;
  (globalThis as { fetch: typeof fetch }).fetch = vi
    .fn()
    .mockResolvedValue(notOkResponse) as unknown as typeof fetch;
}

describe('lessonService', () => {
  beforeEach(() => {
    vi.restoreAllMocks();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  describe('getLessonsCount', () => {
    it('fetches lessons count and returns JSON', async () => {
      const payload = { count: 42 };
      mockFetchOk(payload);

      const result = await lessonService.getLessonsCount();

      expect(result).toEqual(payload);
      expect(global.fetch).toHaveBeenCalledTimes(1);
      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const url = fetchMock.mock.calls[0][0] as string;
      expect(url).toBe('http://api.test/lessons/count?userId=1');
    });

    it('throws when response is not ok', async () => {
      mockFetchNotOk();
      await expect(lessonService.getLessonsCount()).rejects.toThrow('Failed to fetch lesson count');
    });
  });

  describe('getLessons', () => {
    it('fetches lessons with pagination and returns JSON', async () => {
      const payload = [
        { characterId: 1, symbol: 'あ', romanization: 'a', type: 0 },
        { characterId: 2, symbol: 'い', romanization: 'i', type: 0 },
      ];
      mockFetchOk(payload);

      const pageIndex = 3;
      const pageSize = 25;
      const result = await lessonService.getLessons(pageIndex, pageSize);

      expect(result).toEqual(payload);
      expect(global.fetch).toHaveBeenCalledTimes(1);
      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const url = fetchMock.mock.calls[0][0] as string;
      expect(url).toBe(`http://api.test/lessons/?userId=1&pageIndex=${pageIndex}&pageSize=${pageSize}`);
    });

    it('throws when response is not ok', async () => {
      mockFetchNotOk();
      await expect(lessonService.getLessons(0, 10)).rejects.toThrow('Failed to fetch new lessons');
    });
  });

  describe('postLearnLesson', () => {
    it('posts learn lesson with correct method and headers', async () => {
      // This endpoint does not check response.ok in the service
      const okResponse = { ok: false } as unknown as Response; // ok value shouldn't matter here
      (globalThis as { fetch: typeof fetch }).fetch = vi
        .fn()
        .mockResolvedValue(okResponse) as unknown as typeof fetch;

      await expect(lessonService.postLearnLesson(123)).resolves.toBeUndefined();

      expect(global.fetch).toHaveBeenCalledTimes(1);
      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit];
      expect(url).toBe('http://api.test/lessons/learn/123?userId=1');
      expect(init?.method).toBe('POST');
      expect(init?.headers).toEqual({ 'Content-Type': 'application/json' });
    });
  });

  describe('getLessonReviewsCount', () => {
    it('fetches review count and returns JSON', async () => {
      const payload = { count: 7 };
      mockFetchOk(payload);

      const result = await lessonService.getLessonReviewsCount();

      expect(result).toEqual(payload);
      expect(global.fetch).toHaveBeenCalledTimes(1);
      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const url = fetchMock.mock.calls[0][0] as string;
      expect(url).toBe('http://api.test/lessons/reviews/count?userId=1');
    });

    it('throws when response is not ok', async () => {
      mockFetchNotOk();
      await expect(lessonService.getLessonReviewsCount()).rejects.toThrow('Failed to fetch lesson review count');
    });
  });

  describe('getLessonReviews', () => {
    it('fetches reviews and returns JSON', async () => {
      const payload = [
        { question: 'Q1' },
        { question: 'Q2' },
      ];
      mockFetchOk(payload);

      const result = await lessonService.getLessonReviews();

      expect(result).toEqual(payload);
      expect(global.fetch).toHaveBeenCalledTimes(1);
      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const url = fetchMock.mock.calls[0][0] as string;
      expect(url).toBe('http://api.test/lessons/reviews?userId=1');
    });

    it('throws when response is not ok', async () => {
      mockFetchNotOk();
      await expect(lessonService.getLessonReviews()).rejects.toThrow('Failed to fetch lesson reviews');
    });
  });

  describe('postLessonReviewCheck', () => {
    it('posts review check and returns JSON', async () => {
      const payload = { isCorrect: true, correctAnswer: 'あ' };
      mockFetchOk(payload);

      const question = 'What is あ?';
      const answer = 'a';
      const result = await lessonService.postLessonReviewCheck(question, answer);

      expect(result).toEqual(payload);
      expect(global.fetch).toHaveBeenCalledTimes(1);
      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit];
      expect(url).toBe('http://api.test/lessons/reviews/check?userId=1');
      expect(init?.method).toBe('POST');
      expect(init?.headers).toEqual({ 'Content-Type': 'application/json' });
      expect(init?.body).toBe(JSON.stringify({ question, answer }));
    });

    it('throws when response is not ok', async () => {
      mockFetchNotOk();
      await expect(lessonService.postLessonReviewCheck('Q', 'A')).rejects.toThrow('Failed to post lesson review answer');
    });
  });
});
