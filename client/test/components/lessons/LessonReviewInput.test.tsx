import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, fireEvent, act } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import LessonReviewInput from '@/components/lessons/LessonReviewInput'

describe('LessonReviewInput', () => {
  beforeEach(() => {
    vi.useFakeTimers({ shouldAdvanceTime: true })
  })

  it('submits the trimmed answer on Enter key', async () => {
    const onSubmit = vi.fn()
    render(<LessonReviewInput onSubmit={onSubmit} />)

    const input = screen.getByLabelText('Review answer') as HTMLInputElement

    const user = userEvent.setup({ advanceTimers: vi.advanceTimersByTime })
    await user.type(input, '  neko  ')
    fireEvent.keyDown(input, { key: 'Enter', code: 'Enter', charCode: 13 })

    expect(onSubmit).toHaveBeenCalledWith('neko')
  })

  it('shows a warning when submitting empty value and clears after 3 seconds', async () => {
    const onSubmit = vi.fn()
    render(<LessonReviewInput onSubmit={onSubmit} />)

    const submitButton = screen.getByRole('button', { name: /submit answer/i })
    const user = userEvent.setup({ advanceTimers: vi.advanceTimersByTime })
    await user.click(submitButton)

    expect(onSubmit).not.toHaveBeenCalled()
    const warning = screen.getByText(
      'Type your answer first. Or hit Enter to mark it as wrong.'
    )
    expect(warning).toBeInTheDocument()

    // advance fake timers to clear the warning
    act(() => {
      vi.advanceTimersByTime(3000)
    })

    expect(
      screen.queryByText(
        'Type your answer first. Or hit Enter to mark it as wrong.'
      )
    ).not.toBeInTheDocument()
  })
})
