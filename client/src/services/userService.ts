import { API_USERS_URL } from "@/services/routes";

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
        if (!response.ok) throw new Error('Failed to register');
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

    async refreshToken(token: string): Promise<RefreshTokenDto> {
        const response = await fetch(`${API_USERS_URL}/refreshToken`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ token }),
        });
        if (!response.ok) throw new Error('Failed to refresh token');
        return response.json();
    }
};

export default api;