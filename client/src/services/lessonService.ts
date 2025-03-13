// Hardcoded URL and user ID for now
const API_BASE_URL = 'https://localhost:7161/api/lessons';
const MOCK_USER_ID = '1';

const api = {
    async getTodayLessonCount(): Promise<LessonCount> {
      const response = await fetch(`${API_BASE_URL}/todayCount?userId=${MOCK_USER_ID}`);
      if (!response.ok) throw new Error('Failed to fetch lesson count');
      return response.json();
    },

    async getNewLessons(pageIndex: number, pageSize: number): Promise<Lesson[]> {
      const response = await fetch(`${API_BASE_URL}/?userId=${MOCK_USER_ID}&pageIndex=${pageIndex}&pageSize=${pageSize}`);
      if (!response.ok) throw new Error('Failed to fetch new lessons');
      return response.json();
    },

    async postLearnLesson(characterId: string): Promise<void> {
      const response = await fetch(`${API_BASE_URL}/learnLesson/${characterId}?userId=${MOCK_USER_ID}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
      });
      if (!response.ok) throw new Error('Failed to post learn lesson');
      return response.json();
    },

    async getLessonReviews(): Promise<LessonReview[]> {
        const response = await fetch(`${API_BASE_URL}/reviews?userId=${MOCK_USER_ID}`);
        if (!response.ok) throw new Error('Failed to fetch lesson reviews');
        return response.json();
    },

    async postLessonReviewAnswer(character: string, answer: string): Promise<boolean> {
        const repsonse = await fetch(`${API_BASE_URL}/reviews/items/${character}?userId=${MOCK_USER_ID}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ '"answer"': `"${answer}"` }),
        });
        console.log(repsonse);
        if (!repsonse.ok) throw new Error('Failed to post lesson review answer');
        return repsonse.json();
    }
  };

export default api;