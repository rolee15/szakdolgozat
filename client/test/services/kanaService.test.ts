import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';

vi.mock('@/services/routes', () => ({
  API_KANA_URL: 'http://api.test/characters',
}));

import kanaService from '@/services/kanaService';

function mockFetchOk(data: unknown) {
  const json = vi.fn().mockResolvedValue(data);
  const okResponse = { ok: true, json } as unknown as Response;
  (globalThis as { fetch: typeof fetch }).fetch = vi.fn().mockResolvedValue(okResponse) as unknown as typeof fetch;
}

function mockFetchNotOk() {
  const notOkResponse = { ok: false, json: vi.fn() } as unknown as Response;
  (globalThis as { fetch: typeof fetch }).fetch = vi.fn().mockResolvedValue(notOkResponse) as unknown as typeof fetch;
}

describe('kanaService', () => {
  beforeEach(() => {
    vi.restoreAllMocks();
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
      mockFetchOk(payload);

      const type = 'hiragana';
      const result = await kanaService.getCharacters(type);

      expect(result).toEqual(payload);
      expect(global.fetch).toHaveBeenCalledTimes(1);
      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const url = fetchMock.mock.calls[0][0] as string;
      expect(url).toBe(`http://api.test/characters/${type}?userId=1`);
    });

    it('throws on non-ok response', async () => {
      mockFetchNotOk();
      await expect(kanaService.getCharacters('katakana')).rejects.toThrow('Failed to fetch characters');
    });
  });

  describe('getCharacterDetail', () => {
    it('fetches character detail and returns JSON', async () => {
      const payload = { id: 1, symbol: 'あ', romanization: 'a', type: 0 };
      mockFetchOk(payload);

      const type = 'hiragana';
      const char = 'a';
      const result = await kanaService.getCharacterDetail(type, char);

      expect(result).toEqual(payload);
      expect(global.fetch).toHaveBeenCalledTimes(1);
      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const url = fetchMock.mock.calls[0][0] as string;
      expect(url).toBe(`http://api.test/characters/${type}/${char}?userId=1`);
    });

    it('throws on non-ok response', async () => {
      mockFetchNotOk();
      await expect(kanaService.getCharacterDetail('hiragana', 'a')).rejects.toThrow('Failed to fetch character details');
    });
  });

  describe('getExamples', () => {
    it('fetches examples and returns JSON', async () => {
      const payload = [
        { id: 1, text: 'ありがとう' },
      ];
      mockFetchOk(payload);

      const type = 'hiragana';
      const char = 'a';
      const result = await kanaService.getExamples(type, char);

      expect(result).toEqual(payload);
      expect(global.fetch).toHaveBeenCalledTimes(1);
      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const url = fetchMock.mock.calls[0][0] as string;
      expect(url).toBe(`http://api.test/characters/${type}/${char}/examples?userId=1`);
    });

    it('throws on non-ok response', async () => {
      mockFetchNotOk();
      await expect(kanaService.getExamples('hiragana', 'a')).rejects.toThrow('Failed to fetch character examples');
    });
  });
});
