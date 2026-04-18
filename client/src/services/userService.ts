import { API_USERS_URL, API_USERS_SETTINGS_URL, API_USERS_FORGOT_PASSWORD_URL, API_USERS_RESET_PASSWORD_URL, API_USERS_ACTIVATE_URL } from "@/services/routes";
import { apiFetch } from "@/services/apiClient";

const api = {

    async login(email: string, password: string): Promise<LoginDto> {
        const response = await fetch(`${API_USERS_URL}/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password }),
        });
        if (!response.ok) throw new Error('Failed to login');
        return response.json();
    },

    async register(email: string, password: string): Promise<RegisterDto> {
        const response = await fetch(`${API_USERS_URL}/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password }),
        });
        if (!response.ok) {
            const body = await response.json().catch(() => ({}));
            throw new Error(body.errorMessage ?? 'Failed to register');
        }
        return response.json();
    },

    async resetPassword(email: string): Promise<ResetPasswordDto> {
        const response = await fetch(`${API_USERS_URL}/resetPassword`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email }),
        });
        if (!response.ok) throw new Error('Failed to send reset password email');
        return response.json();
    },

    async refreshToken(token: string, refreshToken: string): Promise<RefreshTokenDto> {
        const response = await fetch(`${API_USERS_URL}/refreshToken`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ token, refreshToken }),
        });
        if (!response.ok) throw new Error('Failed to refresh token');
        return response.json();
    },

    async changePassword(currentPassword: string, newPassword: string): Promise<{ isSuccess: boolean; errorMessage?: string }> {
        const response = await apiFetch(`${API_USERS_URL}/changePassword`, {
            method: 'POST',
            body: JSON.stringify({ currentPassword, newPassword }),
        });
        if (!response.ok) {
            const body = await response.json().catch(() => ({}));
            throw new Error(body.errorMessage ?? 'Failed to change password');
        }
        return response.json();
    },

    async getSettings(): Promise<UserSettings> {
        const response = await apiFetch(API_USERS_SETTINGS_URL);
        if (!response.ok) throw new Error('Failed to load settings');
        return response.json();
    },

    async updateSettings(dto: UserSettings): Promise<void> {
        const response = await apiFetch(API_USERS_SETTINGS_URL, {
            method: 'PUT',
            body: JSON.stringify(dto),
        });
        if (!response.ok) throw new Error('Failed to save settings');
    },

    async forgotPassword(email: string): Promise<void> {
        const response = await fetch(API_USERS_FORGOT_PASSWORD_URL, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email }),
        });
        if (!response.ok) throw new Error('Failed to send reset code');
    },

    async confirmResetPassword(email: string, code: string, newPassword: string): Promise<void> {
        const response = await fetch(API_USERS_RESET_PASSWORD_URL, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, code, newPassword }),
        });
        if (!response.ok) throw new Error('Invalid or expired reset code');
    },

    async activateAccount(token: string): Promise<{ success: boolean; message: string }> {
        const response = await fetch(`${API_USERS_ACTIVATE_URL}?token=${encodeURIComponent(token)}`, {
            method: 'POST',
        });
        return response.json();
    },
};

export default api;