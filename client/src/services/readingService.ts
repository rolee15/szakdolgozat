import { apiFetch } from '@/services/apiClient';

const API_BASE = import.meta.env.VITE_API_URL ?? 'https://localhost:7161/api';

const readingService = {
  async getPassages(): Promise<ReadingPassage[]> {
    const res = await apiFetch(`${API_BASE}/reading`);
    if (!res.ok) throw new Error('Failed to fetch reading passages');
    return res.json();
  },

  async getPassageDetail(id: number): Promise<ReadingPassageDetail> {
    const res = await apiFetch(`${API_BASE}/reading/${id}`);
    if (!res.ok) throw new Error('Failed to fetch passage detail');
    return res.json();
  },

  async submitAnswers(
    id: number,
    answers: Record<number, string>,
  ): Promise<ReadingResult> {
    const res = await apiFetch(`${API_BASE}/reading/${id}/submit`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ answers }),
    });
    if (!res.ok) throw new Error('Failed to submit answers');
    return res.json();
  },
};

export default readingService;
