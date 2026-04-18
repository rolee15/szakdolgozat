import { apiFetch } from '@/services/apiClient';

const API_BASE = import.meta.env.VITE_API_URL ?? 'https://localhost:7161/api';

const kanjiService = {
  async getKanjiByLevel(jlptLevel: number): Promise<KanjiCharacter[]> {
    const res = await apiFetch(`${API_BASE}/kanji/level/${jlptLevel}`);
    if (!res.ok) throw new Error('Failed to fetch kanji');
    return res.json();
  },

  async getKanjiPaged(page: number, jlptLevel: number | null): Promise<PagedResult<KanjiCharacter>> {
    const levelParam = jlptLevel !== null ? `&jlptLevel=${jlptLevel}` : '';
    const res = await apiFetch(`${API_BASE}/kanji?page=${page}&pageSize=50${levelParam}`);
    if (!res.ok) throw new Error('Failed to fetch kanji');
    return res.json();
  },

  async getKanjiDetail(character: string): Promise<KanjiDetail> {
    const res = await apiFetch(`${API_BASE}/kanji/${encodeURIComponent(character)}`);
    if (!res.ok) throw new Error('Failed to fetch kanji detail');
    return res.json();
  },

  async getKanjiReviews(): Promise<KanjiReview[]> {
    const res = await apiFetch(`${API_BASE}/kanji/reviews`);
    if (!res.ok) throw new Error('Failed to fetch kanji reviews');
    return res.json();
  },

  async checkKanjiReview(kanjiId: number, isCorrect: boolean): Promise<KanjiReviewResult> {
    const res = await apiFetch(`${API_BASE}/kanji/reviews/check`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ kanjiId, isCorrect }),
    });
    if (!res.ok) throw new Error('Failed to submit kanji review');
    return res.json();
  },
};

export default kanjiService;
