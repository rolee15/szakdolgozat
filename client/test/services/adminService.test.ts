import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';

vi.mock('@/services/routes', () => ({
  API_ADMIN_URL: 'http://api.test/admin',
}));

vi.mock('@/services/apiClient', () => ({
  apiFetch: vi.fn(),
}));

import adminService from '@/services/adminService';
import { apiFetch } from '@/services/apiClient';

const mockApiFetch = apiFetch as ReturnType<typeof vi.fn>;

function mockOk(data: unknown) {
  mockApiFetch.mockResolvedValue({ ok: true, json: vi.fn().mockResolvedValue(data) });
}

function mockNotOk() {
  mockApiFetch.mockResolvedValue({ ok: false, json: vi.fn() });
}

describe('adminService', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  describe('getUsers', () => {
    it('fetches users with pagination params and returns paged result', async () => {
      const payload: PagedResult<AdminUser> = {
        items: [{ id: 1, username: 'alice', role: 'User', mustChangePassword: false, proficiencyCount: 0, lessonCompletionCount: 0 }],
        totalCount: 1,
        page: 1,
        pageSize: 20,
        hasNextPage: false,
      };
      mockOk(payload);

      const result = await adminService.getUsers(1, 20);

      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith('http://api.test/admin/users?page=1&pageSize=20');
    });

    it('includes search param when provided', async () => {
      mockOk({ items: [], totalCount: 0, page: 1, pageSize: 20, hasNextPage: false });

      await adminService.getUsers(1, 20, 'alice');

      expect(mockApiFetch).toHaveBeenCalledWith('http://api.test/admin/users?page=1&pageSize=20&search=alice');
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(adminService.getUsers(1, 20)).rejects.toThrow('Failed to fetch users');
    });
  });

  describe('getUserById', () => {
    it('fetches a user by id and returns detail', async () => {
      const payload: AdminUserDetail = {
        id: 42,
        username: 'testuser',
        role: 'User',
        mustChangePassword: false,
        proficiencyCount: 1,
        lessonCompletionCount: 1,
        proficiencies: [],
        lessonCompletions: [],
      };
      mockOk(payload);

      const result = await adminService.getUserById(42);

      expect(result).toEqual(payload);
      expect(mockApiFetch).toHaveBeenCalledWith('http://api.test/admin/users/42');
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(adminService.getUserById(99)).rejects.toThrow('Failed to fetch user');
    });
  });

  describe('deleteUser', () => {
    it('sends DELETE request for the given user id', async () => {
      mockApiFetch.mockResolvedValue({ ok: true });

      await adminService.deleteUser(7);

      expect(mockApiFetch).toHaveBeenCalledWith('http://api.test/admin/users/7', { method: 'DELETE' });
    });

    it('throws on non-ok response', async () => {
      mockNotOk();
      await expect(adminService.deleteUser(7)).rejects.toThrow('Failed to delete user');
    });
  });
});
