const API_BASE_URL = 'https://localhost:7161/api/users';

const api = {

    async login(email: string, password: string): Promise<User> {
        const response = await fetch(`${API_BASE_URL}/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password }),
        });
        if (!response.ok) throw new Error('Failed to login');
        return response.json();
    },

    async register(email: string, password: string): Promise<User> {
        const response = await fetch(`${API_BASE_URL}/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password }),
        });
        if (!response.ok) throw new Error('Failed to register');
        return response.json();
    },

    async forgotPassword(email: string): Promise<void> {
        const response = await fetch(`${API_BASE_URL}/forgotPassword`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email }),
        });
        if (!response.ok) throw new Error('Failed to send forgot password email');
        return response.json();
    }
};

export default api;