import { describe, it, expect, vi, beforeEach } from 'vitest';

vi.mock('@/services/apiClient', () => ({
  apiFetch: vi.fn(),
}));

import readingService from '@/services/readingService';
import { apiFetch } from '@/services/apiClient';

const mockApiFetch = apiFetch as unknown as ReturnType<typeof vi.fn>;

function makeMockResponse(ok: boolean, data: unknown): Response {
  return {
    ok,
    json: vi.fn().mockResolvedValue(data),
  } as unknown as Response;
}

beforeEach(() => {
  vi.restoreAllMocks();
});

describe('readingService.getPassages', () => {
  it('returns passages on ok response', async () => {
    const data: ReadingPassage[] = [
      { id: 1, title: 'Test', jlptLevel: 5, isPassed: false, score: 0, attemptCount: 0 },
    ];
    mockApiFetch.mockResolvedValue(makeMockResponse(true, data));

    const result = await readingService.getPassages();

    expect(result).toEqual(data);
  });

  it('throws on non-ok response', async () => {
    mockApiFetch.mockResolvedValue(makeMockResponse(false, null));

    await expect(readingService.getPassages()).rejects.toThrow('Failed to fetch reading passages');
  });
});

describe('readingService.getPassageDetail', () => {
  it('returns passage detail on ok response', async () => {
    const data: ReadingPassageDetail = {
      id: 1,
      title: 'Test',
      jlptLevel: 5,
      isPassed: false,
      score: 0,
      attemptCount: 0,
      content: 'Some Japanese text',
      source: 'Book',
      questions: [],
    };
    mockApiFetch.mockResolvedValue(makeMockResponse(true, data));

    const result = await readingService.getPassageDetail(1);

    expect(result).toEqual(data);
  });

  it('throws on non-ok response', async () => {
    mockApiFetch.mockResolvedValue(makeMockResponse(false, null));

    await expect(readingService.getPassageDetail(1)).rejects.toThrow('Failed to fetch passage detail');
  });
});

describe('readingService.submitAnswers', () => {
  it('returns result on ok response', async () => {
    const data: ReadingResult = {
      score: 100,
      isPassed: true,
      correctCount: 2,
      totalQuestions: 2,
      results: [],
    };
    mockApiFetch.mockResolvedValue(makeMockResponse(true, data));

    const result = await readingService.submitAnswers(1, { 1: 'A', 2: 'B' });

    expect(result).toEqual(data);
  });

  it('throws on non-ok response', async () => {
    mockApiFetch.mockResolvedValue(makeMockResponse(false, null));

    await expect(readingService.submitAnswers(1, {})).rejects.toThrow('Failed to submit answers');
  });
});
