import { apiFetch } from '@/services/apiClient';
import { API_PATH_URL } from '@/services/routes';

const pathService = {
  async getPath(): Promise<LearningUnit[]> {
    const res = await apiFetch(`${API_PATH_URL}`);
    if (!res.ok) throw new Error('Failed to fetch learning path');
    return res.json();
  },

  async getUnitDetail(id: number): Promise<LearningUnitDetail> {
    const res = await apiFetch(`${API_PATH_URL}/${id}`);
    if (!res.ok) throw new Error('Failed to fetch unit detail');
    return res.json();
  },

  async getUnitTest(id: number): Promise<UnitTest> {
    const res = await apiFetch(`${API_PATH_URL}/${id}/test`);
    if (!res.ok) throw new Error('Failed to fetch unit test');
    return res.json();
  },

  async submitTest(
    id: number,
    answers: Record<number, string>,
  ): Promise<UnitTestResult> {
    const res = await apiFetch(`${API_PATH_URL}/${id}/test`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ answers }),
    });
    if (!res.ok) throw new Error('Failed to submit test');
    return res.json();
  },
};

export default pathService;
