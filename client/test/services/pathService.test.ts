import { describe, it, expect, vi, beforeEach } from 'vitest';

vi.mock('@/services/apiClient', () => ({
  apiFetch: vi.fn(),
}));

import pathService from '@/services/pathService';
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

describe('pathService.getPath', () => {
  it('returns units on ok response', async () => {
    const data: LearningUnit[] = [
      {
        id: 1,
        title: 'Unit 1',
        description: 'Intro',
        sortOrder: 1,
        contentCount: 5,
        isPassed: false,
        bestScore: 0,
        isUnlocked: true,
      },
    ];
    mockApiFetch.mockResolvedValue(makeMockResponse(true, data));

    const result = await pathService.getPath();

    expect(result).toEqual(data);
  });

  it('throws on non-ok response', async () => {
    mockApiFetch.mockResolvedValue(makeMockResponse(false, null));

    await expect(pathService.getPath()).rejects.toThrow('Failed to fetch learning path');
  });
});

describe('pathService.getUnitDetail', () => {
  it('returns unit detail on ok response', async () => {
    const data: LearningUnitDetail = {
      id: 1,
      title: 'Unit 1',
      description: 'Intro',
      sortOrder: 1,
      contentCount: 2,
      isPassed: false,
      bestScore: 0,
      isUnlocked: true,
      contents: [],
      testQuestionCount: 5,
    };
    mockApiFetch.mockResolvedValue(makeMockResponse(true, data));

    const result = await pathService.getUnitDetail(1);

    expect(result).toEqual(data);
  });

  it('throws on non-ok response', async () => {
    mockApiFetch.mockResolvedValue(makeMockResponse(false, null));

    await expect(pathService.getUnitDetail(1)).rejects.toThrow('Failed to fetch unit detail');
  });
});

describe('pathService.getUnitTest', () => {
  it('returns test on ok response', async () => {
    const data: UnitTest = {
      questions: [
        { id: 1, questionText: 'Q1?', options: { A: 'opt1', B: 'opt2' } },
      ],
    };
    mockApiFetch.mockResolvedValue(makeMockResponse(true, data));

    const result = await pathService.getUnitTest(1);

    expect(result).toEqual(data);
  });

  it('throws on non-ok response', async () => {
    mockApiFetch.mockResolvedValue(makeMockResponse(false, null));

    await expect(pathService.getUnitTest(1)).rejects.toThrow('Failed to fetch unit test');
  });
});

describe('pathService.submitTest', () => {
  it('returns result on ok response', async () => {
    const data: UnitTestResult = {
      score: 80,
      isPassed: true,
      correctCount: 4,
      totalQuestions: 5,
    };
    mockApiFetch.mockResolvedValue(makeMockResponse(true, data));

    const result = await pathService.submitTest(1, { 1: 'A' });

    expect(result).toEqual(data);
  });

  it('throws on non-ok response', async () => {
    mockApiFetch.mockResolvedValue(makeMockResponse(false, null));

    await expect(pathService.submitTest(1, {})).rejects.toThrow('Failed to submit test');
  });
});
