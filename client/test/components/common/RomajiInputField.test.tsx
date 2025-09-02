import { describe, it, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import RomajiInputField from '@/components/common/RomajiInputField'

describe('RomajiInputField', () => {
  it('renders input and clicking submit calls onChange with empty string', async () => {
    const onChange = vi.fn()
    render(<RomajiInputField value="" onChange={onChange} />)

    const input = screen.getByRole('textbox')
    expect(input).toBeInTheDocument()

    const button = screen.getByRole('button', { name: /submit answer/i })
    await userEvent.click(button)

    expect(onChange).toHaveBeenCalledWith('')
  })
})
