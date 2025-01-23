// Hardcoded URL and user ID for now
const API_BASE_URL = 'https://localhost:7161/api/characters';
const MOCK_USER_ID = 'user1';

const api = {
    async getCharacters(type: string): Promise<KanaCharacter[]> {
      const response = await fetch(`${API_BASE_URL}/${type}?userId=${MOCK_USER_ID}`);
      if (!response.ok) throw new Error('Failed to fetch characters');
      return response.json();
    },

    async getCharacterDetail(type: string, character: string): Promise<KanaCharacter> {
      const response = await fetch(`${API_BASE_URL}/${type}/${character}?userId=${MOCK_USER_ID}`);
      if (!response.ok) throw new Error('Failed to fetch character details');
      return response.json();
    },

    async getExamples(type: string, character: string): Promise<string[]> {
      const response = await fetch(`${API_BASE_URL}/${type}/${character}/examples?userId=${MOCK_USER_ID}`);
      if (!response.ok) throw new Error('Failed to fetch character examples');
      return response.json();
    }
  };

export default api;