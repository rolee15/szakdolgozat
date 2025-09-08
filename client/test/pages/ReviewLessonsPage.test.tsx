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

    // click mocked submit button -> triggers onSubmit('ans')
    fireEvent.click(screen.getByRole('button', { name: 'SubmitMock' }))

    // Feedback renders for correct answer
    expect(await screen.findByText(/correct!/i)).toBeInTheDocument()

    // Click container to advance
    fireEvent.click(screen.getByText('Q1').parentElement as Element)

    // Next question should appear
    expect(await screen.findByText('Q2')).toBeInTheDocument()
  })
})
