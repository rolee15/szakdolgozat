import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter, Route, Routes } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import AdminUserDetailPage from '@/pages/admin/AdminUserDetailPage'

vi.mock('@/services/adminService', () => ({
  default: {
    getUserById: vi.fn(),
    deleteUser: vi.fn(),
  },
}))

import adminService from '@/services/adminService'

function makeQueryClient() {
  return new QueryClient({
    defaultOptions: {
      queries: { retry: false },
    },
  })
}

function renderPage(userId: string = '42') {
  return render(
    <QueryClientProvider client={makeQueryClient()}>
      <MemoryRouter initialEntries={[`/admin/users/${userId}`]}>
        <Routes>
          <Route path="/admin/users/:id" element={<AdminUserDetailPage />} />
        </Routes>
      </MemoryRouter>
    </QueryClientProvider>
  )
}

const sampleUser: AdminUserDetail = {
  id: 42,
  username: 'testuser',
  role: 'User',
  mustChangePassword: false,
  proficiencyCount: 2,
  lessonCompletionCount: 1,
  proficiencies: [
    {
      characterId: 10,
      characterSymbol: 'あ',
      learnedAt: '2024-01-15T00:00:00Z',
      lastPracticed: '2024-02-01T00:00:00Z',
    },
    {
      characterId: 11,
      characterSymbol: 'い',
      learnedAt: '2024-01-16T00:00:00Z',
      lastPracticed: '2024-02-02T00:00:00Z',
    },
  ],
  lessonCompletions: [
    {
      characterId: 10,
      characterSymbol: 'あ',
      completionDate: '2024-01-15T00:00:00Z',
    },
  ],
}

describe('AdminUserDetailPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })

  afterEach(() => {
    vi.clearAllMocks()
  })

  it('renders loading state while data is being fetched', () => {
    const svc = adminService as unknown as { getUserById: ReturnType<typeof vi.fn> }
    svc.getUserById.mockReturnValue(new Promise(() => {}))

    renderPage()

    expect(screen.getByText(/loading\.\.\./i)).toBeInTheDocument()
  })

  it('renders user details when data is loaded', async () => {
    const svc = adminService as unknown as { getUserById: ReturnType<typeof vi.fn> }
    svc.getUserById.mockResolvedValue(sampleUser)

    renderPage()

    expect(await screen.findByRole('heading', { name: /user details/i })).toBeInTheDocument()
    expect(await screen.findByText('testuser')).toBeInTheDocument()
    expect(screen.getByText('User')).toBeInTheDocument()
    expect(screen.getByText('42')).toBeInTheDocument()
    expect(screen.getByText('No')).toBeInTheDocument()
  })

  it('renders proficiency table when user has proficiencies', async () => {
    const svc = adminService as unknown as { getUserById: ReturnType<typeof vi.fn> }
    svc.getUserById.mockResolvedValue(sampleUser)

    renderPage()

    expect(await screen.findByRole('heading', { name: /proficiencies/i })).toBeInTheDocument()
    // Both characters appear in the proficiencies section (い only in proficiencies, not in lesson completions)
    expect(await screen.findByText('い')).toBeInTheDocument()
  })

  it('does not render proficiency table when user has no proficiencies', async () => {
    const svc = adminService as unknown as { getUserById: ReturnType<typeof vi.fn> }
    svc.getUserById.mockResolvedValue({ ...sampleUser, proficiencies: [], lessonCompletions: [] })

    renderPage()

    await screen.findByText('testuser')

    expect(screen.queryByRole('heading', { name: /proficiencies/i })).not.toBeInTheDocument()
  })

  it('renders error message when query fails', async () => {
    const svc = adminService as unknown as { getUserById: ReturnType<typeof vi.fn> }
    svc.getUserById.mockRejectedValue(new Error('Not found'))

    renderPage()

    expect(await screen.findByText(/user not found/i)).toBeInTheDocument()
  })

  it('renders delete button for non-admin users', async () => {
    const svc = adminService as unknown as { getUserById: ReturnType<typeof vi.fn> }
    svc.getUserById.mockResolvedValue(sampleUser)

    renderPage()

    await screen.findByText('testuser')

    expect(screen.getByRole('button', { name: /delete user/i })).toBeInTheDocument()
  })

  it('does not render delete button for admin users', async () => {
    const svc = adminService as unknown as { getUserById: ReturnType<typeof vi.fn> }
    svc.getUserById.mockResolvedValue({ ...sampleUser, role: 'Admin' })

    renderPage()

    await screen.findByText('testuser')

    expect(screen.queryByRole('button', { name: /delete user/i })).not.toBeInTheDocument()
  })

  it('shows mustChangePassword as Yes when true', async () => {
    const svc = adminService as unknown as { getUserById: ReturnType<typeof vi.fn> }
    svc.getUserById.mockResolvedValue({ ...sampleUser, mustChangePassword: true })

    renderPage()

    expect(await screen.findByText('Yes')).toBeInTheDocument()
  })

  it('renders lesson completions table when user has completions', async () => {
    const svc = adminService as unknown as { getUserById: ReturnType<typeof vi.fn> }
    svc.getUserById.mockResolvedValue(sampleUser)

    renderPage()

    expect(await screen.findByRole('heading', { name: /lesson completions/i })).toBeInTheDocument()
  })
})
