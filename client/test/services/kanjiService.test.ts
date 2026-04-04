import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';

vi.mock('@/services/apiClient', () => ({
  apiFetch: vi.fn(),
}));

import kanjiService from '@/services/kanjiService';
import { apiFetch } from '@/services/apiClient';

const mockApiFetch = apiFetch as ReturnType<typeof vi.fn>;

function mockOk(data: unknown) {
  mockApiFetch.mockResolvedValue({ ok: true, json: vi.fn().mockResolvedValue(data) });
}

function mockNotOk() {
  mockApiFetch.mockResolvedValue({ ok: false, json: vi.fn() });
}

describe('kanjiService', () => {
  beforeEach(() => vi.clearAllMocks());
  afterEach(() => vi.clearAllMocks());

  describe('getKanjiByLevel', () => {
    it('fetches kanji by JLPT level and returns JSON', async () => {
      const payload = [{ character: '日', meaning: 'sun' }];
      mockOk(payload);
      const result = await kanjiService.getKanjiByLevel(5);
      expect(result).toEqual(payload);
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(kanjiService.getKanjiByLevel(5)).rejects.toThrow('Failed to fetch kanji');
    });
  });

  describe('getKanjiPaged', () => {
    it('fetches paged kanji without level filter', async () => {
      const payload = { items: [], totalCount: 0, page: 1, pageSize: 50 };
      mockOk(payload);
      const result = await kanjiService.getKanjiPaged(1, null);
      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith(expect.stringContaining('page=1'));
    });

    it('fetches paged kanji with level filter', async () => {
      mockOk({ items: [], totalCount: 0, page: 1, pageSize: 50 });
      await kanjiService.getKanjiPaged(2, 5);
      expect(mockApiFetch).toHaveBeenCalledWith(expect.stringContaining('jlptLevel=5'));
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(kanjiService.getKanjiPaged(1, null)).rejects.toThrow('Failed to fetch kanji');
    });
  });

  describe('getKanjiDetail', () => {
    it('fetches kanji detail and returns JSON', async () => {
      const payload = { character: '日', meaning: 'sun', examples: [] };
      mockOk(payload);
      const result = await kanjiService.getKanjiDetail('日');
      expect(result).toEqual(payload);
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(kanjiService.getKanjiDetail('日')).rejects.toThrow('Failed to fetch kanji detail');
    });
  });

  describe('getKanjiReviews', () => {
    it('fetches due kanji reviews and returns JSON', async () => {
      const payload = [{ kanjiId: 1, character: '日', meaning: 'sun' }];
      mockOk(payload);
      const result = await kanjiService.getKanjiReviews();
      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith(expect.stringContaining('/kanji/reviews'));
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(kanjiService.getKanjiReviews()).rejects.toThrow('Failed to fetch kanji reviews');
    });
  });

  describe('checkKanjiReview', () => {
    it('posts correct answer and returns result', async () => {
      const payload = { isCorrect: true, srsStage: 2, srsStageName: 'Apprentice 2' };
      mockOk(payload);
      const result = await kanjiService.checkKanjiReview(1, true);
      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith(
        expect.stringContaining('/kanji/reviews/check'),
        expect.objectContaining({ method: 'POST' })
      );
    });

    it('posts incorrect answer and returns result', async () => {
      mockOk({ isCorrect: false, srsStage: 1, srsStageName: 'Apprentice 1' });
      const result = await kanjiService.checkKanjiReview(1, false);
      expect(result.isCorrect).toBe(false);
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(kanjiService.checkKanjiReview(1, true)).rejects.toThrow('Failed to submit kanji review');
    });
  });
});
