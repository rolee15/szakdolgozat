import { apiFetch } from "@/services/apiClient";
import { API_ADMIN_URL } from "@/services/routes";

const adminService = {
    async getUsers(page: number, pageSize: number, search?: string): Promise<PagedResult<AdminUser>> {
        const params = new URLSearchParams({ page: String(page), pageSize: String(pageSize) });
        if (search) params.set('search', search);
        const response = await apiFetch(`${API_ADMIN_URL}/users?${params}`);
        if (!response.ok) throw new Error('Failed to fetch users');
        return response.json();
    },

    async getUserById(id: number): Promise<AdminUserDetail> {
        const response = await apiFetch(`${API_ADMIN_URL}/users/${id}`);
        if (!response.ok) throw new Error('Failed to fetch user');
        return response.json();
    },

    async deleteUser(id: number): Promise<void> {
        const response = await apiFetch(`${API_ADMIN_URL}/users/${id}`, { method: 'DELETE' });
        if (!response.ok) throw new Error('Failed to delete user');
    },
};

export default adminService;
