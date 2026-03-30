import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';

vi.mock('@/services/routes', () => ({
  API_KANA_URL: 'http://api.test/characters',
}));

vi.mock('@/services/apiClient', () => ({
  apiFetch: vi.fn(),
}));

import kanaService from '@/services/kanaService';
import { apiFetch } from '@/services/apiClient';

const mockApiFetch = apiFetch as ReturnType<typeof vi.fn>;

function mockOk(data: unknown) {
  mockApiFetch.mockResolvedValue({ ok: true, json: vi.fn().mockResolvedValue(data) });
}

function mockNotOk() {
  mockApiFetch.mockResolvedValue({ ok: false, json: vi.fn() });
}

describe('kanaService', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  describe('getCharacters', () => {
    it('fetches characters by type and returns JSON', async () => {
      const payload = [
        { id: 1, symbol: 'あ', romanization: 'a', type: 0 },
        { id: 2, symbol: 'い', romanization: 'i', type: 0 },
      ];
      mockOk(payload);

      const result = await kanaService.getCharacters('hiragana');

      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith('http://api.test/characters/hiragana');
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(kanaService.getCharacters('katakana')).rejects.toThrow('Failed to fetch characters');
    });
  });

  describe('getCharacterDetail', () => {
    it('fetches character detail and returns JSON', async () => {
      const payload = { id: 1, symbol: 'あ', romanization: 'a', type: 0 };
      mockOk(payload);

      const result = await kanaService.getCharacterDetail('hiragana', 'a');

      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith('http://api.test/characters/hiragana/a');
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(kanaService.getCharacterDetail('hiragana', 'a')).rejects.toThrow('Failed to fetch character details');
    });
  });

  describe('getExamples', () => {
    it('fetches examples and returns JSON', async () => {
      const payload = [{ id: 1, text: 'ありがとう' }];
      mockOk(payload);

      const result = await kanaService.getExamples('hiragana', 'a');

      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith('http://api.test/characters/hiragana/a/examples');
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(kanaService.getExamples('hiragana', 'a')).rejects.toThrow('Failed to fetch character examples');
    });
  });
});
