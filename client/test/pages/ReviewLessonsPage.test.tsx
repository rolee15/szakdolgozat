import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import ReviewLessonsPage from '@/pages/ReviewLessonsPage'

// Mock the input component to simplify submission flow
vi.mock('@/components/lessons/LessonReviewInput', () => ({
  default: ({ onSubmit }: { onSubmit: (a: string) => void }) => (
    <button onClick={() => onSubmit('ans')}>SubmitMock</button>
  ),
}))

vi.mock('@/services/lessonService', () => ({
  default: {
    getLessonReviews: vi.fn(),
    postLessonReviewCheck: vi.fn(),
  }
}))

import lessonService from '@/services/lessonService'

describe('ReviewLessonsPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })
  afterEach(() => {
    vi.clearAllMocks()
  })

  it('shows no items message when empty', async () => {
    const svc = lessonService as unknown as { getLessonReviews: ReturnType<typeof vi.fn> }
    svc.getLessonReviews.mockResolvedValue([])

    render(<ReviewLessonsPage />)

    expect(await screen.findByText(/no more items to review/i)).toBeInTheDocument()
  })

  it('submits, shows feedback, and advances on click', async () => {
    const svc = lessonService as unknown as { getLessonReviews: ReturnType<typeof vi.fn>, postLessonReviewCheck: ReturnType<typeof vi.fn> }
    svc.getLessonReviews.mockResolvedValue([{ question: 'Q1' }, { question: 'Q2' }])
    svc.postLessonReviewCheck.mockResolvedValue({ isCorrect: true, correctAnswer: 'a' })

    render(<ReviewLessonsPage />)

    // initial question
    expect(await screen.findByText('Q1')).toBeInTheDocument()

    // click the mocked submit button -> triggers onSubmit('ans')
    fireEvent.click(screen.getByRole('button', { name: 'SubmitMock' }))

    // Feedback renders for the correct answer
    expect(await screen.findByText(/correct!/i)).toBeInTheDocument()

    // Click Continue button to advance
    fireEvent.click(screen.getByRole('button', { name: /continue/i }))

    // The next question should appear
    expect(await screen.findByText('Q2')).toBeInTheDocument()
  })

  it('shows incorrect feedback with correct answer when answer is wrong', async () => {
    const svc = lessonService as unknown as { getLessonReviews: ReturnType<typeof vi.fn>, postLessonReviewCheck: ReturnType<typeof vi.fn> }
    svc.getLessonReviews.mockResolvedValue([{ question: 'Q1' }, { question: 'Q2' }])
    svc.postLessonReviewCheck.mockResolvedValue({ isCorrect: false, correctAnswer: 'correct_ans' })

    render(<ReviewLessonsPage />)

    expect(await screen.findByText('Q1')).toBeInTheDocument()

    fireEvent.click(screen.getByRole('button', { name: 'SubmitMock' }))

    expect(await screen.findByText(/incorrect/i)).toBeInTheDocument()
    expect(await screen.findByText('correct_ans')).toBeInTheDocument()
  })

  it('shows error when getLessonReviews fails', async () => {
    const svc = lessonService as unknown as { getLessonReviews: ReturnType<typeof vi.fn> }
    svc.getLessonReviews.mockRejectedValue(new Error('Network failure'))

    render(<ReviewLessonsPage />)

    expect(await screen.findByText(/error: network failure/i)).toBeInTheDocument()
  })

  it('shows error when postLessonReviewCheck fails', async () => {
    const svc = lessonService as unknown as { getLessonReviews: ReturnType<typeof vi.fn>, postLessonReviewCheck: ReturnType<typeof vi.fn> }
    svc.getLessonReviews.mockResolvedValue([{ question: 'Q1' }])
    svc.postLessonReviewCheck.mockRejectedValue(new Error('Check failed'))

    render(<ReviewLessonsPage />)

    expect(await screen.findByText('Q1')).toBeInTheDocument()

    fireEvent.click(screen.getByRole('button', { name: 'SubmitMock' }))

    expect(await screen.findByText(/error: check failed/i)).toBeInTheDocument()
  })

  it('advances to next item after clicking continue on incorrect answer', async () => {
    const svc = lessonService as unknown as { getLessonReviews: ReturnType<typeof vi.fn>, postLessonReviewCheck: ReturnType<typeof vi.fn> }
    svc.getLessonReviews.mockResolvedValue([{ question: 'Q1' }, { question: 'Q2' }])
    svc.postLessonReviewCheck.mockResolvedValue({ isCorrect: false, correctAnswer: 'correct_ans' })

    render(<ReviewLessonsPage />)

    expect(await screen.findByText('Q1')).toBeInTheDocument()

    fireEvent.click(screen.getByRole('button', { name: 'SubmitMock' }))
    expect(await screen.findByText(/incorrect/i)).toBeInTheDocument()

    // Click Continue — should advance to next item (incorrect branch: index increments)
    fireEvent.click(screen.getByRole('button', { name: /continue/i }))

    expect(await screen.findByText('Q2')).toBeInTheDocument()
  })

  it('keeps index within bounds when correct answer removes non-last item', async () => {
    const svc = lessonService as unknown as { getLessonReviews: ReturnType<typeof vi.fn>, postLessonReviewCheck: ReturnType<typeof vi.fn> }
    // 3 items, start at index 0; after removing item 0, the updated length is 2, index 0 < 2 — no adjustment needed
    svc.getLessonReviews.mockResolvedValue([{ question: 'Q1' }, { question: 'Q2' }, { question: 'Q3' }])
    svc.postLessonReviewCheck.mockResolvedValue({ isCorrect: true, correctAnswer: 'a' })

    render(<ReviewLessonsPage />)

    expect(await screen.findByText('Q1')).toBeInTheDocument()
    fireEvent.click(screen.getByRole('button', { name: 'SubmitMock' }))
    fireEvent.click(await screen.findByRole('button', { name: /continue/i }))

    // After removing Q1 at index 0, Q2 is now at index 0 — still within bounds
    expect(await screen.findByText('Q2')).toBeInTheDocument()
  })

  it('uses fallback message when non-Error is thrown from getLessonReviews', async () => {
    const svc = lessonService as unknown as { getLessonReviews: ReturnType<typeof vi.fn> }
    svc.getLessonReviews.mockRejectedValue('string error')

    render(<ReviewLessonsPage />)

    expect(await screen.findByText(/failed to load lesson reviews/i)).toBeInTheDocument()
  })

  it('uses fallback message when non-Error is thrown from postLessonReviewCheck', async () => {
    const svc = lessonService as unknown as { getLessonReviews: ReturnType<typeof vi.fn>, postLessonReviewCheck: ReturnType<typeof vi.fn> }
    svc.getLessonReviews.mockResolvedValue([{ question: 'Q1' }])
    svc.postLessonReviewCheck.mockRejectedValue('string error')

    render(<ReviewLessonsPage />)

    expect(await screen.findByText('Q1')).toBeInTheDocument()
    fireEvent.click(screen.getByRole('button', { name: 'SubmitMock' }))

    expect(await screen.findByText(/failed to check answer/i)).toBeInTheDocument()
  })
})
