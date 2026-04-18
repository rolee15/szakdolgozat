import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import AdminUsersPage from '@/pages/admin/AdminUsersPage'

vi.mock('@/services/adminService', () => ({
  default: {
    getUsers: vi.fn(),
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

function renderPage() {
  return render(
    <QueryClientProvider client={makeQueryClient()}>
      <MemoryRouter>
        <AdminUsersPage />
      </MemoryRouter>
    </QueryClientProvider>
  )
}

const samplePagedUsers: PagedResult<AdminUser> = {
  items: [
    {
      id: 1,
      username: 'alice',
      role: 'User',
      mustChangePassword: false,
      proficiencyCount: 10,
      lessonCompletionCount: 5,
    },
    {
      id: 2,
      username: 'adminuser',
      role: 'Admin',
      mustChangePassword: false,
      proficiencyCount: 0,
      lessonCompletionCount: 0,
    },
  ],
  totalCount: 2,
  page: 1,
  pageSize: 20,
  hasNextPage: false,
}

describe('AdminUsersPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })

  afterEach(() => {
    vi.clearAllMocks()
  })

  it('renders loading state while data is being fetched', () => {
    const svc = adminService as unknown as { getUsers: ReturnType<typeof vi.fn> }
    svc.getUsers.mockReturnValue(new Promise(() => {}))

    renderPage()

    expect(screen.getByText(/loading\.\.\./i)).toBeInTheDocument()
  })

  it('renders user table with data when query resolves', async () => {
    const svc = adminService as unknown as { getUsers: ReturnType<typeof vi.fn> }
    svc.getUsers.mockResolvedValue(samplePagedUsers)

    renderPage()

    expect(await screen.findByText('alice')).toBeInTheDocument()
    expect(await screen.findByText('adminuser')).toBeInTheDocument()
    expect(screen.getByText('User')).toBeInTheDocument()
    expect(screen.getByText('Admin')).toBeInTheDocument()
  })

  it('renders search input', () => {
    const svc = adminService as unknown as { getUsers: ReturnType<typeof vi.fn> }
    svc.getUsers.mockReturnValue(new Promise(() => {}))

    renderPage()

    expect(screen.getByPlaceholderText(/search by username/i)).toBeInTheDocument()
    expect(screen.getByRole('button', { name: /search/i })).toBeInTheDocument()
  })

  it('renders user count summary when data loads', async () => {
    const svc = adminService as unknown as { getUsers: ReturnType<typeof vi.fn> }
    svc.getUsers.mockResolvedValue(samplePagedUsers)

    renderPage()

    expect(await screen.findByText(/showing 2 of 2 users/i)).toBeInTheDocument()
  })

  it('renders error message when query fails', async () => {
    const svc = adminService as unknown as { getUsers: ReturnType<typeof vi.fn> }
    svc.getUsers.mockRejectedValue(new Error('Network error'))

    renderPage()

    expect(await screen.findByText(/failed to load users/i)).toBeInTheDocument()
  })

  it('does not show delete button for admin users', async () => {
    const svc = adminService as unknown as { getUsers: ReturnType<typeof vi.fn> }
    svc.getUsers.mockResolvedValue(samplePagedUsers)

    renderPage()

    await screen.findByText('alice')

    // There should be only one delete button (for alice, not for adminuser)
    const deleteButtons = screen.getAllByRole('button', { name: /delete/i })
    // One for alice (non-admin), none for adminuser (Admin role)
    expect(deleteButtons).toHaveLength(1)
  })

  it('shows delete button only for non-admin users', async () => {
    const svc = adminService as unknown as { getUsers: ReturnType<typeof vi.fn> }
    svc.getUsers.mockResolvedValue({
      ...samplePagedUsers,
      items: [{ id: 1, username: 'alice', role: 'User', mustChangePassword: false, proficiencyCount: 0, lessonCompletionCount: 0 }],
      totalCount: 1,
    })

    renderPage()

    await screen.findByText('alice')

    expect(screen.getByRole('button', { name: /delete/i })).toBeInTheDocument()
  })

  it('calls getUsers with updated search term on form submit', async () => {
    const svc = adminService as unknown as { getUsers: ReturnType<typeof vi.fn> }
    svc.getUsers.mockResolvedValue({ ...samplePagedUsers, items: [], totalCount: 0 })

    renderPage()

    fireEvent.change(screen.getByPlaceholderText(/search by username/i), { target: { value: 'alice' } })
    fireEvent.submit(screen.getByRole('button', { name: /search/i }).closest('form') as HTMLFormElement)

    await vi.waitFor(() => {
      expect(svc.getUsers).toHaveBeenCalledWith(1, 20, 'alice')
    })
  })
})
