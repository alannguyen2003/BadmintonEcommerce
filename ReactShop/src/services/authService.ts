const AUTH_STORAGE_KEY = "react-shop-auth-token";
const USER_EMAIL_KEY = "react-shop-user-email";
const API_BASE = import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5080";

export const authService = {
  isAuthenticated: (): boolean => Boolean(localStorage.getItem(AUTH_STORAGE_KEY)),

  getToken: (): string | null => localStorage.getItem(AUTH_STORAGE_KEY),

  getUserEmail: (): string | null => localStorage.getItem(USER_EMAIL_KEY),

  login: async (email: string, password: string): Promise<string> => {
    const response = await fetch(`${API_BASE}/auth/login`, {
      method: "POST",
      headers: {
        "Accept": "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ email, password }),
    });

    if (!response.ok) {
      const message = await response.text();
      throw new Error(message || "Đăng nhập thất bại. Vui lòng kiểm tra lại email và mật khẩu.");
    }

    const data = (await response.json()) as string;

    if (!data) {
      throw new Error("Server trả về dữ liệu đăng nhập không hợp lệ.");
    }

    localStorage.setItem(AUTH_STORAGE_KEY, data);
    localStorage.setItem(USER_EMAIL_KEY, email);
    return data;
  },

  logout: () => {
    localStorage.removeItem(AUTH_STORAGE_KEY);
    localStorage.removeItem(USER_EMAIL_KEY);
  },
};
