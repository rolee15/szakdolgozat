import { apiFetch } from '@/services/apiClient';

const API_BASE = import.meta.env.VITE_API_URL ?? 'https://localhost:7161/api';

const kanjiService = {
  async getKanjiByLevel(jlptLevel: number): Promise<KanjiCharacter[]> {
    const res = await apiFetch(`${API_BASE}/kanji/level/${jlptLevel}`);
    if (!res.ok) throw new Error('Failed to fetch kanji');
    return res.json();
  },

  async getKanjiDetail(character: string): Promise<KanjiDetail> {
    const res = await apiFetch(`${API_BASE}/kanji/${encodeURIComponent(character)}`);
    if (!res.ok) throw new Error('Failed to fetch kanji detail');
    return res.json();
  },
};

export default kanjiService;
