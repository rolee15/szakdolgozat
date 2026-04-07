import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';

vi.mock('@/services/routes', () => ({
  API_KANA_URL: 'http://api.test/characters',
}));

vi.mock('@/services/apiClient', () => ({
  apiFetch: vi.fn(),
}));

import katakanaService from '@/services/katakanaService';
import { apiFetch } from '@/services/apiClient';

const mockApiFetch = apiFetch as ReturnType<typeof vi.fn>;

function mockOk(data: unknown) {
  mockApiFetch.mockResolvedValue({ ok: true, json: vi.fn().mockResolvedValue(data) });
}

function mockNotOk() {
  mockApiFetch.mockResolvedValue({ ok: false, json: vi.fn() });
}

describe('katakanaService', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  describe('getCharacters', () => {
    it('fetches katakana characters and returns JSON', async () => {
      const payload = [
        { id: 1, symbol: 'ア', romanization: 'a', type: 1 },
        { id: 2, symbol: 'イ', romanization: 'i', type: 1 },
      ];
      mockOk(payload);

      const result = await katakanaService.getCharacters();

      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith('http://api.test/characters/katakana');
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(katakanaService.getCharacters()).rejects.toThrow('Failed to fetch characters');
    });
  });

  describe('getCharacterDetail', () => {
    it('fetches character detail and returns JSON', async () => {
      const payload = { id: 1, symbol: 'ア', romanization: 'a', type: 1 };
      mockOk(payload);

      const result = await katakanaService.getCharacterDetail('a');

      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith('http://api.test/characters/katakana/a');
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(katakanaService.getCharacterDetail('a')).rejects.toThrow('Failed to fetch character details');
    });
  });

  describe('getExamples', () => {
    it('fetches examples and returns JSON', async () => {
      const payload = [{ id: 1, text: 'アイス' }];
      mockOk(payload);

      const result = await katakanaService.getExamples('a');

      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith('http://api.test/characters/katakana/a/examples');
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(katakanaService.getExamples('a')).rejects.toThrow('Failed to fetch character examples');
    });
  });
});
