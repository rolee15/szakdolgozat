import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import LessonsPage from '@/pages/LessonsPage'

vi.mock('@/services/lessonService', () => ({
  default: {
    getLessonsCount: vi.fn(),
    getLessonReviewsCount: vi.fn(),
  }
}))

import lessonService from '@/services/lessonService'

describe('LessonsPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })
  afterEach(() => {
    vi.clearAllMocks()
  })

  it('renders counts and links to learn and review', async () => {
    const svc = lessonService as unknown as { getLessonsCount: ReturnType<typeof vi.fn>, getLessonReviewsCount: ReturnType<typeof vi.fn> }
    svc.getLessonsCount.mockResolvedValue({ count: 5 })
    svc.getLessonReviewsCount.mockResolvedValue({ count: 3 })

    render(
      <MemoryRouter>
        <LessonsPage />
      </MemoryRouter>
    )

    // await counts to appear
    expect(await screen.findByText('5')).toBeInTheDocument()
    expect(await screen.findByText('3')).toBeInTheDocument()

    const learnLink = screen.getByRole('link', { name: /learn/i })
    expect(learnLink).toHaveAttribute('href', '/lessons/new')

    const reviewLink = screen.getByRole('link', { name: /review/i })
    expect(reviewLink).toHaveAttribute('href', '/lessons/review')
  })

  it('renders error when fetching fails', async () => {
    const svc = lessonService as unknown as { getLessonsCount: ReturnType<typeof vi.fn>, getLessonReviewsCount: ReturnType<typeof vi.fn> }
    svc.getLessonsCount.mockRejectedValue(new Error('boom'))
    svc.getLessonReviewsCount.mockResolvedValue({ count: 0 })

    render(
      <MemoryRouter>
        <LessonsPage />
      </MemoryRouter>
    )

    expect(await screen.findByText(/error: boom/i)).toBeInTheDocument()
  })
})
