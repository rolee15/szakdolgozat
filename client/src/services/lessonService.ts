// Hardcoded URL and user ID for now
const API_BASE_URL = 'https://localhost:7161/api/lessons';
const MOCK_USER_ID = '1';

const api = {
    async getLessonsCount(): Promise<LessonCount> {
      const response = await fetch(`${API_BASE_URL}/count?userId=${MOCK_USER_ID}`);
      if (!response.ok) throw new Error('Failed to fetch lesson count');
      return response.json();
    },

    async getLessons(pageIndex: number, pageSize: number): Promise<Lesson[]> {
      const response = await fetch(`${API_BASE_URL}/?userId=${MOCK_USER_ID}&pageIndex=${pageIndex}&pageSize=${pageSize}`);
      if (!response.ok) throw new Error('Failed to fetch new lessons');
      return response.json();
    },

    async postLearnLesson(characterId: number): Promise<void> {
      await fetch(`${API_BASE_URL}/learn/${characterId}?userId=${MOCK_USER_ID}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
      });
    },

    async getLessonReviewsCount(): Promise<LessonReviewCount> {
        const response = await fetch(`${API_BASE_URL}/reviews/count?userId=${MOCK_USER_ID}`);
        if (!response.ok) throw new Error('Failed to fetch lesson review count');
        return response.json();
    },

    async getLessonReviews(): Promise<LessonReview[]> {
        const response = await fetch(`${API_BASE_URL}/reviews?userId=${MOCK_USER_ID}`);
        if (!response.ok) throw new Error('Failed to fetch lesson reviews');
        return response.json();
    },

    async postLessonReviewCheck(question: string, answer: string): Promise<boolean> {
        const repsonse = await fetch(`${API_BASE_URL}/reviews/check?userId=${MOCK_USER_ID}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ '"question"': `"${question}"`, '"answer"': `"${answer}"` }),
        });
        if (!repsonse.ok) throw new Error('Failed to post lesson review answer');
        return repsonse.json();
    }
  };

export default api;