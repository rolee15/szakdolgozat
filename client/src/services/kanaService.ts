// Hardcoded URL and user ID for now
import { API_KANA_URL } from "@/services/routes";

const MOCK_USER_ID = '1';

const api = {
    async getCharacters(type: string): Promise<KanaCharacter[]> {
      const response = await fetch(`${API_KANA_URL}/${type}?userId=${MOCK_USER_ID}`);
      if (!response.ok) throw new Error('Failed to fetch characters');
      return response.json();
    },

    async getCharacterDetail(type: string, character: string): Promise<KanaCharacter> {
      const response = await fetch(`${API_KANA_URL}/${type}/${character}?userId=${MOCK_USER_ID}`);
      if (!response.ok) throw new Error('Failed to fetch character details');
      return response.json();
    },

    async getExamples(type: string, character: string): Promise<Example[]> {
      const response = await fetch(`${API_KANA_URL}/${type}/${character}/examples?userId=${MOCK_USER_ID}`);
      if (!response.ok) throw new Error('Failed to fetch character examples');
      return response.json();
    }
  };

export default api;