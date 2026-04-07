import { API_KANA_URL } from "@/services/routes";
import { apiFetch } from "@/services/apiClient";

const api = {
    async getCharacters(): Promise<KanaCharacter[]> {
      const response = await apiFetch(`${API_KANA_URL}/hiragana`);
      if (!response.ok) throw new Error('Failed to fetch characters');
      return response.json();
    },

    async getCharacterDetail(character: string): Promise<KanaCharacter> {
      const response = await apiFetch(`${API_KANA_URL}/hiragana/${character}`);
      if (!response.ok) throw new Error('Failed to fetch character details');
      return response.json();
    },

    async getExamples(character: string): Promise<Example[]> {
      const response = await apiFetch(`${API_KANA_URL}/hiragana/${character}/examples`);
      if (!response.ok) throw new Error('Failed to fetch character examples');
      return response.json();
    }
  };

export default api;
