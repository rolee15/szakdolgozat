import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import WritingPracticePage from '@/pages/WritingPracticePage'

// Mock WritingInput to simplify submission flow
vi.mock('@/components/lessons/WritingInput', () => ({
  default: ({ onSubmit }: { onSubmit: (a: string) => void }) => (
    <button onClick={() => onSubmit('ans')}>SubmitMock</button>
  ),
}))

vi.mock('@/services/lessonService', () => ({
  default: {
    getWritingReviews: vi.fn(),
    postWritingReviewCheck: vi.fn(),
  }
}))

import lessonService from '@/services/lessonService'

describe('WritingPracticePage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })
  afterEach(() => {
    vi.clearAllMocks()
  })

  it('shows loading state initially', () => {
    const svc = lessonService as unknown as { getWritingReviews: ReturnType<typeof vi.fn> }
    svc.getWritingReviews.mockReturnValue(new Promise(() => {}))

    render(<WritingPracticePage />)

    expect(screen.getByText(/loading/i)).toBeInTheDocument()
  })

  it('shows "No items to review" when the API returns empty', async () => {
    const svc = lessonService as unknown as { getWritingReviews: ReturnType<typeof vi.fn> }
    svc.getWritingReviews.mockResolvedValue([])

    render(<WritingPracticePage />)

    expect(await screen.findByText(/no items to review/i)).toBeInTheDocument()
  })

  it('shows the romanization prompt from the first review item', async () => {
    const svc = lessonService as unknown as { getWritingReviews: ReturnType<typeof vi.fn> }
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 1, romanization: 'a', characterType: 'hiragana' },
      { characterId: 2, romanization: 'i', characterType: 'hiragana' },
    ])

    render(<WritingPracticePage />)

    expect(await screen.findByText('a')).toBeInTheDocument()
  })

  it('shows the character type label', async () => {
    const svc = lessonService as unknown as { getWritingReviews: ReturnType<typeof vi.fn> }
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 1, romanization: 'a', characterType: 'hiragana' },
    ])

    render(<WritingPracticePage />)

    expect(await screen.findByText('Hiragana')).toBeInTheDocument()
  })

  it('shows the Katakana label for katakana items', async () => {
    const svc = lessonService as unknown as { getWritingReviews: ReturnType<typeof vi.fn> }
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 3, romanization: 'a', characterType: 'katakana' },
    ])

    render(<WritingPracticePage />)

    expect(await screen.findByText('Katakana')).toBeInTheDocument()
  })

  it('calls postWritingReviewCheck on submit', async () => {
    const svc = lessonService as unknown as {
      getWritingReviews: ReturnType<typeof vi.fn>
      postWritingReviewCheck: ReturnType<typeof vi.fn>
    }
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 1, romanization: 'a', characterType: 'hiragana' },
    ])
    svc.postWritingReviewCheck.mockResolvedValue({ isCorrect: true, correctAnswer: 'あ' })

    render(<WritingPracticePage />)

    fireEvent.click(await screen.findByRole('button', { name: 'SubmitMock' }))

    await screen.findByText(/correct/i)
    expect(svc.postWritingReviewCheck).toHaveBeenCalledWith(1, 'ans')
  })

  it('shows correct feedback when answer is correct', async () => {
    const svc = lessonService as unknown as {
      getWritingReviews: ReturnType<typeof vi.fn>
      postWritingReviewCheck: ReturnType<typeof vi.fn>
    }
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 1, romanization: 'a', characterType: 'hiragana' },
      { characterId: 2, romanization: 'i', characterType: 'hiragana' },
    ])
    svc.postWritingReviewCheck.mockResolvedValue({ isCorrect: true, correctAnswer: 'あ' })

    render(<WritingPracticePage />)

    fireEvent.click(await screen.findByRole('button', { name: 'SubmitMock' }))

    expect(await screen.findByText(/correct/i)).toBeInTheDocument()
  })

  it('shows incorrect feedback with correct answer when answer is wrong', async () => {
    const svc = lessonService as unknown as {
      getWritingReviews: ReturnType<typeof vi.fn>
      postWritingReviewCheck: ReturnType<typeof vi.fn>
    }
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 1, romanization: 'a', characterType: 'hiragana' },
    ])
    svc.postWritingReviewCheck.mockResolvedValue({ isCorrect: false, correctAnswer: 'あ' })

    render(<WritingPracticePage />)

    fireEvent.click(await screen.findByRole('button', { name: 'SubmitMock' }))

    expect(await screen.findByText(/incorrect/i)).toBeInTheDocument()
    expect(await screen.findByText('あ')).toBeInTheDocument()
  })

  it('advances to the next item after submitting', async () => {
    const svc = lessonService as unknown as {
      getWritingReviews: ReturnType<typeof vi.fn>
      postWritingReviewCheck: ReturnType<typeof vi.fn>
    }
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 1, romanization: 'a', characterType: 'hiragana' },
      { characterId: 2, romanization: 'i', characterType: 'hiragana' },
    ])
    svc.postWritingReviewCheck.mockResolvedValue({ isCorrect: true, correctAnswer: 'あ' })

    render(<WritingPracticePage />)

    expect(await screen.findByText('a')).toBeInTheDocument()

    fireEvent.click(screen.getByRole('button', { name: 'SubmitMock' }))

    // After correct answer the continue button appears — click it to advance
    fireEvent.click(await screen.findByRole('button', { name: /continue/i }))

    expect(await screen.findByText('i')).toBeInTheDocument()
  })

  it('cycles the last item to end of queue when answered incorrectly — does not show completion screen', async () => {
    const svc = lessonService as unknown as {
      getWritingReviews: ReturnType<typeof vi.fn>
      postWritingReviewCheck: ReturnType<typeof vi.fn>
    }
    // Single item: if incorrect incremented index would equal queue length and wrongly show "complete"
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 1, romanization: 'a', characterType: 'hiragana' },
    ])
    svc.postWritingReviewCheck.mockResolvedValue({ isCorrect: false, correctAnswer: 'あ' })

    render(<WritingPracticePage />)

    // Submit an incorrect answer
    fireEvent.click(await screen.findByRole('button', { name: 'SubmitMock' }))

    // Dismiss the feedback to trigger advanceToNext
    fireEvent.click(await screen.findByRole('button', { name: /continue/i }))

    // The item should have cycled back — romanization prompt still visible, NOT the completion message
    expect(await screen.findByText('a')).toBeInTheDocument()
    expect(screen.queryByText(/writing practice complete/i)).not.toBeInTheDocument()
  })

  it('keeps index within bounds after removing non-last correct item', async () => {
    const svc = lessonService as unknown as {
      getWritingReviews: ReturnType<typeof vi.fn>
      postWritingReviewCheck: ReturnType<typeof vi.fn>
    }
    // 3 items; answer item 0 correctly; index 0 < updated.length(2) — no adjustment branch
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 1, romanization: 'a', characterType: 'hiragana' },
      { characterId: 2, romanization: 'i', characterType: 'hiragana' },
      { characterId: 3, romanization: 'u', characterType: 'hiragana' },
    ])
    svc.postWritingReviewCheck.mockResolvedValue({ isCorrect: true, correctAnswer: 'あ' })

    render(<WritingPracticePage />)

    expect(await screen.findByText('a')).toBeInTheDocument()
    fireEvent.click(screen.getByRole('button', { name: 'SubmitMock' }))
    fireEvent.click(await screen.findByRole('button', { name: /continue/i }))

    // After removing item 0, item 1 ('i') is now at index 0
    expect(await screen.findByText('i')).toBeInTheDocument()
  })

  it('shows error when postWritingReviewCheck fails', async () => {
    const svc = lessonService as unknown as {
      getWritingReviews: ReturnType<typeof vi.fn>
      postWritingReviewCheck: ReturnType<typeof vi.fn>
    }
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 1, romanization: 'a', characterType: 'hiragana' },
    ])
    svc.postWritingReviewCheck.mockRejectedValue(new Error('Check error'))

    render(<WritingPracticePage />)

    fireEvent.click(await screen.findByRole('button', { name: 'SubmitMock' }))

    expect(await screen.findByText(/error: check error/i)).toBeInTheDocument()
  })

  it('uses fallback message when non-Error thrown from getWritingReviews', async () => {
    const svc = lessonService as unknown as { getWritingReviews: ReturnType<typeof vi.fn> }
    svc.getWritingReviews.mockRejectedValue('string error')

    render(<WritingPracticePage />)

    expect(await screen.findByText(/failed to load writing reviews/i)).toBeInTheDocument()
  })

  it('uses fallback message when non-Error thrown from postWritingReviewCheck', async () => {
    const svc = lessonService as unknown as {
      getWritingReviews: ReturnType<typeof vi.fn>
      postWritingReviewCheck: ReturnType<typeof vi.fn>
    }
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 1, romanization: 'a', characterType: 'hiragana' },
    ])
    svc.postWritingReviewCheck.mockRejectedValue('string error')

    render(<WritingPracticePage />)

    fireEvent.click(await screen.findByRole('button', { name: 'SubmitMock' }))

    expect(await screen.findByText(/failed to check answer/i)).toBeInTheDocument()
  })
})
