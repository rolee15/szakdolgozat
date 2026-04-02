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
})
