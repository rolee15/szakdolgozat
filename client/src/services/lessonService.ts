import { API_LESSONS_URL } from "@/services/routes";
import { apiFetch } from "@/services/apiClient";

const api = {
    async getLessonsCount(): Promise<LessonCount> {
      const response = await apiFetch(`${API_LESSONS_URL}/count`);
      if (!response.ok) throw new Error('Failed to fetch lesson count');
      return response.json();
    },

    async getLessons(pageIndex: number, pageSize: number): Promise<Lesson[]> {
      const response = await apiFetch(`${API_LESSONS_URL}/?pageIndex=${pageIndex}&pageSize=${pageSize}`);
      if (!response.ok) throw new Error('Failed to fetch new lessons');
      return response.json();
    },

    async postLearnLesson(characterId: number): Promise<void> {
      await apiFetch(`${API_LESSONS_URL}/learn/${characterId}`, {
        method: 'POST',
      });
    },

    async getLessonReviewsCount(): Promise<LessonReviewCount> {
        const response = await apiFetch(`${API_LESSONS_URL}/reviews/count`);
        if (!response.ok) throw new Error('Failed to fetch lesson review count');
        return response.json();
    },

    async getLessonReviews(): Promise<LessonReview[]> {
        const response = await apiFetch(`${API_LESSONS_URL}/reviews`);
        if (!response.ok) throw new Error('Failed to fetch lesson reviews');
        return response.json();
    },

    async postLessonReviewCheck(question: string, answer: string): Promise<LessonReviewResult> {
        const response = await apiFetch(`${API_LESSONS_URL}/reviews/check`, {
            method: 'POST',
            body: JSON.stringify({ question, answer }),
        });
        if (!response.ok) throw new Error('Failed to post lesson review answer');
        return response.json();
    },

    async getWritingReviewsCount(): Promise<LessonReviewCount> {
        const response = await apiFetch(`${API_LESSONS_URL}/writing-reviews/count`);
        if (!response.ok) throw new Error('Failed to fetch writing review count');
        return response.json();
    },

    async getWritingReviews(): Promise<WritingReview[]> {
        const response = await apiFetch(`${API_LESSONS_URL}/writing-reviews`);
        if (!response.ok) throw new Error('Failed to fetch writing reviews');
        return response.json();
    },

    async postWritingReviewCheck(characterId: number, typedCharacter: string): Promise<LessonReviewResult> {
        const response = await apiFetch(`${API_LESSONS_URL}/writing-reviews/check`, {
            method: 'POST',
            body: JSON.stringify({ characterId, typedCharacter }),
        });
        if (!response.ok) throw new Error('Failed to post writing review answer');
        return response.json();
    }
  };

export default api;
