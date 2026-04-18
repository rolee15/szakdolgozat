import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import ForgotPasswordPage from '@/pages/ForgotPasswordPage'

vi.mock('@/services/userService', () => ({
  default: {
    forgotPassword: vi.fn(),
    confirmResetPassword: vi.fn(),
  },
}))

import userService from '@/services/userService'

function renderPage() {
  return render(
    <MemoryRouter>
      <ForgotPasswordPage />
    </MemoryRouter>
  )
}

describe('ForgotPasswordPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })

  afterEach(() => {
    vi.clearAllMocks()
  })

  it('renders email input in step 1', () => {
    renderPage()

    expect(screen.getByLabelText(/email/i)).toBeInTheDocument()
    expect(screen.getByRole('button', { name: /send reset code/i })).toBeInTheDocument()
  })

  it('calls forgotPassword on step 1 submit', async () => {
    const svc = userService as unknown as {
      forgotPassword: ReturnType<typeof vi.fn>
      confirmResetPassword: ReturnType<typeof vi.fn>
    }
    svc.forgotPassword.mockResolvedValue(undefined)

    renderPage()

    fireEvent.change(screen.getByLabelText(/email/i), { target: { value: 'test@example.com' } })
    fireEvent.submit(
      screen.getByRole('button', { name: /send reset code/i }).closest('form') as HTMLFormElement
    )

    await vi.waitFor(() => {
      expect(svc.forgotPassword).toHaveBeenCalledWith('test@example.com')
    })
  })

  it('shows step 2 after successful code send', async () => {
    const svc = userService as unknown as {
      forgotPassword: ReturnType<typeof vi.fn>
      confirmResetPassword: ReturnType<typeof vi.fn>
    }
    svc.forgotPassword.mockResolvedValue(undefined)

    renderPage()

    fireEvent.change(screen.getByLabelText(/email/i), { target: { value: 'test@example.com' } })
    fireEvent.submit(
      screen.getByRole('button', { name: /send reset code/i }).closest('form') as HTMLFormElement
    )

    expect(await screen.findByText(/コードと新しいパスワードを入力してください/)).toBeInTheDocument()
    expect(screen.getByLabelText(/reset code/i)).toBeInTheDocument()
  })

  it('shows error if forgotPassword fails', async () => {
    const svc = userService as unknown as {
      forgotPassword: ReturnType<typeof vi.fn>
      confirmResetPassword: ReturnType<typeof vi.fn>
    }
    svc.forgotPassword.mockRejectedValue(new Error('Network error'))

    renderPage()

    fireEvent.change(screen.getByLabelText(/email/i), { target: { value: 'test@example.com' } })
    fireEvent.submit(
      screen.getByRole('button', { name: /send reset code/i }).closest('form') as HTMLFormElement
    )

    expect(await screen.findByRole('alert')).toHaveTextContent('Network error')
  })

  it('shows generic error if forgotPassword fails with non-Error', async () => {
    const svc = userService as unknown as {
      forgotPassword: ReturnType<typeof vi.fn>
      confirmResetPassword: ReturnType<typeof vi.fn>
    }
    svc.forgotPassword.mockRejectedValue('unexpected')

    renderPage()

    fireEvent.change(screen.getByLabelText(/email/i), { target: { value: 'test@example.com' } })
    fireEvent.submit(
      screen.getByRole('button', { name: /send reset code/i }).closest('form') as HTMLFormElement
    )

    expect(await screen.findByRole('alert')).toHaveTextContent(/failed to send reset code/i)
  })

  describe('step 2 — code and password entry', () => {
    async function goToStep2() {
      const svc = userService as unknown as {
        forgotPassword: ReturnType<typeof vi.fn>
        confirmResetPassword: ReturnType<typeof vi.fn>
      }
      svc.forgotPassword.mockResolvedValue(undefined)

      renderPage()

      fireEvent.change(screen.getByLabelText(/email/i), { target: { value: 'test@example.com' } })
      fireEvent.submit(
        screen.getByRole('button', { name: /send reset code/i }).closest('form') as HTMLFormElement
      )

      await screen.findByText(/コードと新しいパスワードを入力してください/)
    }

    it('renders code and password fields in step 2', async () => {
      await goToStep2()

      expect(screen.getByLabelText(/reset code/i)).toBeInTheDocument()
      expect(screen.getByLabelText(/^new password$/i)).toBeInTheDocument()
      expect(screen.getByLabelText(/confirm password/i)).toBeInTheDocument()
    })

    it('shows validation error when passwords do not match', async () => {
      await goToStep2()

      fireEvent.change(screen.getByLabelText(/reset code/i), { target: { value: '123456' } })
      fireEvent.change(screen.getByLabelText(/^new password$/i), { target: { value: 'password123' } })
      fireEvent.change(screen.getByLabelText(/confirm password/i), { target: { value: 'different' } })
      fireEvent.submit(
        screen.getByRole('button', { name: /reset password/i }).closest('form') as HTMLFormElement
      )

      expect(await screen.findByRole('alert')).toHaveTextContent(/passwords do not match/i)
    })

    it('calls confirmResetPassword on step 2 submit when passwords match', async () => {
      const svc = userService as unknown as {
        forgotPassword: ReturnType<typeof vi.fn>
        confirmResetPassword: ReturnType<typeof vi.fn>
      }
      svc.confirmResetPassword.mockResolvedValue(undefined)

      await goToStep2()

      fireEvent.change(screen.getByLabelText(/reset code/i), { target: { value: '123456' } })
      fireEvent.change(screen.getByLabelText(/^new password$/i), { target: { value: 'newpass123' } })
      fireEvent.change(screen.getByLabelText(/confirm password/i), { target: { value: 'newpass123' } })
      fireEvent.submit(
        screen.getByRole('button', { name: /reset password/i }).closest('form') as HTMLFormElement
      )

      await vi.waitFor(() => {
        expect(svc.confirmResetPassword).toHaveBeenCalledWith('test@example.com', '123456', 'newpass123')
      })
    })

    it('shows success message after resetPassword completes', async () => {
      const svc = userService as unknown as {
        forgotPassword: ReturnType<typeof vi.fn>
        confirmResetPassword: ReturnType<typeof vi.fn>
      }
      svc.confirmResetPassword.mockResolvedValue(undefined)

      await goToStep2()

      fireEvent.change(screen.getByLabelText(/reset code/i), { target: { value: '123456' } })
      fireEvent.change(screen.getByLabelText(/^new password$/i), { target: { value: 'newpass123' } })
      fireEvent.change(screen.getByLabelText(/confirm password/i), { target: { value: 'newpass123' } })
      fireEvent.submit(
        screen.getByRole('button', { name: /reset password/i }).closest('form') as HTMLFormElement
      )

      expect(await screen.findByText(/パスワードがリセットされました/)).toBeInTheDocument()
      expect(screen.getByRole('link', { name: /ログインに戻る/ })).toBeInTheDocument()
    })

    it('shows error if confirmResetPassword fails', async () => {
      const svc = userService as unknown as {
        forgotPassword: ReturnType<typeof vi.fn>
        confirmResetPassword: ReturnType<typeof vi.fn>
      }
      svc.confirmResetPassword.mockRejectedValue(new Error('Invalid or expired reset code'))

      await goToStep2()

      fireEvent.change(screen.getByLabelText(/reset code/i), { target: { value: 'badcode' } })
      fireEvent.change(screen.getByLabelText(/^new password$/i), { target: { value: 'newpass123' } })
      fireEvent.change(screen.getByLabelText(/confirm password/i), { target: { value: 'newpass123' } })
      fireEvent.submit(
        screen.getByRole('button', { name: /reset password/i }).closest('form') as HTMLFormElement
      )

      expect(await screen.findByRole('alert')).toHaveTextContent(/invalid or expired reset code/i)
    })

    it('shows generic error if confirmResetPassword fails with non-Error', async () => {
      const svc = userService as unknown as {
        forgotPassword: ReturnType<typeof vi.fn>
        confirmResetPassword: ReturnType<typeof vi.fn>
      }
      svc.confirmResetPassword.mockRejectedValue('unexpected')

      await goToStep2()

      fireEvent.change(screen.getByLabelText(/reset code/i), { target: { value: 'badcode' } })
      fireEvent.change(screen.getByLabelText(/^new password$/i), { target: { value: 'newpass123' } })
      fireEvent.change(screen.getByLabelText(/confirm password/i), { target: { value: 'newpass123' } })
      fireEvent.submit(
        screen.getByRole('button', { name: /reset password/i }).closest('form') as HTMLFormElement
      )

      expect(await screen.findByRole('alert')).toHaveTextContent(/failed to reset password/i)
    })
  })
})
