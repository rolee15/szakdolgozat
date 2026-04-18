import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import userEvent from '@testing-library/user-event'

vi.mock('wanakana', () => ({
  bind: vi.fn(),
  unbind: vi.fn(),
}))

import * as wanakana from 'wanakana'
import WritingInput from '@components/lessons/WritingInput'

describe('WritingInput', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('renders an input element and submit button', () => {
    render(<WritingInput characterType="hiragana" onSubmit={vi.fn()} />)

    expect(screen.getByLabelText('Writing answer')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: /submit answer/i })).toBeInTheDocument()
  })

  it('calls onSubmit with the input value when the form is submitted', async () => {
    const onSubmit = vi.fn()
    render(<WritingInput characterType="hiragana" onSubmit={onSubmit} />)

    const input = screen.getByLabelText('Writing answer') as HTMLInputElement
    const user = userEvent.setup()
    await user.type(input, 'あ')

    fireEvent.submit(screen.getByRole('button', { name: /submit answer/i }).closest('form')!)

    expect(onSubmit).toHaveBeenCalledWith('あ')
  })

  it('clears the input after submit', async () => {
    const onSubmit = vi.fn()
    render(<WritingInput characterType="hiragana" onSubmit={onSubmit} />)

    const input = screen.getByLabelText('Writing answer') as HTMLInputElement
    const user = userEvent.setup()
    await user.type(input, 'あ')

    fireEvent.submit(input.closest('form')!)

    expect(input.value).toBe('')
  })

  it('calls wanakana.bind on mount and wanakana.unbind on unmount', () => {
    const { unmount } = render(<WritingInput characterType="hiragana" onSubmit={vi.fn()} />)

    expect(wanakana.bind).toHaveBeenCalledTimes(1)
    expect(wanakana.unbind).not.toHaveBeenCalled()

    unmount()

    expect(wanakana.unbind).toHaveBeenCalledTimes(1)
  })
})
