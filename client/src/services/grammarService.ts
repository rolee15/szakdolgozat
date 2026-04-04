import { apiFetch } from '@/services/apiClient';

const API_BASE = import.meta.env.VITE_API_URL ?? 'https://localhost:7161/api';

const grammarService = {
  async getGrammarPoints(): Promise<GrammarPoint[]> {
    const res = await apiFetch(`${API_BASE}/grammar`);
    if (!res.ok) throw new Error('Failed to fetch grammar points');
    return res.json();
  },

  async getGrammarDetail(id: number): Promise<GrammarPointDetail> {
    const res = await apiFetch(`${API_BASE}/grammar/${id}`);
    if (!res.ok) throw new Error('Failed to fetch grammar detail');
    return res.json();
  },

  async checkExercise(
    grammarPointId: number,
    exerciseId: number,
    answer: string,
  ): Promise<GrammarExerciseResult> {
    const res = await apiFetch(
      `${API_BASE}/grammar/${grammarPointId}/exercises/check`,
      {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ exerciseId, answer }),
      },
    );
    if (!res.ok) throw new Error('Failed to check exercise');
    return res.json();
  },
};

export default grammarService;
