import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'

import LoginPage from '@/pages/LoginPage'
import RegisterPage from '@/pages/RegisterPage'
import ForgotPasswordPage from '@/pages/ForgotPasswordPage'

vi.mock('@/services/userService', () => ({
  default: {
    login: vi.fn(),
    register: vi.fn(),
    resetPassword: vi.fn(),
  }
}))

import userService from '@/services/userService'

describe('Auth pages', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })
  afterEach(() => {
    vi.clearAllMocks()
  })

  describe('LoginPage', () => {
    it('submits email and password and calls setToken', async () => {
      const svc = userService as unknown as { login: ReturnType<typeof vi.fn> }
      svc.login.mockResolvedValue({ token: 'jwt' })
      const setToken = vi.fn()

      render(
        <MemoryRouter>
          <LoginPage setToken={setToken} />
        </MemoryRouter>
      )

      fireEvent.change(screen.getByLabelText(/email/i), { target: { value: 'a@b.c' } })
      fireEvent.change(screen.getByLabelText(/password/i), { target: { value: 'secret' } })
      fireEvent.submit(screen.getByRole('button', { name: /login/i }).closest('form') as HTMLFormElement)

      expect(await screen.findByRole('button', { name: /login/i })).toBeInTheDocument()
      await vi.waitFor(() => {
        expect(svc.login).toHaveBeenCalledWith('a@b.c', 'secret')
        expect(setToken).toHaveBeenCalledWith('jwt')
      })
    })
  })

  describe('RegisterPage', () => {
    it('renders a heading', () => {
      render(
        <MemoryRouter>
          <RegisterPage />
        </MemoryRouter>
      )
      expect(screen.getByRole('heading', { name: /register/i })).toBeInTheDocument()
    })
  })

  describe('ForgotPasswordPage', () => {
    it('renders form and allows submitting without crashing', async () => {
      render(
        <MemoryRouter>
          <ForgotPasswordPage />
        </MemoryRouter>
      )

      const emailInput = screen.getByRole('textbox') as HTMLInputElement
      fireEvent.change(emailInput, { target: { value: 'x@y.z' } })

      const button = screen.getByRole('button', { name: /reset password/i })
      fireEvent.click(button)

      // After submit, component sets loading then unsets; final state shows the same button text
      expect(await screen.findByRole('button', { name: /reset password/i })).toBeInTheDocument()
      // No error is displayed by default
      expect(screen.queryByText(/an error occurred/i)).not.toBeInTheDocument()
    })
  })
})
